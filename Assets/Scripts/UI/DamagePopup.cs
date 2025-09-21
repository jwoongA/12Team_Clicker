using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    private Vector3 moveDirection;
    private float moveSpeed = 200f;
    private float fadeDuration = 0.8f;
    private float timer = 0f;

    public void Setup(int amount, bool isCritical)
    {
        timer = 0f; // 초기화해야 함.
        damageText.text = amount.ToString();
        damageText.color = isCritical ? Color.red : Color.gray;
        // 위로 + 약간 좌/우로 움직이게 랜덤 방향
        moveDirection = (Vector3.up + new Vector3(Random.Range(-1f, 1f), 0, 0)).normalized;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 시간에 따라 알파값 점점 줄이기
        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        var col = damageText.color;
        damageText.color = new Color(col.r, col.g, col.b, alpha);

        if (timer >= fadeDuration)
        {
            PoolManager.Instance.ReturnObject(gameObject);
        }
    }
}
