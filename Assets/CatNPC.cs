using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JellySprite))]
public class CatNPC : NPC
{
    public float minWaitTime = 1, maxWaitTime = 2.5f;
    public Vector2 minVec, maxVec;
    public Vector2 DowningminVec, DowningmaxVec;
    public float minForce, maxForce;
    private JellySprite m_JS;
    private new void Awake()
    {
        base.Awake();
        m_JS = gameObject.GetComponent<JellySprite>();
    }
    protected override IEnumerator Move()
    {
        while (true)
        {
            //print(isDowning);
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            if (!isDowning) m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(minVec, maxVec, Random.value));
            else m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(DowningminVec, DowningmaxVec, Random.value));
        }
    }
}
