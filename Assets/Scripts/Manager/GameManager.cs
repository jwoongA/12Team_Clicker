using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public FinalStats CurrentStats { get; private set; } // 최종 스탯 산출식을 저장한 변수.

    [Header("스테이지/레벨")]
    // public int levelCount = PlayerData.Instance.Level; //레벨 카운트
    public int currentStage = 1; //현재 스테이지
    public int levelsPerStage = 10; //10레벨마다 스테이지 전환


    [Header("보상")]
    public int baseKillReward; // 적 하나 당 기본 지급할 재화.
    public float difficultyMultiplier = 1; // 난이도에 따른 차등 지급.

    public event Action<int /*새로운 스테이지*/> OnStageChanged;
    public event Action<int /*재화*/> OnGoldGained;
    public event Action<int /*능력 포인트*/> OnPointGained;

    //스테이트 머신
    private GameStateMachine _fsm;

    //테스트용
    public GameObject OptionUI;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //최초 계산.
        CurrentStats = StatsCalculator.CalculateFinalStats(PlayerData.Instance);
        //Player 스탯이 바뀌면 다시 계산.
        PlayerData.Instance.OnPlayerStatChanged += Recalculate; // 구독
    }
    void Recalculate() // 스탯값이 바뀔 때마다 자동으로 최종 스탯값을 다시 계산.
    {
        CurrentStats = StatsCalculator.CalculateFinalStats(PlayerData.Instance);
    }

    void Start()
    {
        _fsm = new GameStateMachine();
        _fsm.ChangeState(new ReadyState(_fsm)); //상태 머신 ReadyState 전환.

        BtnClick.OnNewGameButtonClick += StartNewGame; // 새 게임 시작 시 상태머신 PlayState로 전환.

        OnGoldGained += AddGold; //골드 증가.
        OnPointGained += AddPoint; //포인트 증가.
    }

    void Update()
    {
        _fsm.Update();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            OptionUI.SetActive(!OptionUI.activeSelf);
        }
    }


    //게임 플레이 관련.
    public void StartNewGame() //새 게임 시작 시 상태머신: 플레이 상태로 전환.
    {
        _fsm.ChangeState(new PlayState(_fsm));
    }


    // 난이도 관련.
    public void EnemyDefeated(int enemyGold, int enemyPoint) // 적 처치마다 레벨 카운트 증가 및 리워드 추가.
    {
        PlayerData.Instance.Level++;
        CheckStageChange();

        int goldReward = Mathf.CeilToInt((baseKillReward + enemyGold) * CurrentStats.BonusGold);
        int pointReward = Mathf.CeilToInt(baseKillReward + enemyPoint);

        OnGoldGained?.Invoke(goldReward);
        OnPointGained?.Invoke(pointReward);
        // Debug.Log($"리워드: {goldReward}G, {pointReward}P");
    }

    private void CheckStageChange() // 레벨 카운트가 10 단위마다 난이도 조정. >> 작동 확인.
    {
        if (PlayerData.Instance.Level < currentStage * levelsPerStage) return;

        currentStage++;
        difficultyMultiplier = 1f + (currentStage - 1) * 1f;
        OnStageChanged?.Invoke(currentStage);

        // Debug.Log("난이도 값 체크: " + difficultyMultiplier);
    }


    //이벤트 구독 관련
    public void AddGold(int addGold)
    {
        PlayerData.Instance.Gold += addGold;
    }
    public void AddPoint(int addPoint)
    {
        PlayerData.Instance.Point += addPoint;
    }

    void OnDestroy() // 구독 해제.
    {
        PlayerData.Instance.OnPlayerStatChanged -= Recalculate;
        OnGoldGained -= AddGold;
        OnPointGained -= AddPoint;
    }
}
