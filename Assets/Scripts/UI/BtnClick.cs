using System;
using System.Collections;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    public static event Action OnNewGameButtonClick;

    public GameObject CloseScene, OpenScene; //화면전환
    public GameObject StartScene, GameScene; //씬
    public GameObject WeaponChangeUpgrade; //무기 장착 밑 강화칸
    public GameObject NotEnoughGold, NotEnoughPoint; //재화 부족알림



    public void WeaponChangeUpgradeBtnClick() //무기 변경/강화창 열기 버튼
    {
        WeaponChangeUpgrade.SetActive(true);
        SoundManager.Instance.PlaySound2D("SoundOptionBlocked");
    }

    public void WeaponChangeUpgradeCloseBtnClick() //무기 변경/강화창 닫기 버튼
    {
        WeaponChangeUpgrade.SetActive(false);
        SoundManager.Instance.PlaySound2D("SoundOptionBlocked");
    }
    

    public void NewGameBtnBtnClick() // 새로 하기 버튼
    {
        StartCoroutine(SceneChange()); //화면 전환
        SoundManager.Instance.PlaySound2D("SoundPause");
    }


    public void ShowNotEnoughPointPopup()
    {
        StartCoroutine(ShowNotEnoughPoint());
    }

    public void ShowNotEnoughGoldPopup()
    {
        StartCoroutine(ShowNotEnoughGold());
    }

    private IEnumerator SceneChange()
    {
        CloseScene.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartScene.SetActive(false);
        GameScene.SetActive(true);
        OpenScene.SetActive(true);
        CloseScene.SetActive(false);
        yield return new WaitForSeconds(1f);
        OpenScene.SetActive(false);

        OnNewGameButtonClick?.Invoke(); //이벤트
    }

    private IEnumerator ShowNotEnoughGold() //골드 부족 알림
    {
        if (!NotEnoughGold.activeSelf)
        {
            NotEnoughGold.SetActive(true);
            yield return new WaitForSeconds(1f);
            NotEnoughGold.SetActive(false);
        }
    }


    private IEnumerator ShowNotEnoughPoint() //포인트 부족 알림
    {
        if (!NotEnoughPoint.activeSelf)
        {
            NotEnoughPoint.SetActive(true);
            yield return new WaitForSeconds(1f);
            NotEnoughPoint.SetActive(false);
        }
    }
}