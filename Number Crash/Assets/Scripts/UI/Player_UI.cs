using System.Collections.Generic;
using UnityEngine;

namespace MainLogic.UI
{
    public class Player_UI : Basic_UI
    {
        public static Player_UI PUBLIC;
        private void Awake()
        {
            PUBLIC = this;
        }


        private void OnEnable()
        {

        }


        // Update is called once per frame
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentNode == null) FirstSelect();
                else SecondSelect();
            }
        }


        public void FirstSelect()
        {
            //try to get a chess and its node
            RaycastHit hit = Click(QuadMap.chessLayer);
            if (hit.transform)
            {
                ResetHighLight();
                Chess chess = hit.transform.GetComponent<Chess>();
                //CameraController.PUBLIC.SetTarget(chess);

                //a player just can select his own chess
                if (chess.group != PlayerGroup)
                {
                    //start tag system
                    ResetCurrentNode();
                    return;
                }

                List<ChessNode> nodes = chess.ConnectedNodes();
                if (nodes != null) highLightNodes.AddRange(nodes);
                else return;

                CurrentNode = chess.myNode;
                HighLight();
            }
        }

        public void SecondSelect()
        {
            RaycastHit hit = Click(QuadMap.nodeLayer);
            if (hit.transform)
            {
                ChessNode node = hit.transform.GetComponent<ChessNode>();

                //a own chess is selected,try to select the target node
                if (highLightNodes.Contains(node))
                {

                    ChessAction act = new ChessAction(CurrentNode.pos, node.pos);
                    CurrentNode.chess.MoveTo(node);
                    GameManagement.PUBLIC.NextStep(act);

                    //[CurrentNode.pos]上的chess.MoveTo([node.pos]上的node)                   
                    ResetCurrentNode(false);
                }
                else
                {
                    FirstSelect();
                    return;
                }

                ResetHighLight();
            }
        }

        private void OnDisable()
        {
            ResetCurrentNode();
            ResetHighLight();
        }
    }
}
