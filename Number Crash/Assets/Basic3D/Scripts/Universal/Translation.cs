using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Basic2D
{
    /// <summary>
    /// （高级）周期变换
    /// </summary>
    public class Translation : MonoBehaviour
    {

        [SerializeField] private float startTime = 2f;
        public float timeInterval = 2f;
        public float translateTime = 4f;
        public Vector2 targetVector = Vector2.zero;

        public Ease normal = Ease.Linear;
        public Ease reverse = Ease.Linear;

        private float direction = 1f;

        // Use this for initialization
        void Start() {
            StartCoroutine(DelayBeforeStart());
        }

        IEnumerator DelayBeforeStart() {
            yield return new WaitForSeconds(startTime);
            StartCoroutine(StartChange());
        }

        IEnumerator StartChange() {
            while (true) {
                transform.DOMove(transform.position + (Vector3)targetVector * direction, translateTime).SetEase<Tween>(direction > 0f ? normal : reverse);
                yield return new WaitForSeconds(timeInterval + translateTime);
                direction *= -1;
            }
        }

    }
}