using TMPro;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("Upgrade Data")]
    public UpgradeDataSO criticalMutiplierUpgradeData;
    public UpgradeDataSO autoAttackSpeedUpgradeData;
    public UpgradeDataSO goldMultiplierUpgradeData;

    [Header("Current Levels")]
    public int criticalMutiplierLevel = 0;
    public int autoAttackSpeedLevel = 0;
    public int goldMultiplierLevel = 0;

    [Header("Critical Multiplier")]
    public TextMeshProUGUI criticalMultiplierValueText;
    public TextMeshProUGUI criticalMultiplierCostText;
    public TextMeshProUGUI criticalMultiplierLevelText;
    public GameObject criticalMultiplierMaxLevelUI;

    [Header("Auto Attack Speed")]
    public TextMeshProUGUI autoAttackSpeedValueText;
    public TextMeshProUGUI autoAttackSpeedCostText;
    public TextMeshProUGUI autoAttackSpeedLevelText;
    public GameObject autoAttackSpeedMaxLevelUI;

    [Header("Gold Multiplier")]
    public TextMeshProUGUI goldMultiplierValueText;
    public TextMeshProUGUI goldMultiplierCostText;
    public TextMeshProUGUI goldMultiplierLevelText;
    public GameObject goldMultiplierMaxLevelUI;

    public BtnClick btnClick;

    private void Start()
    {
        // 시작할 때 UI 초기화
        UpdateCriticalMultiplierUI();
        UpdateAutoAttackSpeedUI();
        UpdateGoldMultiplierUI();
    }

    private void OnEnable()
    {
        PlayerData.Instance.OnPlayerStatChanged += RefreshAllUpgradeUI;
    }

    private void OnDisable()
    {
        PlayerData.Instance.OnPlayerStatChanged -= RefreshAllUpgradeUI;
    }

    private void RefreshAllUpgradeUI()
    {
        UpdateCriticalMultiplierUI();
        UpdateAutoAttackSpeedUI();
        UpdateGoldMultiplierUI();
    }

    public void UpgradeCriticalMultiplier()
    {
        int nextLevel = criticalMutiplierLevel + 1;

        // 다음 레벨이 업그레이드 테이블에 정의된 최대 레벨을 넘는지 검사
        if (nextLevel > criticalMutiplierUpgradeData.levelDataList.Count - 1)
        {
            return;
        }

        // 업그레이드 비용
        int cost = criticalMutiplierUpgradeData.levelDataList[nextLevel].cost;

        if (PlayerData.Instance.Point >= cost)
        {
            SoundManager.Instance.PlaySound2D("sfx_player_pickup_bonus");
            PlayerData.Instance.Point -= cost;

            criticalMutiplierLevel = nextLevel;

            // 스탯 반영
            float bonusValue = criticalMutiplierUpgradeData.levelDataList[criticalMutiplierLevel].value;
            PlayerData.Instance.BonusCritMultiplier = bonusValue;

            UpdateCriticalMultiplierUI();
        }
        else
        {
            btnClick.ShowNotEnoughPointPopup();
        }

    }

    public void UpgradeAutoAttackSpeed()
    {
        int nextLevel = autoAttackSpeedLevel + 1;

        // 다음 레벨이 업그레이드 테이블에 정의된 최대 레벨을 넘는지 검사
        if (nextLevel > autoAttackSpeedUpgradeData.levelDataList.Count - 1)
        {
            return;
        }

        // 업그레이드 비용
        int cost = autoAttackSpeedUpgradeData.levelDataList[nextLevel].cost;

        if (PlayerData.Instance.Point >= cost)
        {
            SoundManager.Instance.PlaySound2D("sfx_player_pickup_bonus");
            PlayerData.Instance.Point -= cost;

            autoAttackSpeedLevel = nextLevel;

            // 스탯 반영
            float bonusValue = autoAttackSpeedUpgradeData.levelDataList[autoAttackSpeedLevel].value;
            PlayerData.Instance.BonusAutoAttackSpeed = bonusValue;

            UpdateAutoAttackSpeedUI();
        }
        else
        {
            btnClick.ShowNotEnoughPointPopup();
        }

    }

    public void UpgradeGoldMultiplier()
    {
        int nextLevel = goldMultiplierLevel + 1;

        // 다음 레벨이 업그레이드 테이블에 정의된 최대 레벨을 넘는지 검사
        if (nextLevel > goldMultiplierUpgradeData.levelDataList.Count - 1)
        {
            return;
        }

        // 업그레이드 비용
        int cost = goldMultiplierUpgradeData.levelDataList[nextLevel].cost;

        if (PlayerData.Instance.Point >= cost)
        {
            SoundManager.Instance.PlaySound2D("sfx_player_pickup_bonus");
            PlayerData.Instance.Point -= cost;

            goldMultiplierLevel = nextLevel;

            // 스탯 반영
            float bonusValue = goldMultiplierUpgradeData.levelDataList[goldMultiplierLevel].value;
            PlayerData.Instance.BonusGold = bonusValue;

            UpdateGoldMultiplierUI();
        }
        else
        {
            btnClick.ShowNotEnoughPointPopup();
        }

    }

    public void UpdateCriticalMultiplierUI()
    {
        // 최대 레벨이면 MaxLevel UI 표시
        bool isMaxLevel = criticalMutiplierLevel >= criticalMutiplierUpgradeData.levelDataList.Count - 1;
        criticalMultiplierMaxLevelUI.SetActive(isMaxLevel);

        // Value 업데이트
        criticalMultiplierValueText.text = $"{criticalMutiplierUpgradeData.levelDataList[criticalMutiplierLevel].value:F1}%";

        // Cost 업데이트
        if (!isMaxLevel)
        {
            int cost = criticalMutiplierUpgradeData.levelDataList[criticalMutiplierLevel + 1].cost;
            criticalMultiplierCostText.text = $"{cost}";

            if (PlayerData.Instance.Point >= cost)
            {
                criticalMultiplierCostText.color = Color.black;
            }
            else
            {
                criticalMultiplierCostText.color = Color.red;
            }
        }
        else
        {
            criticalMultiplierCostText.text = "";
        }

        criticalMultiplierLevelText.text = $"치명타 {criticalMutiplierLevel}";
    }

    public void UpdateAutoAttackSpeedUI()
    {
        // 최대 레벨이면 MaxLevel UI 표시
        bool isMaxLevel = autoAttackSpeedLevel >= autoAttackSpeedUpgradeData.levelDataList.Count - 1;
        autoAttackSpeedMaxLevelUI.SetActive(isMaxLevel);

        // Value 업데이트
        autoAttackSpeedValueText.text = $"{autoAttackSpeedUpgradeData.levelDataList[autoAttackSpeedLevel].value:F1}회/초";

        // Cost 업데이트
        if (!isMaxLevel)
        {
            int cost = autoAttackSpeedUpgradeData.levelDataList[autoAttackSpeedLevel + 1].cost;
            autoAttackSpeedCostText.text = $"{cost}";

            if (PlayerData.Instance.Point >= cost)
            {
                autoAttackSpeedCostText.color = Color.black;
            }
            else
            {
                autoAttackSpeedCostText.color = Color.red;
            }
        }
        else
        {
            autoAttackSpeedCostText.text = "";
        }

        autoAttackSpeedLevelText.text = $"자동 공격 {autoAttackSpeedLevel}";
    }

    public void UpdateGoldMultiplierUI()
    {
        // 최대 레벨이면 MaxLevel UI 표시
        bool isMaxLevel = goldMultiplierLevel >= goldMultiplierUpgradeData.levelDataList.Count - 1;
        goldMultiplierMaxLevelUI.SetActive(isMaxLevel);

        // Value 업데이트
        goldMultiplierValueText.text = $"{goldMultiplierUpgradeData.levelDataList[goldMultiplierLevel].value:F1}%";

        // Cost 업데이트
        if (!isMaxLevel)
        {
            int cost = goldMultiplierUpgradeData.levelDataList[goldMultiplierLevel + 1].cost;
            goldMultiplierCostText.text = $"{cost}";

            if (PlayerData.Instance.Point >= cost)
            {
                goldMultiplierCostText.color = Color.black;
            }
            else
            {
                goldMultiplierCostText.color = Color.red;
            }
        }
        else
        {
            goldMultiplierCostText.text = "";
        }

        goldMultiplierLevelText.text = $"골드 획득 {goldMultiplierLevel}";
    }
}