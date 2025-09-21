using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IGameState
{
    private GameStateMachine _fsm;
    public GameOverState(GameStateMachine fsm)
    {
        _fsm = fsm;
    }

    public void Enter()
    {
        // Debug.Log("게임 오버");
        //게임 오버 UI 호출.
    }

    public void Execute()
    {

    }

    public void Exit() { }
}
