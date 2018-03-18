using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basic2D
{

    public class Rotate : MonoBehaviour
    {
        public float angularSpeed = 1f;
        public bool clockWise = true;

#if UNITY_EDITOR
#else
    private Vector3 rorateVector;
#endif

        // Use this for initialization
        void Start() {
#if UNITY_EDITOR
#else
        rorateVector = new Vector3(0, 0, (clockWise ? -1f : 1f) * angularSpeed);
#endif
        }

        void LateUpdate() {

#if UNITY_EDITOR
            transform.Rotate(new Vector3(0, 0, (clockWise ? -1f : 1f) * angularSpeed));
#else
        transform.Rotate(rorateVector);
#endif

        }
    }
}