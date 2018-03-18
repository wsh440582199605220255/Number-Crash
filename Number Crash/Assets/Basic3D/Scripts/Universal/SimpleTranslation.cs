using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

namespace Basic2D
{
    /// <summary>
    /// （快速）周期变换_FixedUpdate
    /// </summary>
    public class SimpleTranslation : MonoBehaviour
    {
        public enum SimpleEase
        {
            Linear = 0,
            Accelerated,
            Decelerated,
        }

        [SerializeField] private float startTime = 2f;
        public float timeInterval = 2f;
        public float translateTime = 4f;
        public Vector2 targetVector = Vector2.zero;
        public bool isWaiting = false;

        public SimpleEase normal = SimpleEase.Linear;
        public SimpleEase reverse = SimpleEase.Linear;

        private float direction = 1f;
        private float restTime = 0f;
        private bool clockFlag = true;


        private Vector2 acceleration;           //加速度
        private Vector2 linearSpeed;            //线性速度
                                                // Use this for initialization
        void Start() {
            linearSpeed = targetVector / translateTime * Time.fixedDeltaTime;
            acceleration = 2 * targetVector / (translateTime * translateTime) * Time.fixedDeltaTime * Time.fixedDeltaTime;
        }

        private Vector2 speed;
        private Vector2 acc;
        private SimpleEase mode;

        private void FixedUpdate() {
            //开始之前
            if (startTime > 0f) {
                startTime -= Time.fixedDeltaTime;
                return;
            }

            if (isWaiting) {
                //间隔
                if (clockFlag) {
                    restTime = timeInterval;
                    clockFlag = false;
                }

                if (restTime > 0f) {
                    restTime -= Time.fixedDeltaTime;
                    return;
                }
                isWaiting = false;
                clockFlag = true;
            }
            else {
                //变换        
                if (clockFlag) {
                    restTime = translateTime;
                    clockFlag = false;
                    if (direction > 0f) mode = normal;
                    else mode = reverse;

                    switch (mode) {
                        case SimpleEase.Linear: {
                                speed = linearSpeed;
                                acc = Vector2.zero;
                                break;
                            }
                        case SimpleEase.Accelerated: {
                                speed = Vector2.zero;
                                acc = acceleration;
                                break;
                            }
                        case SimpleEase.Decelerated: {
                                speed = acceleration * translateTime / Time.fixedDeltaTime;
                                acc = -acceleration;
                                break;
                            }
                    }
                }


                if (restTime > 0f) {
                    restTime -= Time.fixedDeltaTime;

                    transform.position += (Vector3)speed * direction;
                    speed += acc;

                    return;
                }

                direction *= -1f;
                isWaiting = true;
                clockFlag = true;
            }
        }
    }
}