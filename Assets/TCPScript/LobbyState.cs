using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using System.Text;
using TCP;
using UnityEngine;

public class LobbyState : State
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UnPackingData(RecvBuffer buffer, out int _result, out string _msg)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        _result = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        int len = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        _msg = Encoding.Default.GetString(data.buffer, data.offset, len);
    }

    public bool Match_Message()
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.LOBBY_STATE | (UInt64)STATE.ENTER;
        Debug.Log(string.Format("temp = {0:x}", Protocol));
        TCPClient.Instance.PackingData(Protocol);
        return true;
    }

    public override void RecvProcess()
    {
        UInt64 Sub = TCPClient.Instance.GetProtocol() & (UInt64)FULL_CODE.SUB;
        UInt64 Protocol = TCPClient.Instance.GetProtocol() & (UInt64)FULL_CODE.PROTOCOL;
        Debug.Log(string.Format("temp = {0:x}", Protocol));

        switch ((STATE)Sub)
        {
            case STATE.ENTER:
                int result;
                string msg;
                UnPackingData(TCPClient.Instance.UnPackingData(), out result, out msg);
                var m_Lobby = GameObject.Find("Lobby_Manager").GetComponent<Lobby>();

                switch ((LOBBY_RESULT)result)
                {
                    case LOBBY_RESULT.NOT_READY:
                        m_Lobby.result = msg;
                        m_Lobby.okay = true;
                        break;

                    case LOBBY_RESULT.READY:
                        m_Lobby.result = msg;
                        m_Lobby.okay = true;
                        m_Lobby.next_Scene = true;
                        break;
                }              
                break;
        }
    }
}
