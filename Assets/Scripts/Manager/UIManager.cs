using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI pointText;

    [Header("장착 무기 UI")][SerializeField] private Image equippedWeaponImage; // 무기 이미지
    [SerializeField] private TextMeshProUGUI equippedLevelText; // 무기 레벨
    [SerializeField] private TextMeshProUGUI equippedDamageText; // 무기 공격력
    [SerializeField] private TextMeshProUGUI equippedCritChanceText; // 무기 치명타 확률

    void Start()
    {
        UpdateGoldText();
        PlayerData.Instance.OnPlayerStatChanged += UpdateGoldText;

        UpdateGameSceneWeaponUI();
        EquipmentManager.OnEquipmentChanged += UpdateGameSceneWeaponUI;
    }

    void OnDestroy()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.OnPlayerStatChanged -= UpdateGoldText;
        }

        if (EquipmentManager.Instance != null)
        {
            EquipmentManager.OnEquipmentChanged -= UpdateGameSceneWeaponUI;
        }
    }

    void UpdateGoldText()
    {
        goldText.text = PlayerData.Instance.Gold.ToString();
        pointText.text = PlayerData.Instance.Point.ToString();
    }

    void UpdateGameSceneWeaponUI() // 게임화면 장착된 무기슬롯 UI 업데이트
    {
        // 현재 장착 중인 무기
        PlayerWeapon currentlyEquippedWeapon = EquipmentManager.Instance.EquippedWeapon;

        if (currentlyEquippedWeapon != null) // 현재 무기를 장착 중이면
        {
            equippedWeaponImage.sprite = currentlyEquippedWeapon.weaponData.weaponSprite;
            equippedLevelText.text =
                $"{currentlyEquippedWeapon.weaponData.weaponName} Lv.{currentlyEquippedWeapon.currentLevel.ToString()}";
            equippedDamageText.text = $"공격력 : {currentlyEquippedWeapon.GetCurrentDamage().ToString()}";
            equippedCritChanceText.text = $"치명타 확률 : {currentlyEquippedWeapon.GetCurrentCritChance().ToString()}%";
        }
    }
}