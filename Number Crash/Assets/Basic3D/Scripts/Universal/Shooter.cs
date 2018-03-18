using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basic2D
{
    /// <summary>
    /// 定时射击
    /// </summary>
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private float startTime = 2f;
        public float timeInterval = 2f;

        public GameObject obj;

        public List<Vector2> speeds = new List<Vector2> { new Vector2(0, -50f) };

        // Use this for initialization
        void Start() {
            if (obj == null) {
                Q.WarningPrint(transform, GetType().ToString());
            }
            StartCoroutine(DelayBeforeStart());
        }

        IEnumerator DelayBeforeStart() {
            yield return new WaitForSeconds(startTime);
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot() {
            while (true) {
                for (int i = 0; i < speeds.Count; i++) {
                    GameObject arrow = Instantiate(obj, transform.position, Quaternion.FromToRotation(Vector3.down, speeds[i]), transform);
                    arrow.GetComponent<Rigidbody2D>().velocity = speeds[i];
                }

                yield return new WaitForSeconds(timeInterval);
            }
        }
    }
}