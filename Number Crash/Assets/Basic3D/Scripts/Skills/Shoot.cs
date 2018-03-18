using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace Basic3D
{
    public sealed class Shoot : BaseSkill
    {
        [SerializeField] private float attackDuration = 0.65f;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private string animationName = "Attack";

        public GameObject bullet;               //Default bullet

        [SerializeField] private AttackPoint m_AttackPoint;
        // Use this for initialization
        void Awake() {
            base.Init();
            if (attackPoint == null) {
                m_AttackPoint = GetComponentInChildren<AttackPoint>();
                attackPoint = m_AttackPoint.transform;
            }
            else m_AttackPoint = attackPoint.GetComponent<AttackPoint>();

            m_AttackPoint.m_State = GetComponent<BaseState>();

            skillType = SkillType.Shoot;
        }

        protected override void _UseSkill() {
            GameObject newBullet = Instantiate(bullet);
            ShootPoint newShoot = newBullet.AddComponent<ShootPoint>();
            newBullet.transform.position = attackPoint.position;

            newShoot.m_State = m_AttackPoint.m_State;

            newShoot.deathEffect = Q.Objs["ammoDeath"];

            StartCoroutine(CloseShoot());
        }

        IEnumerator CloseShoot() {
            m_Actor.m_Ctrl.isLocked = true;
            m_Actor.m_Animator.Play(animationName);
            yield return new WaitForSeconds(attackDuration);
            m_Actor.m_Ctrl.isLocked = false;
        }
    }
}
*/