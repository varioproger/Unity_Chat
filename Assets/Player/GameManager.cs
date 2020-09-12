using System;
using System.Collections;
using System.Collections.Generic;
using TCP;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int , Player_Manager> players = new Dictionary<int, Player_Manager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        TCPClient.m_Player.Player_Init_SendMessage();
    }


    public void SpawnPlayer(int _id,int serial_n,Vector3 _position, Quaternion _rotation)
    {
        Debug.Log("SpawnPlayer");
        GameObject _player;
        if (TCPClient.m_Login.id == _id)
        {
            Debug.Log("SAME");
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            Debug.Log("OTHER");
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        _player.GetComponent<Player_Manager>().Serial_N = _id;
        players.Add(_id, _player.GetComponent<Player_Manager>());
    }

}
