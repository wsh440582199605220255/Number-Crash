using System.Collections.Generic;
using UnityEngine;
using MainLogic;

public enum Group
{
    orange = 1,        //player 0
    green = 2,         //player 1
    blue = -1,         // enemy 0
    purple = -2,       // enemy 1
    neutral = 0,       // neutral
}

public enum CType
{
    untagged = -1,
    boom = 0, flag = 1,
    two, three, four, five, six, seven,         //remove "8" because it seems like infinite
    nine = 9,
    infinite,
}

/// <summary>
/// The base class of chess
/// </summary>
[DefaultExecutionOrder(-100)]
public class Chess : MonoBehaviour, IEffect_Selected
{
    public CType type = CType.two;
    public Group group = Group.orange;
    public ChessNode myNode;

    public Vector2Int Pos { get { return myNode.pos; } }
    private bool canMove = true;

    private CTag cTag;
    public TagData GetTagData(Group tagger) { return cTag.tags[tagger]; }
    public void SetTagData(TagData td, Group tagger) { cTag.tags[tagger] = td; }

    private SpriteRenderer baseRenderer;
    private Renderer numberRenderer;

    private void Awake()
    {
        if (type == CType.flag || type == CType.infinite)
        {
            canMove = false;
            GameManagement.PUBLIC.chessCannotMove[group]++;
        }
        GameManagement.PUBLIC.gDic[group].Add(this);
        numberRenderer = transform.Find("number").GetComponent<Renderer>();
        baseRenderer = GetComponent<SpriteRenderer>();

        cTag = GetComponentInChildren<CTag>(true);
    }

    /// <summary>
    /// Compare with the another chess,the return value is 1 or 0 or -1
    /// </summary>
    public CLogic Compare(Chess c)
    {
        if (type == CType.boom || c.type == CType.boom) return CLogic.equal;
        if (type == CType.infinite) return CLogic.bigger;
        if (c.type == CType.infinite) return CLogic.less;

        if (type == c.type) return CLogic.equal;
        if ((int)(type) > (int)(c.type)) return CLogic.bigger;
        else return CLogic.less;
    }

    public bool IsOpposite(Chess c)
    {
        return ((int)group * (int)c.group < 0);
    }

    /// <summary>
    /// Get all nodes the chess can move to,return null if the chess can't move
    /// </summary>
    public List<ChessNode> ConnectedNodes()
    {
        if (canMove == false) return null;
        else return myNode.ConnectedNodes();
    }

    /// <summary>
    /// Move to the target,if there is a opposite chess,than compare them automaticly
    /// (Make sure the target is vaild)
    /// </summary>
    /// <param name="target"></param>
    public CLogic MoveTo(ChessNode target)
    {
        CLogic result;
        if (target.chess == null) result = MoveToEmptyNode(target);
        else
        {
            result = Compare(target.chess);
            switch (result)
            {
                case CLogic.bigger:         //win
                    {
                        target.chess.Die();
                        MoveToEmptyNode(target);
                        break;
                    }
                case CLogic.equal:         //draw
                    {
                        target.chess.Die();
                        Die();                     
                        break;
                    }
                case CLogic.less:
                    {
                        Die();
                        break;  //lost
                    }
            }
        }

        return result;
    }

    public CLogic MoveToEmptyNode(ChessNode target)
    {
        myNode.Clear();
        myNode = target;
        target.chess = this;

        Vector3 alignPos = target.transform.position;
        alignPos.y = transform.position.y;
        transform.position = alignPos;

        Effect_UnSelected();
        return CLogic.empty;
    }

    public void Die(bool justClear = false)
    {
        myNode.Clear();
        myNode = null;

        if (justClear == false)
        {
            GameManagement.PUBLIC.gDic[group].Remove(this);
            if (type == CType.nine) GameManagement.PUBLIC.DisPlayTheFlag(group);
            if (type == CType.flag) GameManagement.PUBLIC.SetFailed(group);
            else GameManagement.PUBLIC.CheckActive(group);
        }

        //If the player have no active chess,then setfailed
        gameObject.SetActive(false);
    }

    public void HideNumber()
    {
        numberRenderer.enabled = false;
    }

    public void DisplayNumber()
    {
        numberRenderer.enabled = true;
    }

    public void Effect_Selected()
    {
        baseRenderer.color = new Color(baseRenderer.color.r, baseRenderer.color.g, baseRenderer.color.b, 0.5f);
    }

    public void Effect_UnSelected()
    {
        baseRenderer.color = new Color(baseRenderer.color.r, baseRenderer.color.g, baseRenderer.color.b, 1f);
    }
}
