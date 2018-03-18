using UnityEngine;

/// <summary>
/// 说明：静态视野对象
/// 
/// @by xiao_D 2017-12-14
/// </summary>

public sealed class FOWStaticRevealer : MonoBehaviour, IFOWRevealer
{
    // 共享数据
    public bool m_isValid = true;
    public float radius = 10f;
    private Vector3 m_pos = Vector3.zero;

    private void Start() {
        FOWSystem.AddRevealer(this);
        m_pos = transform.position;
    }

    public Vector3 GetPosition() {
        return m_pos;
    }

    public float GetRadius() {
        return radius;
    }

    public bool IsValid() {
        return m_isValid;
    }

    void OnDrawGizmosSelected() {
        Matrix4x4 m44 = Matrix4x4.identity;
        m44.SetColumn(3, transform.position);
        
        Gizmos.matrix = m44;
        Gizmos.color = new Color(0, 0.6f, 1, 0.7f);
        // 绘制圆环
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f) {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, 0, z);
            if (theta == 0) {
                firstPoint = endPoint;
            }
            else {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint, beginPoint);
    }


    private void OnDestroy() {
        m_isValid = false;
        FOWSystem.RemoveRevealer(this);
    }
}
