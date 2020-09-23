using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TCP;
using System.Collections.Generic;

public class Lobby_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Lobby_Manager Instance;
    public GameObject Game_Manager_PF;
    //public int Room_Num;
    public static Dictionary<int, Player_Manager> Game_Room = new Dictionary<int, Player_Manager>();
    public bool okay = false;
    public string result;
    public Text text;
    public bool next_Scene = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        TCPClient.m_Lobby.Match_Message();
    }

    private void Update()
    {
        if(okay)
        {
            UpdateString(result);
            okay = false;
        }
        if(next_Scene)
        {
            SceneManager.LoadScene("Player");
            next_Scene = false;
        }
    }
    public void UpdateString(string str)
    {
        text.text = str;
    }
    public void Make_Room(int room_num)
    {
        GameObject G_M;
        G_M = Instantiate(Game_Manager_PF);
        Game_Room.Add(room_num, G_M.GetComponent<Player_Manager>());
        Debug.Log($"Game_Room.Count = {Game_Room.Count}");
    }
}
