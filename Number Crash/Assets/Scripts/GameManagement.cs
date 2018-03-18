using MainLogic.NetWork;
using MainLogic.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainLogic
{
    [DefaultExecutionOrder(-1000)]
    public class GameManagement : MonoBehaviour
    {
        public Group playerGroup = Group.orange;
        public Dictionary<Group, List<Chess>> gDic = new Dictionary<Group, List<Chess>>(4);
        public Dictionary<Group, int> chessCannotMove = new Dictionary<Group, int>(4);
        public static GameManagement PUBLIC;

        public int step = 0;
        public int cycle = 0;
        public int currPlayerIndex = -1;
        public List<Group> players = new List<Group>();
        public List<AI.Chess_AI> ais = new List<AI.Chess_AI>(3);

        public bool hideMode = false;
        private bool isEnded = false;

        public GameObject layoutCpButton;
        public Text messageText;
        #region Network

        [SerializeField] private bool networkMode = false;
        /// <summary>
        /// Check the network vaild
        /// </summary>
        public bool CheckNetwork()
        {
            if (networkMode == false) return false;
            return true;
        }

        //a player complete his layout
        public void CompleteGroupLayout(CmdData data)
        {
            Group g = data.sender;
            Debug.Log("[" + g.ToString() + "] layout completed.");
            List<ChessData> cs = data.datas;

            int cnt = gDic[g].Count;
            bool[] isChecked = new bool[cnt];
            for (int i = 0; i < cnt; i++) isChecked[i] = false;

            foreach (var item in cs)
            {
                int i = -1;
                foreach (var item1 in gDic[g])
                {
                    i++;
                    if (isChecked[i]) continue;
                    if (item.type == item1.type)
                    {
                        item1.myNode.SwapChessWithOutCheck(QuadMap.PUBLIC.Find(item.pos));
                        isChecked[i] = true;
                        break;
                    }
                }
            }
        }

        public void DisplayChat(string text, Group g) { }

        public void RunChessAction(ChessAction act)
        {
            Debug.Log("Get a chess action.");
            QuadMap.PUBLIC.Find(act.chessPos).chess.MoveTo(QuadMap.PUBLIC.Find(act.targetNodePos));

            NextStep(ChessAction.NULL);
        }
        #endregion


        private void Awake()
        {
            PUBLIC = this;
            gDic.Add(Group.orange, new List<Chess>());
            gDic.Add(Group.green, new List<Chess>());
            gDic.Add(Group.blue, new List<Chess>());
            gDic.Add(Group.purple, new List<Chess>());
            chessCannotMove.Add(Group.orange, 0);
            chessCannotMove.Add(Group.green, 0);
            chessCannotMove.Add(Group.blue, 0);
            chessCannotMove.Add(Group.purple, 0);
        }

        public void EnableLayout()
        {
            if (hideMode)
            {
                foreach (var item in gDic)
                {
                    if (item.Key != playerGroup) foreach (var item1 in item.Value) item1.HideNumber();
                }
            }

            messageText.text = "";
            layoutCpButton.SetActive(true);
            Layout_UI.PUBLIC.enabled = true;
        }

        public void MyLayoutCompleted()
        {
            if (CheckNetwork())
            {
                List<ChessData> datas = new List<ChessData>(gDic[playerGroup].Count);
                foreach (var item in gDic[playerGroup])
                {
                    //pack all chess [type][pos]
                    datas.Add(new ChessData(item.type, item.myNode.pos));
                }

                messageText.text = "Waiting...";
                Socket_Client.SendChessDatas(datas);
            }
            else GameStart();
        }

        public void GameStart()
        {
            for (int i = 0; i < ais.Count; i++) ais[i].OnGameStart();

            currPlayerIndex = -1;
            NextStep(ChessAction.NULL);
        }

        public void NextStep(ChessAction act)
        {
            if (!act.IsNULL() && CheckNetwork() && step > 0) Socket_Client.SendChessAction(act);
            if (isEnded) return;
            step++;

            currPlayerIndex = (currPlayerIndex + 1) % players.Count;
            //CameraController.PUBLIC.RemoveTarget();

            if (players[currPlayerIndex] == playerGroup)
            {
                //your turn
                messageText.text = "Your turn";
                Player_UI.PUBLIC.enabled = true;
                cycle++;
            }
            else
            {
                Player_UI.PUBLIC.enabled = false;
                messageText.text = "Waiting others";
                for (int i = 0; i < ais.Count; i++)
                {
                    if (ais[i].group == players[currPlayerIndex]) ais[i].StartUp();
                }
            }
        }

        /// <summary>
        /// Check if there is any active chess in [Group g],setfailed if not
        /// </summary>
        public void CheckActive(Group g)
        {
            if (isEnded) return;
            if (gDic[g].Count <= chessCannotMove[g]) SetFailed(g);
        }

        public void SetFailed(Group g)
        {
            players.Remove(g);
            foreach (var item in gDic[g]) { item.Die(true); }
            gDic[g].Clear();

            if (players.Count == 1)
            {
                EndTheGame();
            }
        }

        public void DisPlayTheFlag(Group g)
        {
            foreach (var item in gDic[g])
            {
                if (item.type == CType.flag)
                {
                    item.DisplayNumber();
                    break;
                }
            }
        }

        public void EndTheGame()
        {
            isEnded = true;
            Player_UI.PUBLIC.enabled = false;
            messageText.text = "Player [" + players[0].ToString() + "] is the winner!";
            Debug.Log("The end.");
        }
    }
}