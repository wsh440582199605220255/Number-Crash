using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Defender : BaseSkill
{
    public GameObject defender;
    public float duration = 3f;

    // Use this for initialization
    void Awake() {
        base.Init();
        if (defender == null) Q.WarningPrint(transform, GetType().ToString());
        if (defender.activeSelf) defender.SetActive(false);

        skillType = SkillType.Defender;
    }


    protected override void _UseSkill() {
        StartCoroutine(CloseDefender());
    }

    IEnumerator CloseDefender() {
        defender.SetActive(true);
        yield return new WaitForSeconds(duration);
        defender.SetActive(false);
    }

}

