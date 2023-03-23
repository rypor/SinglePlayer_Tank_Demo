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

        public Vector3 ReticlePosition
        {
            get { return reticlePosition; }
        }
        public Vector3 ReticleNormal
        {
            get { return reticleNormal; }
        }


        #endregion


        #region Builtin Methods

        private void Update()
        {
            if(camera)
            {
                HandleInputs();
            }
        }

        #endregion

        #region Custom Methods

        protected virtual void HandleInputs()
        {
            Ray screenRay = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(screenRay, out hit))
            {
                reticlePosition = hit.point;
                reticleNormal = hit.normal;
            }
        }

        #endregion

        #region Debug

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(reticlePosition, 0.5f);
        }

        #endregion
    }
}
