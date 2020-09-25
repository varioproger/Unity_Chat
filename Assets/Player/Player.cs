﻿using System.Collections;
using TCP;
using UnityEngine;

public class Player : MonoBehaviour
{
    //한 개정의 여러 캐릭터가 있을 경우. 추가 하지 않았음.
    public int Serial_N;

    private void Start()
    {
        StartCoroutine("Movement");
    }
    IEnumerator Movement()
    {
        while (true)
        {
            SendInputToServer();
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

}
