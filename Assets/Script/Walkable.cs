using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 每一个可行走的块都将有一个该组件，它记录了地面类型，寻路时所要用的路径前驱
/// </summary>
public class Walkable : MonoBehaviour
{
    public List<WalkPath> possiblePath = new List<WalkPath>();

    public bool isStairs = false;
    public bool isRote = false;

    public Transform proPath;


    //绘制辅助图标
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(GetGizmosPositon(), 0.1f);
        foreach(WalkPath path in possiblePath)
        {
            if (path.active)
            {
                Gizmos.DrawLine(GetGizmosPositon(), path.target.GetComponent<Walkable>().GetGizmosPositon());
            }
        }
    }

    //获取绘制gizmos的位置，因为我们要在两个相邻方块的Gizmos之间画一条线表示潜在路径，为了方便获取设置此函数
    public Vector3 GetGizmosPositon()
    {
        return this.transform.position + ((isStairs)? 0.0f : 0.5f) * this.transform.up;//不要用Vector3.up
    }


    //两个地块的相互连接
    public static void LinkAim(Walkable aim_1, Walkable aim_2)
    {
        WalkPath aim_1_path = new WalkPath { target = aim_1.transform, active = true };
        WalkPath aim_2_path = new WalkPath { target = aim_2.transform, active = true };

        aim_1.possiblePath.Add(aim_2_path);
        aim_2.possiblePath.Add(aim_1_path);

    }

    public static void Disrupt(Walkable aim_1, Walkable aim_2)
    {
        var aimPath = aim_1.possiblePath.Where((x) => x.target == aim_2.transform)
            .ToList();

        foreach (WalkPath tem in aimPath)
        {
            aim_1.possiblePath.Remove(tem);
        }


        aimPath.Clear();
        aimPath = aim_2.possiblePath.Where((x) => x.target == aim_1.transform)
            .ToList();
        foreach (WalkPath tem in aimPath)
        {
            aim_2.possiblePath.Remove(tem);
        }

    }

}


/// <summary>
/// 用来简易记录相邻链接块，也就是临近节点的信息
/// </summary>
[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active;//该路径的激活状态

}
