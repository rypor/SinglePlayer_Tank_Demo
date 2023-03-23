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

        private Rigidbody rb;
        private TankInput input;

        #endregion

        #region Builtin Methods

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<TankInput>();
        }

        #endregion
    }
}
