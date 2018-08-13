using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMove : MonoBehaviour
{
    public float MoveSpeed;
    private void FixedUpdate()
    {
        transform.position += -1 * Time.fixedDeltaTime * new Vector3(MoveSpeed, 0, 0);
    }
}
