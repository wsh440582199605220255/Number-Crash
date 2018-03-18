using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainLogic.UI
{
    public abstract class Basic_UI : MonoBehaviour
    {
        [SerializeField] private ChessNode currentNode;
        public ChessNode CurrentNode {
            get { return currentNode; }
            protected set {
                if (value == null)
                {
                    Debug.LogWarning("You mustn't provide a null value,please check again.\nThis call will redirect to [ResetCurrentNode()]");
                    ResetCurrentNode();
                    return;
                }
                if (currentNode && currentNode.chess != null) currentNode.chess.Effect_UnSelected();
                currentNode = value;
                if (currentNode.chess != null) currentNode.chess.Effect_Selected();
            }
        }
        [SerializeField] private Transform pointer;
        public Group PlayerGroup {
            get {
                if (GameManagement.PUBLIC != null) return GameManagement.PUBLIC.playerGroup;
                else return Group.orange;
            }
        }

        protected List<ChessNode> highLightNodes = new List<ChessNode>();

        public RaycastHit Click(LayerMask layer)
        {
            RaycastHit hit = new RaycastHit();

#if ANDROID
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return hit;
#else
            if (EventSystem.current.IsPointerOverGameObject()) return hit;
#endif
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layer);
            if (hit.transform != null)
            {
                pointer.transform.position = hit.transform.position;
            }
            return hit;
        }

        protected void HighLight()
        {
            for (int i = 0; i < highLightNodes.Count; i++) highLightNodes[i].Effect_Selected();
        }

        protected void ResetHighLight()
        {
            for (int i = 0; i < highLightNodes.Count; i++)
            {
                if (highLightNodes[i] != null) highLightNodes[i].Effect_UnSelected();
            }
            highLightNodes.Clear();
        }

        protected void ResetCurrentNode(bool closePointer = true)
        {
            if (currentNode != null)
            {
                if (currentNode.chess != null) currentNode.chess.Effect_UnSelected();
                currentNode = null;
                if (closePointer) pointer.transform.position += Vector3.right * 4800;
            }
        }
    }
}
