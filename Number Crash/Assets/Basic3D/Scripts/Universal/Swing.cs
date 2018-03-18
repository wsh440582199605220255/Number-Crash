using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basic2D
{
    public class Swing : MonoBehaviour
    {
        public enum Mode
        {
            Swing = 0,          //单摆模式
            Float,              //悬浮模式
        }

        public float A = 2.7f;                //acceleration of gravity divide the length

        public Mode mode = Mode.Swing;

        private float currSpeed = 0f;
        private float currAcceleration;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void FixedUpdate() {

            switch (mode) {
                case Mode.Swing: {
                        currAcceleration = A * Mathf.Sin(transform.eulerAngles.z / 180f * Mathf.PI);

                        currSpeed += currAcceleration * Time.fixedDeltaTime;

                        transform.Rotate(new Vector3(0, 0, -currSpeed));
                        break;
                    }
                case Mode.Float: {
                        float DeltaHeight = (Mathf.Sin(currSpeed + Time.fixedDeltaTime) - Mathf.Sin(currSpeed));

                        transform.position += new Vector3(0, DeltaHeight * A, 0);

                        currSpeed += Time.fixedDeltaTime;
                        break;
                    }
            }

        }

    }
}