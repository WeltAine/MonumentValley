using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondationPivot : MonoBehaviour
{
    public float r = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, r);
    }
}
