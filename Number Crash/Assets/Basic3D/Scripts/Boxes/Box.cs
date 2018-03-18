using BoxManager;
using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 物品实例
/// </summary>
[Serializable]
public class Box
{
    //static Dictionary<string, BoxTypeData> boxes = new Dictionary<string, BoxTypeData>();

    public string name;
    public BoxTypeData m_type;
    public int count = 1;

    public static void initData() {
        //string boxesStr = Resources.Load("boxtypedatas.txt").ToString();
        //string propertyTypes = Resources.Load("propertyTypes.txt").ToString();

        /*
        for (int i = 0; i < boxesStr.Length; i++) {
            BoxTypeData data = BoxTypeData.GetData(boxesStr[i], propertyTypes);
            if (data != null) boxes.Add(data.boxType.name, data);
        }*/
    }

}
