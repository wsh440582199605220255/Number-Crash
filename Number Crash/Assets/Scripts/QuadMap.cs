using UnityEngine;

public class QuadMap : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    protected const int MAX_RC = 32;
    public static ChessNode[,] m = new ChessNode[MAX_RC, MAX_RC];
    public float sideLength = 1;

    public static QuadMap PUBLIC;
    public static LayerMask nodeLayer;
    public static LayerMask chessLayer;

    protected void Awake()
    {
        PUBLIC = this;

        nodeLayer = LayerMask.GetMask("CNode");
        chessLayer = LayerMask.GetMask("Chess");
    }

    private void Start()
    {
        ChessNode[] nodes = GetComponentsInChildren<ChessNode>();
        for (int i = 0; i < nodes.Length; i++) m[nodes[i].pos.x, nodes[i].pos.y] = nodes[i];
    }


    public ChessNode Find(Vector2Int pos, bool ignoreObstacle = true)
    {
        if (pos.x >= 0 && pos.x < size.x && pos.y >= 0 && pos.y < size.y)
        {
            if (ignoreObstacle)
            {
                if (m[pos.x, pos.y].type == NType.obstacle) return null;
            }
            return m[pos.x, pos.y];
        }
        return null;
    }

    public Vector3 GridsPosToWorld(Vector2Int gPos)
    {
        Vector3 result = transform.position;
        result.z -= gPos.x * sideLength;
        result.x += gPos.y * sideLength;
        return result;
    }

}
