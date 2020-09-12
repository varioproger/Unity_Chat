using TCP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register_Manager : MonoBehaviour
{
    public InputField IDField;
    public InputField PWField;
    public InputField NickField;
    public Text result;

    public string result_str;
    public bool okay = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (result_str.Length != 0)
        {
            result.text = result_str;
        }
    }
 
    public void Back()
    {
        SceneManager.LoadScene("Login");
    }

    public void Register_Info()
    {
        if (IDField.text.Length != 0 && PWField.text.Length != 0&& NickField.text.Length!=0)
        {
            Debug.Log($"회원 정보 ={IDField.text},{PWField.text}");
            TCPClient.m_Login.Register_Info(IDField.text, PWField.text, NickField.text);
        }
    }
}
