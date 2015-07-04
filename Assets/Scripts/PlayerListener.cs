using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerListener : Singleton<PlayerListener> {

    protected List<Action<int>> onPlayerJoinEvents = new List<Action<int>>();

    public void SubscribeToPlayerJoin(Action<int> playerJoinAction)
    {
        onPlayerJoinEvents.Add(playerJoinAction);
    }

    public int numPlayers = 0;

    //Woah, a player actually joined!
    public void PlayerJoin()
    {
        numPlayers += 1;
        CallPlayerJoinActions();
    }

    private void CallPlayerJoinActions()
    {
        foreach(var joinEvent in onPlayerJoinEvents)
        {
            joinEvent(numPlayers);
        }
    }

    public void PlayerLeave()
    {
        numPlayers -= 1;
        CallPlayerJoinActions();
    }
}
