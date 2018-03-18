using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainLogic.UI
{
    public class MapEdit_UI : Basic_UI
    {
        public Text posText;

        private Vector3 mpLastFrame;
        private bool rmDown = false;
        private float cameraMoveRate;


        public static MapEdit_UI PUBLIC;
        private void Awake()
        {
            PUBLIC = this;
            cameraMoveRate = 1 / (5.4f / Camera.main.orthographicSize * 100);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = Click(QuadMap.nodeLayer);
                if (hit.transform)
                {
                    ResetHighLight();

                    CurrentNode = hit.transform.GetComponent<ChessNode>();
                    highLightNodes.Add(CurrentNode);
                    highLightNodes.AddRange(CurrentNode.ConnectedNodes());

                    HighLight();
                }
            }
            if (CurrentNode) posText.text = CurrentNode.pos.ToString();

            if (rmDown)
            {
                Vector3 dir = Input.mousePosition - mpLastFrame;
                Camera.main.transform.Translate(-dir * cameraMoveRate, Space.Self);
                mpLastFrame = Input.mousePosition;
            }
            if (Input.GetMouseButtonDown(1))
            {
                rmDown = true;
                mpLastFrame = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)) rmDown = false;
        }
    }
}