using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformForce : MonoBehaviour
{
    [SerializeField] private GameObject mLeftArm;
    [SerializeField] private GameObject mRightArm;
    private UnityJellySprite m_JellySp;
    public float Strenth = 1;
    private Controller m_Controller;
    public float JumpForce = 1000;
    public float HP = 10;
    public Scrollbar HPBar;
    float InitalHP;
    private void Awake()
    {
        m_Controller = FindObjectOfType<Controller>();
        m_JellySp = GetComponent<UnityJellySprite>();
        InitalHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:在地面上才能跳
        if (m_JellySp!=null){
            m_JellySp.AddForce(Strenth * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.JoystickButton0)) m_JellySp.AddForce(new Vector2(0, JumpForce));
        }
        else {
            if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.JoystickButton0)) GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce));
        }
        if (transform.position.x < Controller.minX || transform.position.y > Controller.maxX)
            StartCoroutine(m_Controller.Lose());
        
        if (Input.GetKeyDown(KeyCode.Q)){
            mLeftArm.transform.eulerAngles = new Vector3(0, 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mRightArm.transform.eulerAngles = new Vector3(0, 0, 0f);
        }

        if (HP < 0) StartCoroutine(m_Controller.Lose());
        HPBar.size = Mathf.Clamp01(HP / InitalHP);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NPC"){
            isHurt = true;
        }
    }
    bool isHurt = false;
    private void FixedUpdate()
    {
        if (isHurt)
        {
            HP -= Time.fixedDeltaTime;
        }
        isHurt = false;
    }
}
