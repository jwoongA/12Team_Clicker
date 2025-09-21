using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 무기들을 보여주고, 무기 가방에서 구매, 장착, 강화 기능을 담당
public class WeaponSlotUI : MonoBehaviour
{
    private BtnClick _btnClick;

    [Header("무기가방 UI 참조")] [SerializeField]
    private Image weaponImage; // 무기 스프라이트 Image 컴포넌트

    [SerializeField] private TextMeshProUGUI weaponNameText; // 무기 이름 텍스트
    [SerializeField] private TextMeshProUGUI weaponDamageText; // 무기 공격력 텍스트
    [SerializeField] private TextMeshProUGUI weaponCritChanceText; // 무기 치명타 확률 텍스트
    [SerializeField] private TextMeshProUGUI weaponCostText; // 무기 가격
    [SerializeField] private TextMeshProUGUI weaponUpgradeCostText; // 무기 업그레이드 가격
    [SerializeField] private GameObject notEnoughGold; // 돈 부족 문구
    [SerializeField] private GameObject equippedWeaponText; // 무기 장착 중 표시 텍스트

    [Header("버튼 GameObject 참조")] public GameObject purchaseButton; // 무기 구매 버튼
    public GameObject equipButton; // 무기 장착 버튼
    public GameObject upgradeButton; // 무기 업그레이드 버튼

    [Header("무기 SO 에셋")] public WeaponData weaponDataAsset;

    private WeaponData _currentWeaponData; // 현재 이 UI 슬롯이 나타내는 무기 데이터

    // 이 슬롯에 해당하는 무기의 소유 여부와 현재 레벨 (게임 시작 시 초기화)
    private bool _hasWeapon; // 무기를 소유하고 있는지
    private int _currentWeaponLevel; // 현재 무기 레벨

    void Awake()
    {
        if (weaponDataAsset != null) // weaponDataAsset이 할당되었는지 확인
        {
            _currentWeaponData = weaponDataAsset; // 이 UI 슬롯이 다룰 무기 데이터를 할당
        }
        else // 할당되지 않았다면
        {
            enabled = false; // 스크립트를 비활성화하여 추가 오류를 방지
            return; // 메서드 실행을 즉시 종료
        }

        // 초기 상태 설정: InventoryManager에서 해당 무기를 가지고 있는지 확인
        PlayerWeapon existingWeapon = InventoryManager.Instance.GetPlayerWeapon(_currentWeaponData);
        if (existingWeapon != null) // 무기를 가지고 있다면
        {
            _hasWeapon = true; // 소유 중으로 설정
            _currentWeaponLevel = existingWeapon.currentLevel; // 무기의 현재 레벨을 가져옴
        }
        else
        {
            _hasWeapon = false; // 소유 중아님으로 설정
            _currentWeaponLevel = _currentWeaponData.weaponLevel; // 구매 전에는 WeaponData의 기본 레벨 표시
        }

        // EquipmentManager에서 무기 장비 상태가 변경될 때마다 OnEquipmentChanged 메서드가 호출
        // 장비 변경 시 UI를 자동으로 갱신
        EquipmentManager.OnEquipmentChanged += EquipmentChanged;

        // PlayerData에서 플레이어의 골드(또는 다른 스탯)가 변경되는 순간
        // WeaponSlotUI에 있는 WeaponSlotUIGoldChanged()가 자동으로 실행되도록 연결
        PlayerData.Instance.OnPlayerStatChanged += WeaponSlotUIGoldChanged;
    }

    void Start()
    {
        // 초기화
        _btnClick = FindObjectOfType<BtnClick>();
        UpdateWeaponSlotUI();
    }

