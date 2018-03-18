using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能定时器
/// </summary>
public sealed class SkillTimer : Timer
{
    private Image m_image;
    private Text restTimeText;
    public bool restTimeDisplay = true;  

    // Use this for initialization
    void Start() {
        m_image = transform.Find("mask").GetComponent<Image>();
        restTimeText = transform.Find("restTimeText").GetComponent<Text>();
    }

    protected override void _AfterUpdate() {
        m_image.fillAmount = restTime / coldDownTime;
        if(restTimeDisplay) restTimeText.text = isCd ? restTime.ToString("0.0") : "";       
    }

}
