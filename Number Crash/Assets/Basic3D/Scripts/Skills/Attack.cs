using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace Basic3D
{
    public sealed class Attack : BaseSkill
    {
        [SerializeField]private float attackDuration = 0.65f;
        [SerializeField]private Transform attackPoint;
        [SerializeField]private string animationName = "Attack";

        [SerializeField]private AttackPoint m_AttackPoint;

        private void Awake() {
            base.Init();
            if (attackPoint == null) {
                m_AttackPoint = GetComponentInChildren<AttackPoint>();
                attackPoint = m_AttackPoint.transform;
            }
            else m_AttackPoint = attackPoint.GetComponent<AttackPoint>();

            m_AttackPoint.m_State = GetComponent<BaseState>();
            skillType = SkillType.Attack;
        }

        protected override void _UseSkill() {
            StartCoroutine(CloseAttack());
        }

        IEnumerator CloseAttack() {
            m_Actor.m_Ctrl.isLocked = true;
            m_Actor.m_Animator.Play(animationName);
            m_AttackPoint.IsAttacking = true;

            yield return new WaitForSeconds(attackDuration);

            m_AttackPoint.IsAttacking = false;
            m_AttackPoint.ClearList();
            m_Actor.m_Ctrl.isLocked = false;
        }
    }
}
*/