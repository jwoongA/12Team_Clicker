using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldUpgradeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public float repeatInterval = 0.2f; // 버튼을 누르고 있을 때 업그레이드 반복 간격
    public float holdDelay = 1.5f; // 버튼을 누르고 있는 동안 자동 업그레이드가 시작 될 때 까지의 지연 시간
    public UpgradeType upgradeType; // 어떤 업그레이드를 할 지 구분하는 enum

    private Coroutine delayCoroutine;
    private Coroutine repeatCoroutine;

    private PlayerUpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeManager = FindObjectOfType<PlayerUpgradeManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (delayCoroutine == null)
        {
            delayCoroutine = StartCoroutine(StartAfterDelay());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }

        if (repeatCoroutine != null)
        {
            StopCoroutine(repeatCoroutine);
            repeatCoroutine = null;
        }
    }

    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(holdDelay);
        repeatCoroutine = StartCoroutine(RepeatUpgrade());
    }

    private IEnumerator RepeatUpgrade() // 반복 간격마다 업그레이드가 무한 반복 되도록
    {
        while (true)
        {
            switch (upgradeType) // 업그레이드 타입에 맞게 실행
            {
                case UpgradeType.CritMulitiplier:
                    upgradeManager.UpgradeCriticalMultiplier();
                    break;

                case UpgradeType.GoldMulitiplier:
                    upgradeManager.UpgradeGoldMultiplier();
                    break;

                case UpgradeType.AutoAttackSpeed:
                    upgradeManager.UpgradeAutoAttackSpeed();
                    break;
            }

            yield return new WaitForSeconds(repeatInterval); // 다음 반복까지 대기
        }
    }
}
