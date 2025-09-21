using System;
using UnityEngine;

[SerializeField]
public class PlayerData
{
    private static PlayerData _instance;
    public static PlayerData Instance => _instance ?? (_instance = new PlayerData());

    [SerializeField] private int level;
    // [SerializeField] private float lifeTime; //생명주기. (도전)
    [SerializeField] private int gold; //재화: 무기 강화
    [SerializeField] private int point; //스탯포인트: 능력치 강화.
    public event Action OnPlayerStatChanged;


    [SerializeField] private int _level = 1;

    public int Level
    {
        get => _level;
        set
        {
            if (_level != value) // 값이 달라지면 자동으로 갱신됨.
            {
                _level = value;
                OnPlayerStatChanged?.Invoke();
            }
        }
    }

    // [SerializeField] private float _lifeTime = 60f;

    // public float LifeTime
    // {
    //     get => _lifeTime;
    //     set
    //     {
    //         if (Math.Abs(_lifeTime - value) > 0.01f)
    //         {
    //             _lifeTime = value; //이벤트 실행.
    //             OnPlayerStatChanged?.Invoke();
    //         }
    //     }
    // }


    [SerializeField] private int _gold = 10;

    public int Gold // 플레이어가 가지고 있는 돈.
    {
        get => _gold;
        set
        {
            if (_gold != value) // 값이 달라지면 자동으로 갱신됨.
            {
                _gold = value; //이벤트 실행.
                OnPlayerStatChanged?.Invoke();
            }
        }
    }

    [SerializeField] private int _point = 10;

    public int Point
    {
        get => _point;
        set
        {
            if (_point != value) // 값이 달라지면 자동으로 갱신됨.
            {
                _point = value; //이벤트 실행.
                OnPlayerStatChanged?.Invoke();
            }
        }
    }


    // 기본 스탯
    public int BaseAttack { get; set; } = 1;
    public float BaseCritChance { get; set; } = 5f;
    public float BaseCritMultiplier { get; set; } = 150f; // 배율
    public float BaseGold { get; set; } = 100f; // 돈 버는 능력.
    public float BaseAutoAttackSpeed { get; set; } = 0f;

    // 추가 보너스 스탯(장비, 능력치 강화)]
    private int _bonusAttack = 0;
    public int BonusAttack
    {
        get => _bonusAttack;
        set
        {
            if (_bonusAttack != value)
            {
                _bonusAttack = value;
                OnPlayerStatChanged?.Invoke();
            }
        }
    }
    
    private float _bonusCritChance = 0;
    public float BonusCritChance
    {
        get => _bonusCritChance;
        set
        {
            if (_bonusCritChance != value)
            {
                _bonusCritChance = value;
                OnPlayerStatChanged?.Invoke();
            }
        }
    }

    public float BonusCritMultiplier { get; set; } = 0f;
    public float BonusGold { get; set; } = 0f; // 추가 돈을 버는 능력.
    public float BonusAutoAttackSpeed { get; set; } = 0f;

}