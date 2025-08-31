using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAuthority : MonoBehaviour, IAuthority
{
    public bool isManual = true;//是鼠标交互的机关，还是角色触发的机关

    //预期情况，与目标当前情况
    public List<Transform> expectCondstions;
    public List<Transform> observationTarget;

    //操纵的目标
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
