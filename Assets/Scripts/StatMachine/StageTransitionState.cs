using UnityEngine;

public class StageTransitionState : IGameState
{
    private GameStateMachine _fsm;
    public StageTransitionState(GameStateMachine fsm)
    {
        _fsm = fsm;
    }

    public void Enter()
    {
        // Debug.Log("스테이지 전환 연출 시작");
        //BGM 볼륨 다운, 배경 변경, 짧은 연출 타이머머
    }

    public void Execute()
    {
        _fsm.ChangeState(new ReadyState(_fsm));
    }

    public void Exit()
    {
        // Debug.Log("스테이지 전환 연출 종료");
        //BGM 볼륨 업 등.
    }
}
