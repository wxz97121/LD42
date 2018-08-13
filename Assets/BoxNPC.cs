using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxNPC : NPC {

    public float minWaitTime = 1, maxWaitTime = 2.5f;
    public Vector2 minVec, maxVec;
    public Vector2 DowningminVec, DowningmaxVec;
    public float minForce, maxForce;
    private Rigidbody2D m_Rigid;
    private new void Awake()
    {
        base.Awake();
        m_Rigid = gameObject.GetComponent<Rigidbody2D>();
    }
    protected override IEnumerator Move()
    {
        while (true)
        {
            //print(isDowning);
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            if (!isDowning) m_Rigid.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(minVec, maxVec, Random.value));
            else m_Rigid.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(DowningminVec, DowningmaxVec, Random.value));
        }
    }
    public override void BeatBehavior()
    {
        gameObject.transform.eulerAngles += new Vector3(0, 180f, 0);
    }
}
