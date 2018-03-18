using System.Collections.Generic;
using UnityEngine;

public enum NType
{
    basic,
    quick,
    camp,
    home,
    obstacle,
}

[DefaultExecutionOrder(-100)]
public class ChessNode : MonoBehaviour, IEffect_Selected
{
    static Vector2Int[] dir = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public NType type = NType.basic;
    public Vector2Int pos;
    public Chess chess;
    #region Layout Properties
    public Group groupInLayout = Group.neutral;
    [SerializeField] private bool canPlaceBoom = true;
    #endregion
    private List<ChessNode> cNodes = new List<ChessNode>();     //temp

    private SpriteRenderer myRenderer;
    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// return a node list which the chess can move to(commend using by player rather than AI)
    /// </summary>
    public List<ChessNode> ConnectedNodes()
    {
        cNodes.Clear();
        Vector2Int leftDown = pos - Vector2Int.one;
        Vector2Int rightUp = pos + Vector2Int.one;

        switch (type)
        {
            case NType.obstacle: return cNodes;
            case NType.home:
            case NType.basic:
                {
                    for (int i = 0; i < 4; i++) CheckNode(pos + dir[i]);
                    CheckNode(leftDown, NType.camp);
                    CheckNode(leftDown + Vector2Int.up * 2, NType.camp);
                    CheckNode(rightUp, NType.camp);
                    CheckNode(leftDown + Vector2Int.right * 2, NType.camp);
                    break;
                }
            case NType.camp:
                {
                    for (int i = leftDown.x; i <= rightUp.x; i++)
                    {
                        for (int j = leftDown.y; j <= rightUp.y; j++)
                        {
                            if (pos.x != i || pos.y != j) CheckNode(new Vector2Int(i, j));
                        }
                    }
                    break;
                }
            case NType.quick:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2Int nextPos = pos + dir[i];
                        ChessNode node = CheckNode(nextPos);
                        if (node != null && node.chess == null && node.type == NType.quick)
                        {
                            do
                            {
                                nextPos += dir[i];
                                node = CheckNode(nextPos, NType.quick);
                            }
                            while (node != null && node.chess == null);
                        }
                    }

                    CheckNode(leftDown, NType.camp);
                    CheckNode(leftDown + Vector2Int.up * 2, NType.camp);
                    CheckNode(rightUp, NType.camp);
                    CheckNode(leftDown + Vector2Int.right * 2, NType.camp);
                    break;
                }
        }
        return cNodes;
    }

    /// <summary>
    /// Check a node whether the player can move to
    /// </summary>
    /// <param name="fliter">Define the vaild types,if you don't provide,that means all.</param>
    private ChessNode CheckNode(Vector2Int pos, params NType[] fliter)
    {
        ChessNode node = QuadMap.PUBLIC.Find(pos);
        if (node != null)
        {
            if (fliter != null && fliter.Length > 0)
            {
                for (int i = 0; i < fliter.Length; i++) if (fliter[i] == node.type) goto VAILD;
                return null;
            }

            VAILD:
            if (node.chess != null)
            {
                if (chess.IsOpposite(node.chess) && node.type != NType.camp) cNodes.Add(node);
            }
            else
            {
                cNodes.Add(node);
            }
        }
        return node;
    }

    /// <summary>
    /// Reset the node to empty
    /// </summary>
    public void Clear()
    {
        chess = null;
    }

    /// <summary>
    /// return whether the node can place the specific chess
    /// </summary>
    private bool TryToPlace(Chess c)
    {
        if (c.group != groupInLayout) return false;

        if (c.type == CType.flag || c.type == CType.infinite)
        {
            if (type != NType.home) return false;
        }

        if (c.type == CType.boom) return canPlaceBoom;
        return true;
    }

    /// <summary>
    /// Swap chess between two nodes,return whether it is successful
    /// </summary>
    public bool SwapChess(ChessNode another)
    {
        bool result = true;
        if (chess != null) result = another.TryToPlace(chess);
        if (result == false) return false;
        if (another.chess != null) result = this.TryToPlace(another.chess);
        if (result == false) return false;

        Chess temp = this.chess;
        ResetCurrentChess(another.chess);
        another.ResetCurrentChess(temp);

        temp.Effect_UnSelected();
        return true;
    }

    /// <summary>
    /// Swap two chess without check
    /// </summary>
    public void SwapChessWithOutCheck(ChessNode another)
    {
        Chess temp = this.chess;
        ResetCurrentChess(another.chess);
        another.ResetCurrentChess(temp);
    }

    public void ResetCurrentChess(Chess c)
    {
        if (c == null) return;
        this.chess = c;
        chess.myNode = this;

        Vector3 alignPos = transform.position;
        alignPos.y = c.transform.position.y;
        c.transform.position = alignPos;
    }

    public void Effect_Selected()
    {
        myRenderer.color = new Color(1, 1, 1, 0.85f);
    }

    public void Effect_UnSelected()
    {
        myRenderer.color = new Color(1f, 1f, 1f, 0.32f);
    }
}
