using HappyFunTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Message when player presses or release jump button
public class MessageJump : MessageCmdData
{
    public bool jump = false;
}

// Message when player pressed left or right
public class MessageMove : MessageCmdData
{
    public int dir = 0;  // will be -1, 0, or +1
}


public abstract class MessageActor : MonoBehaviour
{
    public abstract void OnMove(MessageMove data);

    public abstract void OnJump(MessageJump data);

    public abstract void Init();
}

