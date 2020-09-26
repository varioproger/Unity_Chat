using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using System.Net.Sockets;

namespace TCP
{
    public class Packing : TCPSocket
    {

        /// <summary>Full Protocol from Server</summary>
        public enum FULL_CODE : UInt64
        {
            MAIN = 0xff00000000000000,
            SUB = 0x00ff000000000000,
            PROTOCOL = 0x0000ff0000000000
        };

        /// <summary>Sent from server to client.</summary>
        public enum CLASS_STATE : UInt64
        {
            NO_STATE = 0x0000000000000000,
            INIT_STATE = 0x0100000000000000,
            LOGIN_STATE = 0x0200000000000000,
            CHAT_STATE = 0x0300000000000000,
            LOBBY_STATE = 0x0400000000000000,
            PLAYER_STATE = 0x0500000000000000,
        }

        /// <summary>Sent from client to server.</summary>
        public enum PROTOCOL : UInt64
        {
            JOIN_INFO = 0x0000010000000000,
            JOIN_RESULT = 0x0000020000000000,
            LOGIN_INFO = 0x0000030000000000,
            LOGIN_RESULT = 0x0000040000000000,
            SEND = 0x0000010000000000,
            RECV = 0x0000020000000000,
            SPAWN = 0x0000010000000000,
            POSITION = 0x0000020000000000,
            INITROTATION = 0x0000040000000000,
            ROTATION = 0x0000050000000000,
            CHARACTERFORWARD = 0x0000060000000000
        };

        public enum STATE : UInt64
        {
            MENU_SELECT_STATE = 0x0001000000000000,
            SEND_RESULT_STATE = 0x0002000000000000,
            RECV_STATE = 0x0001000000000000,
            SEND_STATE = 0x0002000000000000,
            ENTER = 0x0001000000000000,
            LEAVE = 0x0002000000000000,
            ONLINE = 0x0001000000000000,
            MOVEMENT = 0x0002000000000000,
            OFFLINE = 0x0003000000000000,
        };

        public enum LOBBY_RESULT
        {
            NOT_READY,
            READY
        };

        public enum INFO_RESULT
        {
            NODATA = -1,
            JOIN_SUCCESS = 1,
            ERROR_JOIN_EXISTS,
            LOGIN_SUCCESS,
            ERROR_LOGIN_ID,
            ERROR_LOGIN_PW,
            ERROR_LOGIN_EXISTS
        };


        public enum RESULT
        {
            NODATA = -1,
            JOIN_SUCCESS = 1,
            ERROR_JOIN_EXISTS,
            LOGIN_SUCCESS,
            ERROR_LOGIN_ID,
            ERROR_LOGIN_PW,
            ERROR_LOGIN_EXISTS
        };

        public Queue<SendBuffer> m_SendBuffer;
        public Queue<RecvBuffer> m_RecvBuffer;

        Thread SendThread;
        Thread RecvThread;

        public AutoResetEvent m_SendEvent;

        static public object m_lock;
        static public object m_Sendlock;

        public void Begin()
        {
            if (!Connecting("127.0.0.1", 9050))
                Application.Quit();

            m_lock = new object();
            m_Sendlock = new object();
            m_SendEvent = new AutoResetEvent(false);

            m_SendBuffer = new Queue<SendBuffer>();
            m_RecvBuffer = new Queue<RecvBuffer>();

            SendThread = new Thread(new ThreadStart(SendProcess));
            RecvThread = new Thread(new ThreadStart(RecvProcess));

            SendThread.Start();
            RecvThread.Start();
        }

        public void End()
        {
            m_SendBuffer.Clear();
            m_RecvBuffer.Clear();

            SendThread.Abort();
            RecvThread.Abort();

            Release();
        }

        void SendProcess()
        {
            while (true)
            {

                if (m_SendBuffer.Count == 0)
                {
                    m_SendEvent.WaitOne();
                }
                if (m_SendBuffer.Count != 0)
                {
                    Send(m_SendBuffer.Dequeue());
                }
            }
        }

        void RecvProcess()
        {
            while (true)
            {

                RecvBuffer data = new RecvBuffer();
                try
                {
                        data.buffer = new byte[4];
                        data.size = sizeof(int);
                        data.offset = 0;

                        int retval = Recv(data);
                        data.size = BitConverter.ToInt32(data.buffer, 0);
                        data.buffer = new byte[data.size];

                        while (data.size > 0)
                        {
                            retval = Recv(data);
                            if (retval == 0)
                                break;

                            data.size -= retval;
                            data.offset += retval;
                        }

                        data.size = data.offset;
                }
                catch (SocketException e)
                {
                    print("Socket Error : " + e.Message.ToString());
                    continue;
                }
                m_RecvBuffer.Enqueue(data);
            }
        }

        public void PackingData(UInt64 Protocol)
        {
            lock (m_lock)
            {
                SendBuffer data = new SendBuffer();
                data.buffer = new byte[4096];
                data.size = 0;

                data.offset = sizeof(int);

                Buffer.BlockCopy(BitConverter.GetBytes(Protocol), 0, data.buffer, data.offset, sizeof(UInt64));
                data.size += sizeof(UInt64);

                Buffer.BlockCopy(BitConverter.GetBytes(data.size), 0, data.buffer, 0, sizeof(int));
                data.size += sizeof(int);

                m_SendBuffer.Enqueue(data);
                m_SendEvent.Set();
            }
        }

        public void PackingData(UInt64 Protocol, byte[] buffer)
        {
            lock(m_lock)
            {
                SendBuffer data = new SendBuffer();
                data.buffer = new byte[4096];
                data.size = 0;

                data.offset = sizeof(int);

                Buffer.BlockCopy(BitConverter.GetBytes(Protocol), 0, data.buffer, data.offset, sizeof(UInt64));
                data.offset += sizeof(UInt64);
                data.size += sizeof(UInt64);

                if (buffer.Length != 0)
                {
                    Buffer.BlockCopy(buffer, 0, data.buffer, data.offset, buffer.Length);
                    data.size += buffer.Length;
                }
                
                Buffer.BlockCopy(BitConverter.GetBytes(data.size), 0, data.buffer, 0, sizeof(int));
                data.size += sizeof(int);

                m_SendBuffer.Enqueue(data);
                m_SendEvent.Set();
            }
        }

        public UInt64 GetProtocol()
        {
            lock (m_lock)
            {
                return BitConverter.ToUInt64(m_RecvBuffer.Peek().buffer, 0);
            }
        }

        public RecvBuffer UnPackingData()
        {
            lock (m_lock)
            {
                return m_RecvBuffer.Dequeue();
            }
        }
    }
}