using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    protected bool isDowning = false;
    // isDowning 表示一个NPC处于要下车的状态，还是不想下车的状态
    public bool alive = false;
    protected abstract IEnumerator Move();
    protected void Awake()
    {
        StartCoroutine(Move());
    }
    public void ExitFromTrain()
    {
        isDowning = true;
    }
    public virtual void BeatBehavior (){
        
    }
}
