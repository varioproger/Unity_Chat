using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using System.Text;
using TCP;
using UnityEngine;
using System.Net.Sockets;

public class ChatState : State
{
    public ChatState()
    {

    }

    Chat_Manager m_Chat;

    byte[] PackingData(string msg)
    {
        int size = msg.Length + (sizeof(int));
        SendBuffer data = new SendBuffer();
        data.buffer = new byte[size];
        data.size = 0;
        data.offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(msg.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(msg.ToCharArray()), 0, data.buffer, data.offset, msg.Length);
        data.size += msg.Length;

        return data.buffer;
    }

    void UnPackingData(RecvBuffer buffer, out string _msg)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        int len = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        _msg = Encoding.Default.GetString(data.buffer, data.offset, len);
    }

    public bool Chat_SendMessage(string msg)
    {
        try
        {
            UInt64 Protocol = (UInt64)CLASS_STATE.CHAT_STATE | (UInt64)STATE.RECV_STATE | (UInt64)PROTOCOL.RECV;
            Debug.Log(string.Format("temp = {0:x}", Protocol));

            TCPClient.Instance.PackingData(Protocol, PackingData(msg));
        }
        catch (SocketException e)
        {
            print("Socket Error : " + e.Message.ToString());
            return false;
        }

        return true;
    }

    public override void RecvProcess()
    {
        UInt64 Protocol = TCPClient.Instance.GetProtocol() & (UInt64)FULL_CODE.PROTOCOL;
        Debug.Log(string.Format("temp = {0:x}", Protocol));

        string msg;

        switch ((PROTOCOL)Protocol)
        {
            case PROTOCOL.RECV:
                UnPackingData(TCPClient.Instance.UnPackingData(), out msg);
                m_Chat = GameObject.Find("TCPClientServer").GetComponent<Chat_Manager>();
                m_Chat.OnClientReceivedMessage(msg);
                break;
        }
    }
}
