using System.Collections.Generic;
using UnityEngine;

public enum WeaponStatType
{
    Damage, // 대미지
    CriticalChance // 크리티컬 확률
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "WeaponStat")]
public class WeaponData : ScriptableObject // 무기의 스탯 데이터를 저장
{
    [Header("기본 정보")] public string weaponName; // 무기 이름
    public int weaponLevel; // 무기 레벨
    public List<WeaponStatEntry> weaponStats; // 무기 스탯 리스트
    public Sprite weaponSprite; // 무기 스프라이트 

    [Header("강화 정보")] public int maxLevel; // 무기 최대 레벨
    public int damageIncreasePerLevel; // 레벨당 대미지 증가량
    public float critChanceIncreasePerLevel; // 레벨당 크리티컬 확률 증가량

    [Header("구매, 업그레이드 비용")] public int purchaseCost; // 무기 가격
    public int upgradeCost; // 업그레이드 가격

    public int GetDamage(int level) // 무기 SO에서 현재 대미지 값 가져오기
    {
        int baseWeaponDamage = 0; // 무기 기본 대미지

        foreach (WeaponStatEntry weaponStat in weaponStats)
        {
            if (weaponStat.weaponStatType == WeaponStatType.Damage)
            {
                // 무기 SO에 저장된 대미지 기본값을 저장
                baseWeaponDamage = weaponStat.baseValue;
                break;
            }
        }

        // 무기 레벨업 시 증가하는 대미지 량
        // 무기 기본 대미지 + (레벨당 증가하는 대미지 * 무기 레벨)
        return baseWeaponDamage + damageIncreasePerLevel * level;
    }

    public float GetCritChance(int level) // 무기 SO에서 현재 크리티컬 확률 값 가져오기
    {
        int baseWeaponCritChance = 0; // 무기 기본 크리티컬 확률

        foreach (WeaponStatEntry weaponStat in weaponStats)
        {
            if (weaponStat.weaponStatType == WeaponStatType.CriticalChance)
            {
                // 무기 SO에 저장된 크리티컬 확률 기본값을 저장
                baseWeaponCritChance = weaponStat.baseValue;
                break;
            }
        }
        
        // 치명타 확률이 100을 넘어가면 100으로 수치를 고정하고 아닐 경우에는 계산값 적용
        float returnCritChance = baseWeaponCritChance + critChanceIncreasePerLevel * level <= 100 ? baseWeaponCritChance + critChanceIncreasePerLevel * level : 100;
        
        // 무기 레벨업 시 증가하는 치명타 확률
        // 무기 기본 치명타 확률 + (레벨당 증가하는 치명타 확률 * 무기 레벨)
        return returnCritChance;
    }

    public int GetUpgradeCost(int level) // 무기 업그레이드 비용
    {
        return upgradeCost * (level + 1); // 레벨 비례로 강화 비용 증가 - 레벨 1당 기본 가격이 더해진다 생각하면 됨
    }
}

[System.Serializable]
public class WeaponStatEntry // 무기 스탯 정보 정의
{
    public WeaponStatType weaponStatType; // 어떤 종류의 능력치인지
    public int baseValue; // 해당 능력치의 기본 값
}