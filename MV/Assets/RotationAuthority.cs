using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAuthority : MonoBehaviour, IAuthority
{
    public bool isManual = true;//����꽻���Ļ��أ����ǽ�ɫ�����Ļ���

    //Ԥ���������Ŀ�굱ǰ���
    public List<Transform> expectCondstions;
    public List<Transform> observationTarget;

    //���ݵ�Ŀ��
    public Transform aim;
    public void Action()
    {
        throw new System.NotImplementedException();
    }

    public bool Compare()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
