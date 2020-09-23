using System.Collections.Generic;
using TCP;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public static Player_Manager instance;
    public static Dictionary<int, Player> players = new Dictionary<int, Player>();

    public GameObject localPlayerPrefab;
    public GameObject otherplayerPrefab;
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


    public void SpawnPlayer(int _id, Vector3 _position, Quaternion _rotation)
    {
        Debug.Log("SpawnPlayer");
        GameObject _player;
        if (TCPClient.m_Player.my_id == _id)
        {
            Debug.Log("SAME");
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            Debug.Log("OTHER");
            _player = Instantiate(otherplayerPrefab, _position, _rotation);
        }
        _player.GetComponent<Player>().Serial_N = _id;
        players.Add(_id, _player.GetComponent<Player>());
    }

}
