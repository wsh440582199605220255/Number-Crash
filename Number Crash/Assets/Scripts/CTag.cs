using MainLogic;
using System.Collections.Generic;
using UnityEngine;


namespace MainLogic
{
    public enum CLogic
    {
        bigger = 1,                  //the chess is bigger than main
        less = -1,                   //the chess is less than main
        equal = 0,                   //the chess is equal to main
        approximate,                 //the chess is may be main
        empty,                       //...
    }

    public struct TagData
    {
        public CLogic logic;
        public CType main;

        public TagData(CLogic logic, CType main)
        {
            this.logic = logic;
            this.main = main;
        }
    }
}


/// <summary>
/// tag of the chess
/// </summary>
public class CTag : MonoBehaviour
{

    //private SpriteRenderer spRenderer;
    public Dictionary<Group, TagData> tags = new Dictionary<Group, TagData>(4);

    private void Awake()
    {
        //spRenderer = GetComponent<SpriteRenderer>();
        foreach (var item in GameManagement.PUBLIC.players) tags.Add(item, new TagData(CLogic.equal, CType.untagged));
    }
}
