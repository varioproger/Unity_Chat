using System.Collections;
using System.Collections.Generic;
using TCP;
using UnityEngine;

public abstract class State : TCPClient
{
    public abstract void RecvProcess();
    //public abstract void SendProcess();
}
