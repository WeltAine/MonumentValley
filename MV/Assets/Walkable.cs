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


    //���Ƹ���ͼ��
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

    //��ȡ����gizmos��λ�ã���Ϊ����Ҫ���������ڷ����Gizmos֮�仭һ���߱�ʾǱ��·����Ϊ�˷����ȡ���ô˺���
    public Vector3 GetGizmosPositon()
    {
        return this.transform.position + ((isStairs)? 0.0f : 0.5f) * this.transform.up;//��Ҫ��Vector3.up
    }

}



//������¼���ڿ�
[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active;//��·���ļ���״̬������ʱ��·���ܷ�ʹ�ã����ڹ����Ǵ��·����˵�����Ҫ

}
