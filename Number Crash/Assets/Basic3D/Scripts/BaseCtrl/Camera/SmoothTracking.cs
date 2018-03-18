using UnityEngine;

namespace Basic3D
{
    /// <summary>
    /// （Camera）平滑跟踪_LateUpdate
    /// </summary>
    public class SmoothTracking : MonoBehaviour
    {
        public Transform target;
        public float smoothDampTime = 0.2f;

        private Vector3 _smoothDampVelocity;
        private Vector3 targetPos;
        private Vector3 targetPrePos;


        void Start() {
            if (target == null) {
                target = GameObject.FindWithTag("Player").transform;    //自动搜索Tag为默认目标
                if (target == null) Q.WarningPrint(transform, this.GetType().ToString());
                return;
            }

            targetPos = target.transform.position;
            targetPrePos = target.transform.position;
        }

        void LateUpdate() {
            targetPos = target.transform.position;
            if (targetPos != targetPrePos) {
                Vector3 change = targetPos - targetPrePos;
                transform.position += change;
            }


            targetPrePos = targetPos;
        }

    }
}