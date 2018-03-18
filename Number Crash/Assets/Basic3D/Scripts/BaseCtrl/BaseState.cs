using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basic3D
{
    public abstract class BaseState : MonoBehaviour
    {
        public float hp = 100;
        public float mp = 100;
        public float maxHp = 100;
        public float maxMp = 100;
        //public float attackForce = 0;

        public float GetData(int index) {
            switch (index) {
                case 0: return hp;
                case 1: return mp;
            }
            return 0;
        }

        [SerializeField] protected GameObject hp_bar;
        [SerializeField] protected GameObject mp_bar;
        [HideInInspector] public BaseActor m_Actor;

        private UpdateBar HP_barClass;
        private UpdateBar MP_barClass;

        /// <summary>
        /// 无敌的
        /// </summary>
        public bool Invincible { get; set; }

        public LayerMask safeLayers = -1;    //安全层

        /// <summary>
        /// 在派生类中调用以进行初始化
        /// </summary>
        protected void Init() {
            m_Actor = GetComponent<BaseActor>();
            if (hp_bar) {
                HP_barClass = hp_bar.GetComponent<UpdateBar>();
                HP_barClass.currSlashValue = hp;
                HP_barClass.MaxSlashValue = maxHp;
            }
            if (mp_bar) {
                MP_barClass = mp_bar.GetComponent<UpdateBar>();
                MP_barClass.currSlashValue = mp;
                MP_barClass.MaxSlashValue = maxMp;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            //检查collison是否包含在safeLayers中
            if (!Invincible && (safeLayers >> collision.gameObject.layer & 1) == 0) {
                Change_HP(-HP_barClass.currSlashValue);
            }
        }

        public abstract void OnDeath();

        public virtual void Change_HP(float change, bool hit = true) {
            if (Invincible && change < 0f) return;
            hp += change;
            if (hp > maxHp) hp = maxHp;
            if (HP_barClass) HP_barClass.LinearTarget = hp;

            if (hp < Mathf.Epsilon) {
                OnDeath();
                return;
            }

            if (hit && change < 0) StartCoroutine(BeHit());
        }

        public virtual void Change_MP(float change) {
            mp += change;
            if (mp > maxMp) mp = maxMp;
            if (MP_barClass) MP_barClass.LinearTarget = mp;
        }

        protected virtual IEnumerator BeHit() { yield return 0; }
    }
}