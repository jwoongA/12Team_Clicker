using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour // 플레이어가 소유한 무기를 관리하는 매니저
{
    public static InventoryManager Instance { get; private set; }

    // 플레이어가 소유한 무기 리스트
    public List<PlayerWeapon> playerWeapons = new List<PlayerWeapon>();

    [Header("게임 시작 시 자동 장착될 무기")] // (SO 데이터 참조)
    [SerializeField] private WeaponData initialEquippedWeapon; // 초기 장착된 무기

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

    void Start()
    {
        // 초기 장착 무기 초기화
        // 이 시점에서 플레이어가 소유한 무기 리스트에 무기가 없으므로 null 반환
        PlayerWeapon initWeapon = GetPlayerWeapon(initialEquippedWeapon);

        if (initWeapon == null) // 초기 무기가 없을 때
        {
            // 인스펙터에서 할당한 무기 데이터를 소유 중인 무기 리스트에 추가
            AddNewWeapon(initialEquippedWeapon);
            // 그 무기를 리스트에서 다시 가져와서 initWeapon에 저장
            initWeapon = GetPlayerWeapon(initialEquippedWeapon);
        }

        EquipmentManager.Instance.EquipWeapon(initWeapon); // 초기 무기를 장착
    }

    public void AddNewWeapon(WeaponData weaponData) // 새로운 무기(기본 데이터) 추가
    {
        // 매개변수로 받아온 무기의 기본 데이터와 초기 레벨을 사용하여
        // 플레이어가 무기 리스트에 PlayerWeapon 클래스의 새 인스턴스(무기 틀)를 생성함
        PlayerWeapon newWeapon = new PlayerWeapon(weaponData, weaponData.weaponLevel);
        playerWeapons.Add(newWeapon); // 새로운 무기를 소유 중인 무기 리스트에 추가
    }

    // 소유 중인 무기 리스트에서 특정 WeaponData를 가진 PlayerWeapon 인스턴스를 찾는 메서드
    // 매개변수 data는 우리가 찾으려는 무기의 기본 데이터(SO)
    public PlayerWeapon GetPlayerWeapon(WeaponData data)
    {
        // playerWeapons 리스트를 순회하면서, PlayerWeapon의 weaponData가 매개변수(찾으려는 무기)의 데이터와 동일한지 비교
        // 조건을 만족하면 첫 번째 PlayerWeapon 인스턴스를 반환, 없으면 null 반환
        return playerWeapons.Find(w => w.weaponData == data);
    }
}