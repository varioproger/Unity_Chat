using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TCP;

public class Login : MonoBehaviour
{
    
    public InputField IDField;
    public InputField PWField;
    public Text result;

    public string result_str;
    public bool okay = false;

    private void Update()
    {
        if(result_str.Length!=0)
        {
            if(okay)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("This is a debug build!");
                }
                SceneManager.LoadScene("Lobby_Scene");
               
            }
            else
            {
                result.text = result_str;
            }
        }
    }

    public void Login_Info()
    {      
        if(IDField.text.Length !=0 && PWField.text.Length!=0)
        {
            Debug.Log($"회원 정보 ={IDField.text},{PWField.text}");
            TCPClient.m_Login.Login_Info(IDField.text, PWField.text);
        }  
    }
    public void To_Register()
    {
        SceneManager.LoadScene("Register");
    }
}
