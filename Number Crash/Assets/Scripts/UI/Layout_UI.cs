using MainLogic.NetWork;
using UnityEngine;

namespace MainLogic.UI
{
    public class Layout_UI : Basic_UI
    {
        public static Layout_UI PUBLIC;
        public GameObject layoutCpButton;

        private void Awake()
        {
            PUBLIC = this;
        }

        private void OnEnable()
        {
            GameManagement.PUBLIC.EnableHide();
            layoutCpButton.SetActive(true);
        }

        public void Completed()
        {
            ResetCurrentNode();
            this.enabled = false;

            GameManagement.PUBLIC.MyLayoutCompleted();
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
                Chess chess = hit.transform.GetComponent<Chess>();
                if (chess.group != PlayerGroup) return;

                CurrentNode = chess.myNode;
            }
        }

        public void SecondSelect()
        {
            RaycastHit hit = Click(QuadMap.chessLayer);
            if (hit.transform)
            {
                Chess chess = hit.transform.GetComponent<Chess>();

                if (CurrentNode.chess == chess)
                {
                    ResetCurrentNode();
                    return;
                }
                if (chess.group != PlayerGroup) return;

                if (CurrentNode.SwapChess(chess.myNode)) ResetCurrentNode();
            }
        }
    }
}
