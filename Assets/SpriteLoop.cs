using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoop : MonoBehaviour
{
    public float HeightWhenTrans;
    public float SelfHeight;
    public Transform target;
    void Update()
    {
        if (transform.position.x <= HeightWhenTrans)
            transform.position = target.position + new Vector3(SelfHeight, 0, 0);

    }
}
