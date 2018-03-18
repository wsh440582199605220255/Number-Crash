using UnityEngine;
using System.Collections.Generic;

namespace MainLogic.AI
{
    /// <summary>
    /// The base class of chess AI
    /// </summary>
    public abstract class Chess_AI : MonoBehaviour
    {
        public Group group;
        [SerializeField] protected List<Chess> myChess = new List<Chess>();
        protected List<Chess> targets = new List<Chess>();

        public virtual void OnGameStart()
        {
            myChess = GameManagement.PUBLIC.gDic[group];

            foreach (var item in GameManagement.PUBLIC.gDic)
            {
                if (IsOpposite(item.Key, group))
                {
                    foreach (var item1 in GameManagement.PUBLIC.gDic[item.Key])
                    {
                        if (item1.myNode.type == NType.home) targets.Add(item1);
                    }
                }
            }
        }

        /// <summary>
        /// The entrance of AI
        /// </summary>
        public abstract void StartUp();

        public static bool IsOpposite(Group g1, Group g2)
        {
            return ((int)g1 * (int)g2 < 0);
        }
    }
}
