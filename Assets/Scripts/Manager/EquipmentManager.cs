using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour // 무기 장착 관리
{
    public static EquipmentManager Instance { get; private set; }

    public PlayerWeapon EquippedWeapon { get; private set; } // 현재 장착된 무기 인스턴스
    
    public static event Action OnEquipmentChanged; // 장비 변경 시 다른 스크립트에 알리는 이벤트

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipWeapon(PlayerWeapon newWeaponToEquip) // 무기 장착
    {
        // 생각해보면 굳이 해제 시키고 장착 시킬 필요없이 바로 장착 무기에 저장해버리면 됨
        EquippedWeapon = newWeaponToEquip; // 매개변수 무기 장착

        if (EquippedWeapon != null) // 무기 장착 중이면
        {
            // 차라리 처음부터(WeaponData에서) 말고 장착한 시점(장착 메서드가 작동했을 때)에서 추가 스탯 계산
            // WeaponData에 있는 무기 기본 스탯을 가져와 보너스 스탯에 저장
            PlayerData.Instance.BonusAttack = EquippedWeapon.GetCurrentDamage();
            PlayerData.Instance.BonusCritChance = EquippedWeapon.GetCurrentCritChance();
        }

        // 장착이 완료된 후 OnEquipmentChanged 이벤트를 발생
        OnEquipmentChanged?.Invoke();
    }
}