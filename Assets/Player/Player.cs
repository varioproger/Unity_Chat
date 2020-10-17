using System.Collections;
using System.Collections.Generic;
using TCP;
using UnityEngine;

public class Player : MonoBehaviour
{
    //한 개정의 여러 캐릭터가 있을 경우. 추가 하지 않았음.
    public int Serial_N;
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("Movement");
    }
    IEnumerator Movement()
    {
        while (true)
        {
            SendInputToServer();
            //Play_Anim();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };
        PlayerMovement(_inputs);
    }
    public void PlayerMovement(bool[] _inputs)
    {      
        TCPClient.m_Player.Player_MoveMent(_inputs, Serial_N);
    }
    public void Play_Anim()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        animator.SetFloat("Velocity_X", horz * 10f);
        animator.SetFloat("Velocity_Z", vert * 10f);
    }

}
