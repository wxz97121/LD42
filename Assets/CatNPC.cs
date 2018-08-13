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
    public int Dir;
    public AudioSource punch;
    [HideInInspector]
    public TransformForce m_Character;
    private new void Awake()
    {
        base.Awake();
        m_JS = gameObject.GetComponent<JellySprite>();
    }
    private void Start()
    {
        GetComponent<JellySprite>().AddForce(new Vector2(Dir, 1) * 1000 * Random.Range(2, 5));
        DowningmaxVec *= new Vector2(Dir, 1);
        DowningminVec *= new Vector2(Dir, 1);
    }
    protected override IEnumerator Move()
    {
        yield break;
        while (true)
        {
            //print(isDowning);
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            if (!isDowning) m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(minVec, maxVec, Random.value));
            else m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(DowningminVec, DowningmaxVec, Random.value));
        }
    }
    public override void BeatBehavior()
    {
        //base.BeatBehavior();
        if (!m_JS) return;
        //gameObject.transform.eulerAngles += new Vector3(0, 180f, 0);
        if (!isDowning) m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(minVec, maxVec, Random.value));
        else m_JS.AddForce(Random.Range(minForce, maxForce) * Vector2.Lerp(DowningminVec, DowningmaxVec, Random.value));
    }
    void OnJellyCollisionStay2D(JellySprite.JellyCollision2D collision)
    {
        if (collision.Collision2D.gameObject.CompareTag("Player")){
            if (m_Character)
            {
                m_Character.isHurt++;

            }
        }

    }
}
