using System;
using System.Text;
using TCP;
using UnityEngine;

public class PlayerState : State
{
    public int my_id;
    void UnPackingData(RecvBuffer buffer, out int id, out Vector3 position, out Quaternion rotation)
    {

        Debug.Log("UNPACKINHGG");
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        id = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        position.x = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(position.x);

        position.y = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
       // Debug.Log(position.y);

        position.z = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(position.z);

        rotation.w = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(rotation.w);

        rotation.x = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(rotation.x);

        rotation.y = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
       // Debug.Log(rotation.y);

        rotation.z = BitConverter.ToSingle(data.buffer, data.offset);
        //Debug.Log(rotation.z);
    }
    void UnPackingData(RecvBuffer buffer, out int serial, out Vector3 position)
    {
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        ////serial
        serial = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);
        //Debug.Log(string.Format("temp = {0:x}", serial));

        position.x = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(position.x);

        position.y = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(position.y);

        position.z = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        //Debug.Log(position.z);

       
    }
    void UnPackingData(RecvBuffer buffer, out int serial, out Quaternion rotation)
    {

        Debug.Log("UNPACKINHGG");
        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        ////serial
        serial = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);
        Debug.Log(string.Format("temp = {0:x}", serial));

        rotation.w = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        Debug.Log(rotation.w);

        rotation.x = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        Debug.Log(rotation.x);

        rotation.y = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);
        Debug.Log(rotation.y);

        rotation.z = BitConverter.ToSingle(data.buffer, data.offset);
        Debug.Log(rotation.z);
    }

    void UnPackingData(RecvBuffer buffer, out int serial, out float y, out float x)
    {

        RecvBuffer data = buffer;
        data.offset = sizeof(UInt64);

        ////serial
        serial = BitConverter.ToInt32(data.buffer, data.offset);
        data.offset += sizeof(int);

        y = BitConverter.ToSingle(data.buffer, data.offset);
        data.offset += sizeof(float);

        x = BitConverter.ToSingle(data.buffer, data.offset);
    }
    byte[] PackingData(bool[] _inputs, Quaternion rotation)
    {

        int size = _inputs.Length + (sizeof(bool) * 4)+(sizeof(float)*4) ;
        SendBuffer data = new SendBuffer();
        data.buffer = new byte[size];
        data.size = 0;
        data.offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_inputs.Length), 0, data.buffer, data.offset, sizeof(int));
        data.size += sizeof(int);
        data.offset += sizeof(int);

        foreach(bool input in _inputs)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(input), 0, data.buffer, data.offset, sizeof(bool));
            data.size += sizeof(bool);
            data.offset += sizeof(bool);
        }
        Buffer.BlockCopy(BitConverter.GetBytes(rotation.w), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);
        data.offset += sizeof(float);

        Buffer.BlockCopy(BitConverter.GetBytes(rotation.x), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);
        data.offset += sizeof(float);

        Buffer.BlockCopy(BitConverter.GetBytes(rotation.y), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);
        data.offset += sizeof(float);

        Buffer.BlockCopy(BitConverter.GetBytes(rotation.z), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);

        return data.buffer;
    }


    byte[] PackingData(float verticalRotation , float horizontalRotation)
    {

        int size = sizeof(float) * 2;
        SendBuffer data = new SendBuffer();
        data.buffer = new byte[size];
        data.size = 0;
        data.offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(verticalRotation), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);
        data.offset += sizeof(float);

        Buffer.BlockCopy(BitConverter.GetBytes(horizontalRotation), 0, data.buffer, data.offset, sizeof(float));
        data.size += sizeof(float);

        return data.buffer;
    }

    public void PlayerInitRotation(float verticalRotation, float horizontalRotation)
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.PLAYER_STATE | (UInt64)STATE.MOVEMENT | (UInt64)PROTOCOL.INITROTATION;

        TCPClient.Instance.PackingData(Protocol, PackingData(verticalRotation, horizontalRotation));
    }
    public void PlayerRotationCheck(float Mouse_Y, float Mouse_X)
    {
        Debug.Log("PlayerRotationCheck");
        UInt64 Protocol = (UInt64)CLASS_STATE.PLAYER_STATE | (UInt64)STATE.MOVEMENT | (UInt64)PROTOCOL.ROTATION;

        TCPClient.Instance.PackingData(Protocol, PackingData(Mouse_Y, Mouse_X));
    }
    public void Player_MoveMent(bool[] _inputs, int serial)
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.PLAYER_STATE | (UInt64)STATE.MOVEMENT | (UInt64)PROTOCOL.POSITION;
        Debug.Log(string.Format("temp = {0:x}", Protocol));
        Debug.Log($"TCPClient.Instance.myId = {TCPClient.m_Player.my_id}");
        Debug.Log($"serial = {serial}");
        TCPClient.Instance.PackingData(Protocol, PackingData(_inputs, Player_Manager.players[TCPClient.m_Player.my_id].transform.rotation));
    }
    public bool Player_Init_SendMessage()
    {
        UInt64 Protocol = (UInt64)CLASS_STATE.PLAYER_STATE | (UInt64)STATE.ONLINE;
        Debug.Log(string.Format("Player_Init_SendMessage = {0:x}", Protocol));
        TCPClient.Instance.PackingData(Protocol);
        return true;
    }
    public static void PlayerPosition(int serial,Vector3 _position)
    {
        Player_Manager.players[serial].transform.position = _position;
    }

    public static void PlayerRotation(int serial,Quaternion _rotation)
    {
        Player_Manager.players[serial].transform.rotation = _rotation;
    }

    public static void PlayerRotation(int serial, float verticalRotation, float horizontalRotation)
    {
        Player_Manager.players[serial].transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        Player_Manager.players[serial].transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }
    public override void RecvProcess()
    {
        UInt64 Protocol = TCPClient.Instance.GetProtocol() & (UInt64)FULL_CODE.PROTOCOL;
        Debug.Log(string.Format("temp = {0:x}", Protocol));
        int id;
        Vector3 position=Vector3.zero;
        Quaternion rotation = Quaternion.identity ;
        float x = 0.0f;
        float y = 0.0f;
        switch ((PROTOCOL)Protocol)
        {
            case PROTOCOL.SPAWN:
                Debug.Log("SPAWN");
                UnPackingData(TCPClient.Instance.UnPackingData(), out id, out position, out rotation);
                Debug.Log(string.Format("id = {0:x}", id));
                Player_Manager.instance.SpawnPlayer(id, position, rotation);
                break;
            case PROTOCOL.ROTATION:
                Debug.Log("ROTATION");
                UnPackingData(TCPClient.Instance.UnPackingData(), out id, out y, out x);
                Debug.Log(string.Format("serial Rotation = {0:x}", id));
                PlayerRotation(id, y, x);
                break;
            case PROTOCOL.POSITION:
                Debug.Log("POSITION");
                UnPackingData(TCPClient.Instance.UnPackingData(), out id, out position);
                Debug.Log(string.Format("serial Position = {0:x}", id));
                PlayerPosition(id, position);
                break;            
        }
    }
}
