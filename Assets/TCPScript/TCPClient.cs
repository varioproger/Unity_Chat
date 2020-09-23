using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCP
{
    public class TCPClient : Packing
    {
        public  static TCPClient Instance;
        static State m_State;

        [HideInInspector] public static IntroState m_Intro;
        [HideInInspector] public static LoginState m_Login;
        [HideInInspector] public static ChatState m_Chat;
        [HideInInspector] public static LobbyState m_Lobby;
        [HideInInspector] public static PlayerState m_Player;

        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this);

            if (Instance != null)
            {
                GameObject obj = GameObject.Find("ServerManager");
                DestroyImmediate(obj);
            }
            else
            {
                Instance = this;
                gameObject.name = "ServerManager_Orizin";
                Begin();

                m_Intro = new IntroState();
                m_Login = new LoginState();
                m_Chat = new ChatState();
                m_Lobby = new LobbyState();
                m_Player = new PlayerState();
                m_Intro.Intro_Message();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_RecvBuffer.Count != 0)
            {
                var SubState = GetProtocol() & (UInt64)FULL_CODE.MAIN;

                switch ((CLASS_STATE)SubState)
                {
                    case CLASS_STATE.INIT_STATE:
                        SetState(m_Intro);
                        break;
                    case CLASS_STATE.LOGIN_STATE:
                        SetState(m_Login);
                        break;
                    case CLASS_STATE.CHAT_STATE:
                        SetState(m_Chat);
                        break;
                    case CLASS_STATE.LOBBY_STATE:
                        SetState(m_Lobby);
                        break;
                    case CLASS_STATE.PLAYER_STATE:
                        SetState(m_Player);
                        break;

                }
                m_State.RecvProcess();
            }
        }

        public void SetState(State _state)
        {
            m_State = _state;
        }

        public State GetState()
        {
            return m_State;
        }

        public void ExitProgram()
        {            
            End();
            Application.Quit();
        }

        private void OnApplicationQuit()
        {
            End();
        }
    }
}