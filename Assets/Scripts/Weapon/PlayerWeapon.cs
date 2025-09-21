[System.Serializable]
public class PlayerWeapon // 플레이어가 소유한 무기 데이터와 상태를 관리
{
    public WeaponData weaponData; // 이 무기의 기본 정보 
    public int currentLevel; // 플레이어가 소유한 이 무기 현재 레벨
    
    // PlayerWeapon 클래스 생성자
    // WeaponData와 이 무기 인스턴스(무기 틀)의 초기 데이터와 레벨을 받음
    public PlayerWeapon(WeaponData data, int initialLevel)
    {
        weaponData = data;
        currentLevel = initialLevel;
    }
    
    public int GetCurrentDamage() // 이 PlayerWeapon 인스턴스의 'currentLevel'을 사용
    {
        // WeaponData의 GetDamage 메서드를 활용하여 현재 레벨의 데미지를 계산
        return weaponData.GetDamage(currentLevel);
    }
    
    public float GetCurrentCritChance() // 이 PlayerWeapon 인스턴스의 'currentLevel'을 사용
    { 
        // WeaponData의 GetCritChance 메서드를 활용하여 현재 레벨의 치명타 확률을 계산
        return weaponData.GetCritChance(currentLevel);
    }
    
    public void WeaponLevelUp() // 무기 업그레이드시 레벨업
    {
        currentLevel++; // 현재 무기 레벨 증가
        GetCurrentCritChance();
        GetCurrentDamage();
    }
}