using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayGizmos : MonoBehaviour
{
    public Color _color = Color.magenta;
    public float _radius = 0.5f;


    const string wayPointFile = "Enemy";
    void Start()
    {

    }


    private void OnDrawGizmos()
    {

        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);



    }

    void Update()
    {

    }
}
