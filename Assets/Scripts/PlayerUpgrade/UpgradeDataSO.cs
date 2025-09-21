using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    CritMulitiplier,
    GoldMulitiplier,
    AutoAttackSpeed
}

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Scriptable Object/Upgrade Data SO")]
public class UpgradeDataSO : ScriptableObject
{
    public string upgradeName; // 업그레이드 이름
    public UpgradeType upgradeType;
    public List<UpgradeLevelData> levelDataList = new List<UpgradeLevelData>();

    [ContextMenu("Generate Auto Upgrade Data")]
    public void GenerateAutoData()
    {
        levelDataList.Clear();

        int maxLevel = 100;

        for (int i = 0; i <= maxLevel; i++)
        {
            float value = 0f;
            int cost = 0;

            switch (upgradeType)
            {
                case UpgradeType.CritMulitiplier:
                    value = i * 50f;
                    cost = i * 10;
                    break;

                case UpgradeType.GoldMulitiplier:
                    value = i * 100f;
                    cost = i * 10;
                    break;

                case UpgradeType.AutoAttackSpeed:
                    value = i * 0.3f;
                    cost = i * 10;
                    break;
            }

            levelDataList.Add(new UpgradeLevelData
            {
                level = i,
                value = value,
                cost = cost
            });
        }
    }
}