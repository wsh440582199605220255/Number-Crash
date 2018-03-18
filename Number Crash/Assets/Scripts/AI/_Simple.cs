using System.Collections.Generic;
using UnityEngine;

namespace MainLogic.AI
{
    public class _Simple : Chess_AI
    {
        public override void StartUp()
        {
            List<ChessNode> nodes = null;

            Chess item = null;
            while (nodes == null || nodes.Count == 0)
            {
                int i = Random.Range(0, myChess.Count - 1);
                item = myChess[i];
                nodes = item.ConnectedNodes();
            }

            int j = Random.Range(0, nodes.Count - 1);
            //pre data
            ChessAction act = new ChessAction(item.Pos, nodes[j].pos);
            CType myType = item.type;

            //action
            CLogic result = item.MoveTo(nodes[j]);

            //deal with result
            switch (result)
            {
                case CLogic.less:
                    {
                        nodes[j].chess.SetTagData(new TagData(CLogic.bigger, myType), group);
                        break;
                    }
            }

            //send message to main logic
            GameManagement.PUBLIC.NextStep(act);
        }
    }
}