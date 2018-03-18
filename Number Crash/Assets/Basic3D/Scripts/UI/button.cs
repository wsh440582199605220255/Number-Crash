using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{

    public GameObject target = null;
    private StoryMask storyMaskClass;

    private void Start() {
        storyMaskClass = target.GetComponent<StoryMask>();
    }

    public void OnClick() {
        storyMaskClass.MaskChange();
    }
    
}
