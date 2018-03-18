using System.Collections;
using UnityEngine;
using Basic3D;

public enum SkillType
{
    Attack = 0,
    Shoot,
    Defender,
}

public abstract class BaseSkill : MonoBehaviour
{
    /*
     * 技能可以交给SkillTimer类可视化（多用于玩家），也可以由InnerTimer管理
     * 只能通过Actor来访问施法者
    */
    [SerializeField] private float coldDownTime = 5f;
    [HideInInspector] public BaseActor m_Actor;
    [SerializeField] private float magicCost = 30f;

    public float SkillCd {
        get { return coldDownTime; }
        set {
            coldDownTime = value;
            if (m_SkillTimer) m_SkillTimer.ColdDownTime = value;
        }
    }

    [HideInInspector] public bool isCd = false;
    [HideInInspector] public SkillType skillType;

    [SerializeField] private GameObject skillTimer = null;
    private SkillTimer m_SkillTimer;

    protected void Init() {
        if (skillTimer) {
            m_SkillTimer = skillTimer.GetComponent<SkillTimer>();
            m_SkillTimer.ColdDownTime = this.coldDownTime;
            m_SkillTimer.TimeOut += Reset;
        }
    }

    private void Reset(GameObject from) { isCd = false; }

    public void ResetCd() {
        isCd = false;
        if (m_SkillTimer) m_SkillTimer.ResetCd();
        else StopCoroutine("InnerTimer");
    }

    protected abstract void _UseSkill();

    public bool UseSkill() {
        if (isCd) return false;
        if (m_Actor.m_State.mp < magicCost) return false;
        else {
            m_Actor.m_State.Change_MP(-magicCost);
        }
        _UseSkill();

        isCd = true;
        if (m_SkillTimer) m_SkillTimer.ResetCd();
        else StartCoroutine("InnerTimer");

        return true;
    }

    IEnumerator InnerTimer() {
        yield return new WaitForSeconds(coldDownTime);
        isCd = false;
    }

}
