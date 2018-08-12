using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformForce : MonoBehaviour
{
    private UnityJellySprite m_JellySp;
    public float Strenth = 1;
    private Controller m_Controller;
    public float JumpForce = 1000;
    private void Awake()
    {
        m_Controller = FindObjectOfType<Controller>();
        m_JellySp = GetComponent<UnityJellySprite>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:在地面上才能跳
        m_JellySp.AddForce(Strenth * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.JoystickButton0)) m_JellySp.AddForce(new Vector2(0, JumpForce));
        if (transform.position.x < Controller.minX || transform.position.y > Controller.maxX)
            StartCoroutine(m_Controller.Lose());
    }
}
