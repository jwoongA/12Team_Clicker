using System;

public static class StatsCalculator
{
    public static FinalStats CalculateFinalStats(PlayerData data)
    {
        //최종 데미지(기본 공격력 + 추가 공격력)
        int finalAttack = data.BaseAttack + data.BonusAttack;

        //최종 치명
        float finalCritChance = (data.BaseCritChance / 100f) + (data.BonusCritChance / 100f);
        float finalCritAttack = finalAttack * ((data.BaseCritMultiplier / 100f) + (data.BonusCritMultiplier / 100f));

        float finalGoldBonus = (data.BaseGold / 100f) + (data.BonusGold / 100f);

        float finalAutoAttackSpeed = 0f;
        if (data.BonusAutoAttackSpeed > 0)
        {
           finalAutoAttackSpeed = (data.BaseAutoAttackSpeed) + (data.BonusAutoAttackSpeed);
        }

        //최종 치명 확률
        finalCritChance = Math.Clamp(finalCritChance, 0f, 100f);

        return new FinalStats(finalAttack, finalCritChance, finalCritAttack, finalGoldBonus, finalAutoAttackSpeed);
    }

    // private static float CalculateFinalCritDamage(int attack, float critMultiplier)
    // {
    //     return attack * critMultiplier;
    // }
}
