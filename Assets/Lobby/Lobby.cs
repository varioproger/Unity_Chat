using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TCP;

public class Lobby : MonoBehaviour
{
    public bool okay = false;
    public string result;
    public Text text;
    public bool next_Scene = false;

    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
    public void UpdateString(string str)
    {
        text.text = str;
    }
}
