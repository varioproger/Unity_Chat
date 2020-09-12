using System;
using TCP;
using UnityEngine;
using UnityEngine.UI;

public class Chat_Manager : MonoBehaviour
{
    public InputField MessageInputField;
    public Text TextWindow;

    private object cacheLock = new object();
    private string cache;

    private void Awake()
    {

    }

    private void Update()
    {
        lock (cacheLock)
        {
            if (!string.IsNullOrEmpty(cache))
            {
                TextWindow.text += string.Format("{0}", cache);
                cache = null;
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SendMessageToServer()
    {
        string message = MessageInputField.text;
        if (message.StartsWith("!ping"))
        {
            message += " " + (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        if (!string.IsNullOrEmpty(message))
        {
            if (TCPClient.m_Chat.Chat_SendMessage(message))
            {
                MessageInputField.text = string.Empty;
            }
        }
    }

    public void OnClientReceivedMessage(string msg)
    {
        string finalMessage = msg;
        lock (cacheLock)
        {
            if (string.IsNullOrEmpty(cache))
            {
                cache = string.Format("<color=green>{0}</color>\n", finalMessage);
            }
            else
            {
                cache += string.Format("<color=green>{0}</color>\n", finalMessage);
            }
        }
    }

    private void OnClientLog(string message)
    {
        lock (cacheLock)
        {
            if (string.IsNullOrEmpty(cache))
            {
                cache = string.Format("<color=grey>{0}</color>\n", message);
            }
            else
            {
                cache += string.Format("<color=grey>{0}</color>\n", message);
            }
        }
    }
}
