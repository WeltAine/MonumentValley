using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using UnityEditor;


[System.Serializable]
public class AuthoritiesRotate : MonoBehaviour
{
//    //public List<Authority> Authoritys = new List<Authority>();
//    public Authority authority;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        int index = 0;

//        foreach (Vector3 _condation in authority.condations)
//        {

//                foreach(WalkPath aimPath in authority.Links[index].aims[0].GetComponent<Walkable>().possiblePath)//֮�����ܲ���������Ȳ��������Լ�Walkable��Walkpath���캯��ת��
//                {
//                    if (aimPath.target == authority.Links[index].aims[1])
//                    {
//                        aimPath.active = (this.transform.eulerAngles == _condation || this.transform.eulerAngles += _condation == new Vector3());


//                    }
//                }

//                foreach (WalkPath aimPath in authority.Links[index].aims[1].GetComponent<Walkable>().possiblePath)//֮�����ܲ���������Ȳ��������Լ�Walkable��Walkpath���캯��ת��
//                {
//                    if (aimPath.target == authority.Links[index].aims[0])
//                    {
//                        aimPath.active = (this.transform.eulerAngles == _condation);

//                    }
//                }


//            index++;
//        }
//    }

//    public void Rotate(Vector3 _eulerAngles)
//    {
//        //this.transform.DORotate(_eulerAngles, 0.5);
//        //this.transform.DORotate(_eulerAngles + authority.pivot.eulerAngles, 0.5f, RotateMode.WorldAxisAdd);
//        this.transform.DORotate(_eulerAngles, 0.5f, RotateMode.WorldAxisAdd);//������ת����ָ����ת
//    }

//    public void Move()
//    {
        
//    }
}


public interface IAuthority
{
    void Action();

    bool Compare();
}
