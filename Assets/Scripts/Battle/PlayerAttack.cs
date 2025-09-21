using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;

    public GameObject damagePopupPrefab;
    public Canvas uiCanvas;
    public CritPopup critPopup;

    public void OnEnemyClicked(Enemy enemy)
    {
        if (enemy.HP <= 0) return;

        var stats = GameManager.Instance.CurrentStats;
        int baseDamage = stats.Attack;
        float critChance = stats.CritChance;
        float critAttack = stats.CritAttack;

        bool isCritical = Random.value < critChance;
        int damage = isCritical ? (int)critAttack : baseDamage;

        Vector3 worldPos = enemy.transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-0.3f, 0.3f), 0, 0);
        ShowDamagePopup(damage, worldPos, isCritical);

        critPopup.Setup(isCritical, enemy.transform.position);//크리 담당

        enemy.TakeDamage(damage);
    }

    public void ShowDamagePopup(int amount, Vector3 worldPos, bool isCritical)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
    
        var popupObj = PoolManager.Instance.GetObject(damagePopupPrefab, screenPos, Quaternion.identity);
        popupObj.transform.SetParent(uiCanvas.transform, false);
        popupObj.transform.position = screenPos; // UI 위치
        
        popupObj.GetComponent<DamagePopup>().Setup(amount, isCritical);
        // popupObj.SetActive(true);
    }
}