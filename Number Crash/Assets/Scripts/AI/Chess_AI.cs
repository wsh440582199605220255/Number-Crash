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
        public List<Chess> myChess;

        private void Start()
        {
            myChess = GameManagement.PUBLIC.gDic[group];
        }


        /// <summary>
        /// The entrance of AI
        /// </summary>
        public abstract void StartUp();
    }
}
