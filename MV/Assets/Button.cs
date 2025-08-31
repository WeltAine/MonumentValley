using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Transform aim;
    public Vector3 aix;//Ðý×ªÖá
    public float angle;//Ðý×ª½Ç¶È

    public bool isFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.name == "player")
        aim.Rotate(aix, angle);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    aim.Rotate(aix,angle);
    //}
}
