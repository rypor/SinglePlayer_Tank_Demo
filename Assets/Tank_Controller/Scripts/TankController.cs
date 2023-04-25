using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace INoodleI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ITankInput))]
    public class TankController : MonoBehaviour, ITankController
    {
        #region Variables

        [Header("Prefabs")]
        [SerializeField] private PrefabEnum BulletEnum;

        [Header("Grounding")]
        public Transform TreadCheck;
        public List<Transform> SlopeCheckPoints;

        private ITankInput input;
        private Rigidbody rb;
        private InputData inputData;
        private bool _hasInput = false;

        private bool _grounded;
        //private Vector3 _avgGroundNorm;
        //private float _groundAngleFromFlat;

        private float _treadSpeed;
        private float _treadRotSpeed;

        private Vector3 _momentum;

        private bool MoveInputPressed => (Mathf.Abs(inputData.Movement.y) > stats.InputDeadzone);
        private bool TurnInputPressed => (Mathf.Abs(inputData.Movement.x) > stats.InputDeadzone);
        private bool ChangeMoveDir => MoveInputPressed && (Mathf.Abs(_treadSpeed) > 0.05f && Mathf.Sign(_treadSpeed) != Mathf.Sign(inputData.Movement.y));
        private bool ChangeTurnDir => TurnInputPressed && (Mathf.Abs(_treadRotSpeed) > 0.05f && Mathf.Sign(_treadRotSpeed) != Mathf.Sign(inputData.Movement.x));
        
        [SerializeField] private TankStats stats;

        #endregion

        #region Builtin Methods

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<ITankInput>();
            _hasInput = true;

            rb.centerOfMass = stats.CenterOfMass;
        }

        private void FixedUpdate()
        {
            if (_hasInput && stats && rb)
            {
                // Update Grounding
                _grounded = CheckGrounding(TreadCheck);
                //if (_grounded) FindAverageGroundSlope();

                CollectInput();
                CalculateMovement();
            }
            else
            {
                Debug.LogError("Tank Controller Error:  Input=" + input + ", Stats=" + stats+", Rb="+rb);
            }
        }

        #endregion

        #region ITankController Methods

        public void StopMotion()
        {
            _treadSpeed = 0;
            _treadRotSpeed = 0;
            _momentum = Vector3.zero;
            _grounded = false;
        }

        public void OnExplode()
        {

        }

        #endregion

        #region Custom Methods

        private bool CheckGrounding(Transform t)
        {
            return Physics.CheckBox(t.position, stats.GroundingCheckSize, t.rotation, stats.GroundMask);
        }

        // Deprecated
        /*private void FindAverageGroundSlope()
        {
            int count = 0;
            Vector3 avgNorm = Vector3.zero;

            RaycastHit hit;
            foreach (Transform point in SlopeCheckPoints)
            {
                if (Physics.Raycast(point.position, Vector3.down, out hit, stats.GroundingCheckSize.y * 2, stats.GroundMask))
                {
                    avgNorm += hit.normal;
                    count++;
                }
            }
            
            _avgGroundNorm = (count == 0)? Vector3.zero : avgNorm / count;
            _groundAngleFromFlat = Vector3.Angle(_avgGroundNorm, Vector3.up);
        }*/

        private void CollectInput()
        {
            inputData = input.InputData;
        }

        private void CalculateMovement()
        {
            // Ground Movement Logic
            if(_grounded)
            {
                float targetMoveSpd = stats.MaxSpeed * inputData.Movement.y;
                float targetRotSpd = stats.RotMaxSpeed * inputData.Movement.x;

                float accel = (MoveInputPressed)? ( (ChangeMoveDir)? stats.Decceleration :stats.Acceleration ) : stats.Decceleration;
                float rotAccel = (TurnInputPressed)? ( (ChangeTurnDir)? stats.RotDecceleration :stats.RotAcceleration ) : stats.RotDecceleration;

                _treadSpeed = Mathf.Lerp(_treadSpeed, targetMoveSpd, accel * Time.fixedDeltaTime);
                _treadRotSpeed = Mathf.Lerp(_treadRotSpeed, targetRotSpd, rotAccel * Time.fixedDeltaTime);
                //_treadSpeed = Utils.FloatStepWithIncrementTarget(_treadSpeed, targetMoveSpd, accel * Time.fixedDeltaTime);
                //_treadRotSpeed = Utils.FloatStepWithIncrementTarget(_treadRotSpeed, targetRotSpd, rotAccel * Time.fixedDeltaTime);

                // Reduce tank's ability to climb slopes. Only effective if tank is traveling uphill
                if (transform.forward.y * _treadSpeed > 0.1)
                {
                    float _angleFromFlat = Vector3.SignedAngle(transform.forward * _treadSpeed, new Vector3(transform.forward.x, 0, transform.forward.y), transform.right);
                    if (_angleFromFlat > stats.MinSlopeAngle)
                    {
                        if (_angleFromFlat > 90) _angleFromFlat = 180 - _angleFromFlat;
                        Debug.Log("GOING UP SLOPE: "+_angleFromFlat);
                        float stepIncrement = Mathf.Clamp01((_angleFromFlat - stats.MinSlopeAngle) / (stats.MaxSlopeAngle - stats.MinSlopeAngle))
                            * Physics.gravity.y * stats.SlopeGravityModifier * Time.fixedDeltaTime;
                        // Debug.Log("_angleFromFlat: " + _angleFromFlat + ",  sInc: "+stepIncrement);
                        _treadSpeed = Utils.FloatStepWithIncrementTarget(_treadSpeed, 0, stepIncrement);
                    }
                    //_treadSpeed += _angleFromFlat / 90 * Physics.gravity.y * Time.fixedDeltaTime;
                }

                Vector3 targetDir = rb.rotation * Vector3.forward * _treadSpeed;
                _momentum = Vector3.Lerp(_momentum, targetDir, stats.TreadSpeedToMomentumMultiplier * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, _treadRotSpeed * Time.fixedDeltaTime, 0)));
                rb.MovePosition(rb.position + _momentum * Time.fixedDeltaTime);
                rb.AddForce(Vector3.down * stats.GroundingForce * Time.fixedDeltaTime, ForceMode.Impulse);
                PaintManager.instance.PaintSphere(rb.position, 1.5f, 0.1f, Color.red);
            }
            // Air Movement Influence Logic
            else
            {
                _treadSpeed = Utils.FloatStepWithIncrementTarget(_treadSpeed, 0, stats.TreadSpeedReductionInAir * Time.fixedDeltaTime);
                _treadRotSpeed = Utils.FloatStepWithIncrementTarget(_treadRotSpeed, 0, stats.TreadSpeedReductionInAir * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + _momentum * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, _treadRotSpeed * Time.fixedDeltaTime, 0)));
            }
        }

        #endregion

        #region Debug

        [Header("Debug Toggles")]
        public bool DebugTreadBox = false;
        public bool DebugCenterOfMass = false;
        public bool DebugSlopeCheck = false;

        private void OnDrawGizmos()
        {
            //Debug Ground Check Box
            if (DebugTreadBox)
            {
                Gizmos.color = Color.red;
                if (_grounded)
                    Gizmos.color = Color.green;
                if (TreadCheck)
                    DrawGizmoRect(TreadCheck.position, stats.GroundingCheckSize, TreadCheck.rotation);
            }

            // Center Of Mass
            if (DebugCenterOfMass)
            {
                Gizmos.color = Color.red;
                if (stats && rb)
                    Gizmos.DrawSphere(stats.CenterOfMass + rb.position, 0.1f);
            }

            // Slope Check
            if (DebugSlopeCheck)
            {
                RaycastHit hit;
                foreach (Transform point in SlopeCheckPoints)
                {
                    Gizmos.color = Color.red;
                    Vector3 center = point.position + Vector3.down * stats.GroundingCheckSize.y * 2;
                    if (Physics.Raycast(point.position, Vector3.down, out hit, stats.GroundingCheckSize.y * 2, stats.GroundMask))
                    {
                        Gizmos.color = Color.green;
                        center = hit.point;
                    }
                }
            }
        }

        private void DrawGizmoRect(Vector3 center, Vector3 size, Quaternion rot)
        {
            // Calculate rotated Box Edge points
            Vector3[] points = new Vector3[] {
                new Vector3(size.x, size.y, size.z),
                new Vector3(size.x, size.y, -size.z),
                new Vector3(size.x, -size.y, size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(-size.x, size.y, size.z),
                new Vector3(-size.x, size.y, -size.z),
                new Vector3(-size.x, -size.y, size.z),
                new Vector3(-size.x, -size.y, -size.z),
                };
            RotateAround(points, rot);

            // Connect all Box Edges with each other for full X box
            for(int i=0; i< 7; i++)
            {
                for(int j = 1; j<8; j++)
                {
                    Gizmos.DrawLine(points[i] + center, points[j] + center);
                }
            }
        }
        private void RotateAround (Vector3[] points, Quaternion rot)
        {
            for(int i = 0; i<points.Length; i++)
            {
                points[i] = rot * points[i];
            }
        }

        #endregion
    }
}
