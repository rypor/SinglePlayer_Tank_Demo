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

        private InputData inputData;
        private Rigidbody rb;
        private TankInput input;
        
        [SerializeField] private TankStats stats;

        #endregion

        #region Builtin Methods

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<TankInput>();
        }

        private void Update()
        {
            if (input && stats)
            {

            }
            else
            {
                Debug.LogError("Tank Controller Error:  Input=" + input + ", Stats=" + stats);
            }
        }

        #endregion
    }
}
