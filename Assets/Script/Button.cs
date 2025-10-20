using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Button : MonoBehaviour
{

    [SerializeField] public MonoBehaviour authority;

    public IAuthority Authority
    {
        get => authority as IAuthority;
    }

    public void OnClick()
    {
        Authority.Action();
    }


    private void OnTriggerEnter(Collider other)
    {
        this.OnClick();
        
    }


    private void Start()
    {
        Authority.CompareAll();
    }
}
