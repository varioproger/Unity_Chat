using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using System.Text;
using TCP;
using UnityEngine;

public class LoginState : State
{
    public LoginState()
    {
        m_SubState = SUBSTATE.LOGININFO;
    }

    enum SUBSTATE : int
    {
        LOGININFO,

    }

    Login m_login;
    Register_Manager m_register;
    public int id;
    SUBSTATE m_SubState;

    byte[] PackingData(string _id, string _pw)
    {
        int size = _id.Length + _pw.Length + (sizeof(int) * 2);
        SendBuffer data = new SendBuffer();
        data.buffer = new byte[size];
        data.size = 0;
        data.offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_id.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(_id.ToCharArray()), 0, data.buffer, data.offset, _id.Length);
        data.size += _id.Length;
        data.offset += _id.Length;

        Buffer.BlockCopy(BitConverter.GetBytes(_pw.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(_pw.ToCharArray()), 0, data.buffer, data.offset, _pw.Length);
        data.size += _pw.Length;

        return data.buffer;
    }

    byte[] PackingData(string _id, string _pw, string name)
    {
        int size = _id.Length + _pw.Length  + name.Length + (sizeof(int) * 3);
        SendBuffer data = new SendBuffer();
        data.buffer = new byte[size];
        data.size = 0;
        data.offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_id.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(_id.ToCharArray()), 0, data.buffer, data.offset, _id.Length);
        data.size += _id.Length;
        data.offset += _id.Length;

        Buffer.BlockCopy(BitConverter.GetBytes(_pw.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(_pw.ToCharArray()), 0, data.buffer, data.offset, _pw.Length);
        data.size += _pw.Length;
        data.offset += _pw.Length;

        Buffer.BlockCopy(BitConverter.GetBytes(name.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        Buffer.BlockCopy(Encoding.Default.GetBytes(name.ToCharArray()), 0, data.buffer, data.offset, name.Length);
        data.size += name.Length;

        return data.buffer;
    }

    void UnPackingData(RecvBuffer buffer, out int _result)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        _result = BitConverter.ToInt32(data.buffer, data.offset);
    }
    void UnPackingData(RecvBuffer buffer, out int _num, out string _msg)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);
        data.offset += sizeof(int);

        _num = BitConverter.ToInt32(data.buffer, data.offset);
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

    public void Register_Info(string id, string pw, string name)
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.LOGIN_STATE | (UInt64)STATE.MENU_SELECT_STATE | (UInt64)PROTOCOL.JOIN_INFO;
        Debug.Log(string.Format("temp = {0:x}", Protocol));

        TCPClient.Instance.PackingData(Protocol, PackingData(id, pw, name));
    }

    public void Login_Info(string id, string pw)
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.LOGIN_STATE | (UInt64)STATE.MENU_SELECT_STATE | (UInt64)PROTOCOL.LOGIN_INFO;
        Debug.Log(string.Format("temp = {0:x}", Protocol));

        TCPClient.Instance.PackingData(Protocol, PackingData(id, pw));
    }

    public override void RecvProcess()
    {
        UInt64 Protocol = TCPClient.Instance.GetProtocol() & (UInt64)FULL_CODE.PROTOCOL;
        Debug.Log(string.Format("temp = {0:x}", Protocol));

        int result;
        string msg = null;
        int num =0;
        switch((PROTOCOL)Protocol)
        {
            case PROTOCOL.JOIN_RESULT:
                UnPackingData(TCPClient.Instance.UnPackingData(), out result, out msg);
                if (result == (int)RESULT.JOIN_SUCCESS)
                {
                    print("ok");
                    SceneManager.LoadScene("Login");
                }
                else
                {
                    print("No");
                }

                m_register = GameObject.Find("Register_Manager").GetComponent<Register_Manager>();
                m_register.result_str = msg;
                print(msg);
                break;

            case PROTOCOL.LOGIN_RESULT:
                RecvBuffer buffer = TCPClient.Instance.UnPackingData();
                UnPackingData(buffer, out result);
            
                if (result == (int)RESULT.LOGIN_SUCCESS)
                {
                    print("ok");
                    //이 부분 수정
                    UnPackingData(buffer, out num, out msg);
                    id = num;
                    SceneManager.LoadScene("Lobby_Scene");
                    //TCPClient.Instance.SetState(TCPClient);
                }
                else
                {
                    print("No");
                    UnPackingData(buffer, out msg);
                }
                m_login = GameObject.Find("Login_Manager").GetComponent<Login>();
                m_login.result_str = msg;
                print(msg);
                break;
        }
    }
}
