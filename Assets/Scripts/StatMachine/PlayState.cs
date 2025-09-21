using System.Collections;
using UnityEngine;

public class PlayState : IGameState
{
    private GameStateMachine _fsm;


    public PlayState(GameStateMachine fsm)
    {
        _fsm = fsm;
    }

    public void Enter()
    {
        // Debug.Log("플레이 시작");
        //클릭이나 자동 생산 활성 상태
        StageManager.Instance.StartStage(1);

        GameManager.Instance.StartCoroutine(AutoAttackRoutine());
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
        }
    }
    private IEnumerator AutoAttackRoutine() //자동공격
    {
        while (true)
        {
            FinalStats stats = StatsCalculator.CalculateFinalStats(PlayerData.Instance);
            float autoAttackSpeed = stats.AutoAttackSpeed;

            if (autoAttackSpeed > 0) 
            {
                GameObject.FindObjectOfType<PlayerAttack>().OnEnemyClicked(GameObject.FindObjectOfType<Enemy>());
                yield return new WaitForSeconds(1f / autoAttackSpeed);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    public void Exit()
    {
        // Debug.Log("플레이 종료");
        //클릭, 자동 생산 비활성화.
    }

    public void OnEnemyDefeated() 
    {
        if (PlayerData.Instance.Level >= GameManager.Instance.currentStage * GameManager.Instance.levelsPerStage)
        {
            _fsm.ChangeState(new StageTransitionState(_fsm));
        }
    }
}
