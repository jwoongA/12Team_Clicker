using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp;
    public int HP => hp;
    private int maxHp;
    StageManager stageManager;
    public Image hpFillImage;

    public int gold;
    public int point;

    private bool isDead = false;

    public Animator animator;

    private Coroutine hpBarRoutine;

    private SpriteRenderer spriteRenderer;
    private EnemySO currentSO;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(EnemySO data, StageManager _stageManager, Image assignedHpBar)
    {
        currentSO = data;
        maxHp = data.baseHp;
        hp = maxHp;
        hpFillImage = assignedHpBar;

        gold = data.baseGold;
        point = data.basePoint;
        GetComponent<SpriteRenderer>().sprite = data.enemySprite;
        stageManager = _stageManager;
        UpDateHpBar();

        spriteRenderer.sprite = currentSO.enemySprite;
    }

    // 왼쪽 마우스 클릭시
    private void OnMouseDown()
    {
        GameObject WeaponChangeUpgrade = GameObject.Find("WeaponChangeUpgrade");
        if (WeaponChangeUpgrade != null && WeaponChangeUpgrade.activeSelf) return;
            if (isDead) return;
        PlayerAttack atk = FindObjectOfType<PlayerAttack>();
        atk.OnEnemyClicked(this);
    }

    // 피격 데미지 처리
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        int prevHp = hp;
        hp -= dmg;
        hp = Mathf.Max(0, hp);

        if (hpBarRoutine != null)
        {
            StopCoroutine(hpBarRoutine);
        }

        hpBarRoutine = StartCoroutine(UpdateHpBarSmoothly(prevHp, hp));
        SoundManager.Instance.PlaySound2D("SoundOptionBlocked");
        if (hp > 0)
        {
            HitEffect();
        }
        else if (hp <= 0)
        {
            isDead = true;
            stageManager.OnEnemyDead();
            DieEffect();
        }
    }

    // 죽는 애니메이션, 보상 지급
    public void DieEffect()
    {
        GameManager.Instance.EnemyDefeated(gold, point);

        if (animator != null)
        {
            animator.SetTrigger("Die");
            SoundManager.Instance.PlaySound2D("DM-CGS-07");
        }

        Destroy(gameObject, 1f);
    }

    // 피격 애니메이션
    public void HitEffect()
    {
        if (currentSO.hitSprite != null)
        {
            spriteRenderer.sprite = currentSO.hitSprite;
            StartCoroutine(BackToIdleSprite(0.12f));
        }

        if (animator != null)
            animator.SetTrigger("Hit");
            
    }

    // 체력바 초기화 용도
    private void UpDateHpBar()
    {
        if (hpFillImage != null)
            hpFillImage.fillAmount = Mathf.Clamp01((float)hp / maxHp);
    }

    // 코루틴을 이용한 체력 바 감소
    IEnumerator UpdateHpBarSmoothly(int fromHp, int toHp)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        float startFill = (float)fromHp / maxHp;
        float endFill = (float)toHp / maxHp;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float fill = Mathf.Lerp(startFill, endFill, t);

            if (hpFillImage != null)
            {
                hpFillImage.fillAmount = fill;
            }

            yield return null;
        }

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = endFill;
        }
    }

    IEnumerator BackToIdleSprite(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isDead && spriteRenderer != null && currentSO != null)
        {
            spriteRenderer.sprite = currentSO.enemySprite;
        }
    }
}
