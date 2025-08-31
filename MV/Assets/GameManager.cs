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
////������
//public class Authority
//{
//    public string authorityName;
//    [Space]
//    public List<Link> Links = new List<Link>();//���ǵĻ�������ʵ�ֵ���·�����ӣ������ж����Ŀ��
//    [Space]
//    public List<Vector3> condations = new List<Vector3>();//��������
//    public Transform pivot;
//}
////�Ҿ����ˣ����ز�����д������

//[System.Serializable]
//public class Link
//{
//    public List<Transform> aims = new List<Transform>(2);
//}


[System.Serializable]
//������
public class Authority
{
    public string authorityName;
    [Space]
    public Walkable[] linkAims = new Walkable[2];//ÿһ�����ض�Ӧһ�Կ������
    [Space]
    public List<Transform> condations = new List<Transform>();//��������
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



