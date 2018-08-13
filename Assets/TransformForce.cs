using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TransformForce : MonoBehaviour
{
    private Rigidbody2D mLeftArm;
    private Rigidbody2D mRightArm;
    private Rigidbody2D mLeftLeg;
    private Rigidbody2D mRightLeg;

    private UnityJellySprite m_JellySp;
    public float Strenth = 1;
    private Controller m_Controller;
    public float JumpForce = 1000;
    public float HP = 10;
    public Scrollbar HPBar;
    [HideInInspector]
    public float InitalHP;
    private void Awake()
    {
        mLeftArm = transform.parent.Find("LArm").GetComponent<Rigidbody2D>();
        mRightArm = transform.parent.Find("RArm").GetComponent<Rigidbody2D>();
        mLeftLeg = transform.parent.Find("LLeg").GetComponent<Rigidbody2D>();
        mRightLeg = transform.parent.Find("RLeg").GetComponent<Rigidbody2D>();

        m_Controller = FindObjectOfType<Controller>();
        m_JellySp = GetComponent<UnityJellySprite>();
        InitalHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:在地面上才能跳
        //if (m_JellySp != null)
        //{
        //    m_JellySp.AddForce(Strenth * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        //    if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.JoystickButton0)) m_JellySp.AddForce(new Vector2(0, JumpForce));
        //}
        //else
        //{
        //    if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.JoystickButton0)) GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce));
        //}

        //if (transform.position.x < Controller.minX || transform.position.y > Controller.maxX)
        //    StartCoroutine(m_Controller.Lose());

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //mLeftArm.transform.eulerAngles = new Vector3(0, 0, 0f);
            mLeftArm.AddRelativeForce(Strenth * new Vector2(0, 1), ForceMode2D.Impulse);
            mLeftArm.GetComponentInChildren<SpriteRenderer>().transform.DOKill();
            mLeftArm.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, 2.5f, 1);
            mLeftArm.GetComponentInChildren<SpriteRenderer>().transform.DOScaleY(1, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mRightArm.AddRelativeForce(Strenth * new Vector2(0, 1), ForceMode2D.Impulse);
            mRightArm.GetComponentInChildren<SpriteRenderer>().transform.DOKill();
            mRightArm.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, 2.5f, 1);
            mRightArm.GetComponentInChildren<SpriteRenderer>().transform.DOScaleY(1, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            mLeftLeg.AddRelativeForce(Strenth * new Vector2(0, -1), ForceMode2D.Impulse);
            mLeftLeg.GetComponentInChildren<SpriteRenderer>().transform.DOKill();
            mLeftLeg.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, 2.5f, 1);
            mLeftLeg.GetComponentInChildren<SpriteRenderer>().transform.DOScaleY(1, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            mRightLeg.AddRelativeForce(Strenth * new Vector2(0, -1), ForceMode2D.Impulse);
            mRightLeg.GetComponentInChildren<SpriteRenderer>().transform.DOKill();
            mRightLeg.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, 2.5f, 1);
            mRightLeg.GetComponentInChildren<SpriteRenderer>().transform.DOScaleY(1, 0.5f);
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Kinematic)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                HP = 5000;
            }
            else
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                HP = InitalHP;
            }
        }
        if (HP < 0) StartCoroutine(m_Controller.Lose("呜呜呜，游戏失败，你被挤死了"));
        HPBar.size = Mathf.Clamp01(HP / InitalHP);
    }
    public int isHurt = 0;
    private void FixedUpdate()
    {
        if (isHurt >= 2)
        {
            HP -= Time.fixedDeltaTime * 0.5f * (isHurt - 1);
        }
        isHurt = 0;
    }
}