    void OnDestroy() // 메모리 누수 방지
    {
        if (EquipmentManager.Instance != null)
        {
            // 구독한 이벤트 해제
            EquipmentManager.OnEquipmentChanged -= EquipmentChanged;
        }

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.OnPlayerStatChanged -= WeaponSlotUIGoldChanged;
        }
    }

    private void EquipmentChanged() // 장착하면 UI 최신화
    {
        UpdateWeaponSlotUI();
    }

    private void WeaponSlotUIGoldChanged() // 플레이어의 골드가 바뀔 때 호출되는 메서드
    {
        UpdateWeaponSlotUI();
    }

    public void OnWeaponBuy() // 무기 구매 처리
    {
        // 현재 UI 슬롯에 할당된 무기 데이터가 없으면 함수를 즉시 종료
        if (_currentWeaponData == null) return;

        int cost = _currentWeaponData.purchaseCost; // 현재 무기의 구매 비용을 가져옴

        if (PlayerData.Instance.Gold >= cost) // 플레이어의 현재 골드 >= 무기 구매 비용
        {
            SoundManager.Instance.PlaySound2D("coin");
            PlayerData.Instance.Gold -= cost; // 플레이어 골드에서 차감

            InventoryManager.Instance.AddNewWeapon(_currentWeaponData); // 보유 무기 리스트에 추가

            _hasWeapon = true; // 소유 중
            // UI에 나타내는 무기의 현재 레벨 = 그 무기의 기본 데이터(WeaponData)에 설정된 초기 레벨로 설정
            _currentWeaponLevel = _currentWeaponData.weaponLevel;

            UpdateWeaponSlotUI();
        }
        else
        {
            _btnClick.ShowNotEnoughGoldPopup(); // 가격 부족 팝업 출력
        }
    }

    public void OnWeaponUpgrade() // 무기 강화 처리
    {
        // 현재 UI 슬롯에 할당된 무기 데이터가 없으면 함수를 즉시 종료
        if (_currentWeaponData == null) return;

        PlayerWeapon weaponToUpgrade = InventoryManager.Instance.GetPlayerWeapon(_currentWeaponData);

        // 무기 강화 비용은 무기 레벨과 비례로 증가하게 되어있음
        int upgradeCost = _currentWeaponData.GetUpgradeCost(weaponToUpgrade.currentLevel); // 현재 무기의 강화 비용을 가져옴

        if (PlayerData.Instance.Gold >= upgradeCost) // 플레이어의 현재 골드 >= 무기 구매 비용
        {
            PlayerData.Instance.Gold -= upgradeCost; // 플레이어 골드에서 차감
            SoundManager.Instance.PlaySound2D("coin");

            weaponToUpgrade.WeaponLevelUp(); // 강화 시 무기 레벨업
            _currentWeaponLevel = weaponToUpgrade.currentLevel; // UI표시 레벨을 장착 중인 무기 레벨로 반영

            // 장착 중인 무기와 업그레이드한 무기가 같다면
            if (EquipmentManager.Instance.EquippedWeapon == weaponToUpgrade)
            {
                // 메인 화면에 무기UI를 바로 업데이트 하기 위함
                // EquipmentManager의 EquipWeapon()을 호출하여 UI 업데이트 이벤트 발동
                EquipmentManager.Instance.EquipWeapon(weaponToUpgrade);
            }

            UpdateWeaponSlotUI();
        }
        else
        {
            _btnClick.ShowNotEnoughGoldPopup(); // 가격 부족 팝업 출력
        }
    }

    public void OnEquipWeapon() // 무기 장착 처리
    {
        // 현재 UI 슬롯에 할당된 무기 데이터가 없으면 함수를 즉시 종료
        if (_currentWeaponData == null) return;

        // InventoryManager에서 현재 UI 슬롯이 나타내는 무기를 가져옴
        PlayerWeapon weaponToEquip = InventoryManager.Instance.GetPlayerWeapon(_currentWeaponData);
        // 해당 무기를 인벤토리에서 찾을 수 없으면 함수를 즉시 종료
        if (weaponToEquip == null) return;

        // EquipmentManager의 EquipWeapon 메서드를 호출하여 해당 무기를 플레이어에게 장착
        EquipmentManager.Instance.EquipWeapon(weaponToEquip);
    }

    private void UpdateWeaponSlotUI() // 무기슬롯 UI 업데이트
    {
        weaponImage.sprite = _currentWeaponData.weaponSprite; // 해당 무기 이미지 할당

        // 이 무기를 장착 중인가?
        // 현재 무기 장착 중이고 무기 데이터와 UI에 표시된 데이터가 같으면 참
        bool isWeaponEquipped = (EquipmentManager.Instance.EquippedWeapon != null &&
                                 EquipmentManager.Instance.EquippedWeapon.weaponData == _currentWeaponData);

        if (_hasWeapon) // 무기를 소유하고 있는 경우
        {
            PlayerWeapon weaponToUpgrade = InventoryManager.Instance.GetPlayerWeapon(_currentWeaponData);
            int upgradeCost = _currentWeaponData.GetUpgradeCost(weaponToUpgrade.currentLevel); // 현재 무기의 강화 비용을 가져옴

            weaponImage.color = Color.white; // 구매하면 원래 이미지 색상으로 설정
            purchaseButton.SetActive(false); // 구매버튼 비활성화
            equipButton.SetActive(!isWeaponEquipped); // 장착 중이지 않으면 장착 버튼 활성화
            upgradeButton.SetActive(_currentWeaponLevel < _currentWeaponData.maxLevel); // 만렙 전에만 활성화

            if (isWeaponEquipped) // 장착 중이면
            {
                equippedWeaponText.SetActive(true); // "장착 중" 텍스트 활성화
            }
            else // 아니면
            {
                equippedWeaponText.SetActive(false); // "장착 중 "텍스트 비활성화
            }

            // 소유한 무기는 현재 레벨 기준으로 정보 표시
            weaponNameText.text = $"{_currentWeaponData.weaponName} Lv.{_currentWeaponLevel}";
            weaponDamageText.text =
                $"공격력: {_currentWeaponData.GetDamage(_currentWeaponLevel)}"; // 장착 중인 아이템의 공격력을 불러옴
            weaponCritChanceText.text =
                $"크리티컬 확률: {_currentWeaponData.GetCritChance(_currentWeaponLevel)}%"; // 장착 중인 아이템의 치명타 확률을 불러옴

            // 강화 비용이 충분하면 검은색 텍스트로, 부족하면 빨간색 텍스트로 출력
            weaponUpgradeCostText.color = PlayerData.Instance.Gold >= upgradeCost
                ? Color.black
                : Color.red;
            
            weaponUpgradeCostText.text =
                _currentWeaponData.GetUpgradeCost(_currentWeaponLevel).ToString(); // 업그레이드 가격 표시
        }
        else // 무기를 소유하고 있지 않은 경우
        {
            weaponImage.color = Color.black; // 없으면 검은색 실루엣처럼 보이게 함
            purchaseButton.SetActive(true); // 구매버튼 활성화
            equipButton.SetActive(false); // 장착버튼 비활성화
            upgradeButton.SetActive(false); // 업그레이드 버튼 비활성화

            // 구매 전에는 WeaponData의 기본 데이터 표시
            weaponNameText.text = "???"; // 이름 숨김
            weaponDamageText.text = $"공격력: {_currentWeaponData.GetDamage(_currentWeaponData.weaponLevel)}";
            weaponCritChanceText.text =
                $"치명타 확률: {_currentWeaponData.GetCritChance(_currentWeaponData.weaponLevel)}%";
            weaponCostText.text = _currentWeaponData.purchaseCost.ToString(); // 구매 가격 표시
        }
    }
}