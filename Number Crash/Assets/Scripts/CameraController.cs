using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainLogic
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private Transform target = null;
        public float smoothDampTime = 0.2f;

        private Vector3 _smoothDampVelocity;
        private Vector3 targetPos;
        private Vector3 nextPos;

        public static CameraController PUBLIC;
        private void Awake()
        {
            PUBLIC = this;
        }

        void LateUpdate()
        {
            if (target == null) return;

            targetPos = target.position;
            targetPos.y = transform.position.y;

            nextPos = Vector3.SmoothDamp(transform.position, targetPos, ref _smoothDampVelocity, smoothDampTime);
            transform.position = nextPos;
        }

        public void SetTarget(Chess c) { target = c.transform; }
        public void RemoveTarget() { target = null; }
    }
}
