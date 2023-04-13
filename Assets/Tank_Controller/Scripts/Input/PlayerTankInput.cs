using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace INoodleI
{
    public class PlayerTankInput : MonoBehaviour, ITankInput
    {
        #region Variables

        //[Header("Input Properties")]
        private Camera cam;

        #endregion


        #region Properties

        private Vector3 reticlePosition;
        private Vector3 reticleNormal;
        private InputData inputData;

        public InputData InputData { get { return inputData; } }

        #endregion


        #region Builtin Methods

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if(cam)
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
            // Variable Declaration
            Vector2 movementInput = new Vector2(0, 0);
            bool fireBullet;
            bool plantMine;

            bool reticleHit;
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
            fireBullet = Input.GetKeyDown(KeyCode.Mouse0);

            // Plant Mine Input
            plantMine = Input.GetKeyDown(KeyCode.Mouse1);

            // Mouse Target Input
            Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(screenRay, out hit))
            {
                reticleHit = true;
                reticlePosition = hit.point;
                reticleNormal = hit.normal;
            }
            else
            {
                reticleHit = false;
            }

            // Compile Data
            inputData = new InputData
            {
                Movement = movementInput,
                FirePressed = fireBullet,
                MinePressed = plantMine,
                ReticleHit = reticleHit,
                ReticleNormal = reticleNormal,
                ReticlePosition = reticlePosition,
            };
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
        }

        #endregion
    }
}
