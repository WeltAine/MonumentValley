using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<Authority> authorities = new List<Authority>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Authority _authority in authorities)
        {
            _authority.CheckCondation();
        }
    }
}



//[System.Serializable]
////机关类
//public class Authority
//{
//    public string authorityName;
//    [Space]
//    public List<Link> Links = new List<Link>();//我们的机关最终实现的是路径链接，所以有多个块目标
//    [Space]
//    public List<Vector3> condations = new List<Vector3>();//触发条件
//    public Transform pivot;
//}
////我决定了，机关操作不写在这里

//[System.Serializable]
//public class Link
//{
//    public List<Transform> aims = new List<Transform>(2);
//}


[System.Serializable]
//机关类
public class Authority
{
    public string authorityName;
    [Space]
    public Walkable[] linkAims = new Walkable[2];//每一个机关对应一对块的链接
    [Space]
    public List<Transform> condations = new List<Transform>();//触发条件
    [Space]
    public List<Transform> condationAims = new List<Transform>();

    public void CheckCondation()
    {
        int _count = 0;

        for(int i = 0; i < condationAims.Count; i++)
        {
            //if (condationAims[i].position == condations[i].position && condationAims[i].rotation == condations[i].rotation)
            if (condationAims[i].position == condations[i].position)
            {
                _count++;
            }
        }

        if(_count == condations.Count)
        {
            LinkAim();
        }
        else
        {
            Disrupt();
        }
    }

    public void LinkAim()
    {
        foreach(WalkPath otherAim in linkAims[0].possiblePath)
        {
            if(otherAim.target == linkAims[1].transform)
            {
                otherAim.active = true;
            }
        }

        foreach (WalkPath otherAim in linkAims[1].possiblePath)
        {
            if (otherAim.target == linkAims[0].transform)
            {
                otherAim.active = true;
            }
        }


    }

    public void Disrupt()
    {
        foreach (WalkPath otherAim in linkAims[0].possiblePath)
        {
            if (otherAim.target == linkAims[1].transform)
            {
                otherAim.active = false;
            }
        }

        foreach (WalkPath otherAim in linkAims[1].possiblePath)
        {
            if (otherAim.target == linkAims[0].transform)
            {
                otherAim.active = false;
            }
        }

    }
}



