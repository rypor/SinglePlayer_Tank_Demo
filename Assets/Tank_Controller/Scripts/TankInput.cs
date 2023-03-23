using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace INoodleI
{
    public class TankInput : MonoBehaviour
    {
        #region Variables

        [Header("Input Properties")]
        public Camera camera;

        #endregion


        #region Properties

        private Vector3 reticlePosition;
        private Vector3 reticleNormal;
        private InputData inputData;

        public InputData InputData { get { return inputData; } }

        #endregion


        #region Builtin Methods

        private void Update()
        {
            if(camera)
            {
                HandleInputs();
            }
            else
            {
                Debug.LogError("No Camera Ref: Tank Input - " + gameObject);
            }
        }

        #endregion

        #region Custom Methods

        protected virtual void HandleInputs()
        {
            Vector2 movementInput = new Vector2(0, 0);
            bool fireBullet;
            bool plantMine;

            Vector3 reticleNormal = Vector3.zero;
            Vector3 reticlePosition = Vector3.zero;

            // Keyboard Movement Input
            if (Input.GetKey(KeyCode.W))
                movementInput.y++;
            if (Input.GetKey(KeyCode.S))
                movementInput.y--;
            if (Input.GetKey(KeyCode.A))
                movementInput.x--;
            if (Input.GetKey(KeyCode.D))
                movementInput.x++;

            // Fire Bullet Input
            fireBullet = Input.GetKey(KeyCode.Mouse0);

            // Plant Mine Input
            plantMine = Input.GetKey(KeyCode.Mouse1);

            // Mouse Target Input
            Ray screenRay = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(screenRay, out hit))
            {
                reticlePosition = hit.point;
                reticleNormal = hit.normal;
            }

            inputData = new InputData
            {
                movementInput = movementInput,
                firePressed = fireBullet,
                minePressed = plantMine,
                reticleNormal = reticleNormal,
                reticlePosition = reticlePosition,
            };
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
        }

        #endregion
    }

    public struct InputData
    {
        public Vector2 movementInput;
        public bool firePressed;
        public bool minePressed;

        public Vector3 reticlePosition;
        public Vector3 reticleNormal;
    }
}
