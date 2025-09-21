
public class FinalStats
{
    public int Attack { get; private set; }
    public float CritChance { get; private set; }
    public float CritAttack { get; private set; }
    public float BonusGold { get; private set; }
    public float AutoAttackSpeed { get; private set; }


    public FinalStats(int attack, float critChance, float critAttack, float bonusGold, float autoAttackSpeed)
    {
        Set(attack, critChance, critAttack, bonusGold, autoAttackSpeed);
    }
    
    public void Set(int attack, float critChance, float critAttack, float bonusGold, float autoAttackSpeed)
    {
        Attack = attack;
        CritChance = critChance;
        CritAttack = critAttack;
        BonusGold = bonusGold;
        AutoAttackSpeed = autoAttackSpeed;
    }
}
