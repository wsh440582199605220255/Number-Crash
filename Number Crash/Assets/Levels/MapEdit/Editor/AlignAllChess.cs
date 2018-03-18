using UnityEditor;
using UnityEngine;

public class AlignAllChess : ScriptableWizard
{
    public LayerMask nodeLayer;
    public GameObject tag;

    [MenuItem("Tools/Chess/Aligh All Chess")]
    static void CreateWizerd()
    {
        DisplayWizard("Aligh All Chess", typeof(AlignAllChess), "Align");
    }

    private void OnEnable()
    {
        nodeLayer = LayerMask.GetMask("CNode");
    }

    private void OnWizardCreate()
    {
        Chess[] cs = FindObjectsOfType<Chess>();
        ChessNode[] ns = FindObjectsOfType<ChessNode>();
        for (int i = 0; i < ns.Length; i++) ns[i].Clear();

        RaycastHit hit;
        for (int i = 0; i < cs.Length; i++)
        {
            //add tag if wanted
            if (tag != null)
            {
                if (cs[i].transform.Find("tag") == null)
                {
                    GameObject obj = Instantiate(tag, cs[i].transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.name = "tag";
                }
            }

            bool result = Physics.Raycast(cs[i].transform.position + Vector3.up * 5, Vector3.down, out hit, Mathf.Infinity, nodeLayer);
            if (result)
            {
                cs[i].myNode = hit.transform.GetComponent<ChessNode>();
                cs[i].myNode.chess = cs[i];

                cs[i].myNode.ResetCurrentChess(cs[i]);
            }
            else
            {
                Debug.LogWarning("A chess has aligning failed.", hit.transform);
            }
        }

        for (int i = 0; i < ns.Length; i++)
        {
            if (ns[i].chess != null) ns[i].groupInLayout = ns[i].chess.group;
            else ns[i].groupInLayout = Group.neutral;
        }

        Debug.Log("You should click [Save Scene As] and override the previous file,or the change maybe lost.");
        AssetDatabase.SaveAssets();
    }
}
