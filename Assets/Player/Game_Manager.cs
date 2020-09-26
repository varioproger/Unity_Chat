using UnityEngine;
using TCP;


public class Game_Manager : MonoBehaviour
{

    //방이 생성됨에 따라 이것도 static이 아닌 동적생성 가능하도록 수정 필요
    public static Game_Manager instance;
    public static int room_number;
    GameObject lobby_manager;
    // Start is called before the first frame update
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
        Debug.Log("AWAKE_Game_Manager");
        lobby_manager = GameObject.Find("Lobby_Manager");
        lobby_manager.GetComponent<Lobby_Manager>().Make_Room(room_number);
    }
}
