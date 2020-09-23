using System;
using System.Text;
using TCP;
using UnityEngine;

public class LobbyState : State
{
    void UnPackingData(RecvBuffer buffer, out int _result)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        _result = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);
    }
    void UnPackingData(RecvBuffer buffer, out int room_num, out string _msg)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);
        data.offset += sizeof(int);

        room_num = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        int len = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        _msg = Encoding.Default.GetString(data.buffer, data.offset, len);
    }
    void UnPackingData(RecvBuffer buffer, out string _msg)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);
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


        switch ((STATE)Sub)
        {
            case STATE.ENTER:
                int result;
                int room_num;
                string msg;
                RecvBuffer buffer = TCPClient.Instance.UnPackingData();
                UnPackingData(buffer, out result);
                var m_Lobby = GameObject.Find("Lobby_Manager").GetComponent<Lobby_Manager>();

                switch ((LOBBY_RESULT)result)
                {
                    case LOBBY_RESULT.NOT_READY:
                        UnPackingData(buffer, out room_num, out msg);
                        Game_Manager.instance.room_number = room_num;
                        m_Lobby.result = msg;
                        m_Lobby.okay = true;
                        break;

                    case LOBBY_RESULT.READY:
                        UnPackingData(buffer, out msg);
                        m_Lobby.result = msg;
                        m_Lobby.okay = true;
                        m_Lobby.next_Scene = true;
                        break;
                }              
                break;
        }
    }
}
