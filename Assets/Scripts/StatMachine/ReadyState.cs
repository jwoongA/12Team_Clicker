using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ReadyState : IGameState
{
    private GameStateMachine _fsm;

    public ReadyState(GameStateMachine fsm)
    {
        _fsm = fsm;
    }

    public void Enter()
    {
        //난이도 선택 UI 보여주기
        // Debug.Log("준비 상태 돌입");

        // Debug.Log("사운드2D : 플레이 호출 성공.");

        // StartCoroutine(PlayBGM());
        // SoundManager.Instance.PlaySound2D("BossMain");
    }


    public void Execute()
    {
        // GameManager.Instance.StartNewGame();
        //난이도 선택 시 PlayState로 전환
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // _fsm.ChangeState(new PlayState(_fsm));
        }
    }

    public void Exit()
    {
        // Debug.Log("준비 상태 종료");
        //준비 상태 종료: UI 숨기기
        //난이도 선택 UI 숨기기
    }
}

