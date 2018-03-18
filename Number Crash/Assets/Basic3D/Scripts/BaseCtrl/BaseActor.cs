using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basic3D
{
    [RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// Physics-based
    /// </summary>
    [DefaultExecutionOrder(100)]
    public abstract class BaseActor : MonoBehaviour
    {
        internal Dictionary<SkillType, BaseSkill> skillBook = new Dictionary<SkillType, BaseSkill>();

        //public float damping = 0.9f;            //转向速度衰减
        public float forceMultiplyer = 5f;

        [HideInInspector] public Animator m_Animator;
        [HideInInspector] public BaseCtrl m_Ctrl;
        [HideInInspector] public BaseState m_State;
        private Rigidbody m_rigidbody;

        public float maxSpeed = 1.2f;
        public float acceleration = 1f;         //每秒加速度
        public float speed = 0;                 //当前速度

        /// <summary>
        /// 在派生类中调用以进行初始化
        /// </summary>
        protected void Awake() {
            m_State = GetComponent<BaseState>();
            m_rigidbody = GetComponent<Rigidbody>();
            BaseSkill[] m_Skills = GetComponents<BaseSkill>();
            foreach (var item in m_Skills) {
                item.m_Actor = this;
                skillBook.Add(item.skillType, item);
            }

            m_Ctrl = GetComponent<BaseCtrl>();
            m_Animator = GetComponent<Animator>();

            //m_rigidbody.velocity = maxSpeed * transform.forward;
        }
        
        public void Move(Vector3 dir) {
            if (dir != Vector3.zero) {
                dir = dir.normalized;

                speed += Time.deltaTime * acceleration;
                if (speed > maxSpeed) speed = maxSpeed;

                m_rigidbody.velocity = speed * dir;
                transform.forward = Vector3.Slerp(transform.forward, dir, 0.2f);
            }
            else {
                speed = 0;
            }
        }

        //private Vector3 otherVelocity = Vector3.zero;
        public void Fly(Vector3 dir) {
            if (dir != Vector3.zero) {
                dir = dir.normalized;

                speed += Time.deltaTime * acceleration;
                if (speed > maxSpeed) speed = maxSpeed;

                m_rigidbody.velocity += dir * Time.deltaTime;
                //transform.forward = Vector3.Slerp(transform.forward, dir, 0.2f);
            }
            else {
                speed = 0;
            }
        }

        public void GoForward() {
            transform.position += maxSpeed * transform.forward * Time.deltaTime;
        }


        /// <summary>
        /// 命令Actor实例使用技能
        /// </summary>
        /// <param name="skillName">技能类型</param>
        public void UseSkill(SkillType skillType) {
            skillBook[skillType].UseSkill();
        }
    }
}