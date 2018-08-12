using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    bool isDowning = false;
    // isDowning 表示一个NPC处于要下车的状态，还是不想下车的状态
    public bool alive = false;
    protected abstract void Move();
    void Update()
    {
        Move();
    }
    public void ExitFromTrain()
    {
        //TODO;
    }
}
