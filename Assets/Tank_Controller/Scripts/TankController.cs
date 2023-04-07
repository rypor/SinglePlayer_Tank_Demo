using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INoodleI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(TankInput))]
    public class TankController : MonoBehaviour
    {
        #region Variables

        [Header("Prefabs")]
        [SerializeField] private PrefabEnum BulletEnum;

        [Header("Grounding")]
        public Transform TreadCheck;

        private InputData inputData;
        private Rigidbody rb;
        private TankInput input;

        private bool _grounded;

        private float _treadSpeed;
        private float _treadRotSpeed;


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
            input = GetComponent<TankInput>();

            rb.centerOfMass = stats.CenterOfMass;
        }

        private void FixedUpdate()
        {
            if (input && stats && rb)
            {
                // Update Grounding
                _grounded = CheckGrounding(TreadCheck);
                CollectInput();
                CalculateMovement();
            }
            else
            {
                Debug.LogError("Tank Controller Error:  Input=" + input + ", Stats=" + stats+", Rb="+rb);
            }
        }

        #endregion

        #region Custom Methods

        private bool CheckGrounding(Transform t)
        {
            return Physics.CheckBox(t.position, stats.GroundingCheckSize, t.rotation, stats.GroundMask);
        }

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

                rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, _treadRotSpeed * Time.fixedDeltaTime, 0)));
                rb.MovePosition(rb.position + rb.rotation * Vector3.forward * _treadSpeed * Time.fixedDeltaTime);

            }
            // Air Movement Influence Logic
            else
            {
                rb.MovePosition(rb.position + rb.rotation * Vector3.forward * _treadSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, _treadRotSpeed * Time.fixedDeltaTime, 0)));
            }
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
            //Debug Ground Check Box
            Gizmos.color = Color.red;
            if(_grounded)
                Gizmos.color = Color.green;
            if(TreadCheck)
                DrawGizmoRect(TreadCheck.position, stats.GroundingCheckSize, TreadCheck.rotation);

            // Center Of Mass
            Gizmos.color = Color.red;
            if(stats && rb)
                Gizmos.DrawWireSphere(stats.CenterOfMass + rb.position, 0.2f);
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
