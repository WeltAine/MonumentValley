using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public List<WalkPath> possiblePath = new List<WalkPath>();

    public bool isStairs = false;
    public bool isRote = false;

    public Transform proPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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

}



//用来记录相邻块
[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active;//该路径的激活状态，即此时该路径能否使用，对于构建是错觉路线来说这很重要

}
