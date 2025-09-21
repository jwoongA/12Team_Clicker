using System.Collections;
using UnityEngine;

public class CritPopup : MonoBehaviour
{
    public GameObject Hit,CritHit;      // 이펙트
    public void Setup(bool isCritical,Vector3 position)
    {
        Quaternion effectRotation = Quaternion.Euler(0, -180f, 0); //방향 조절
        GameObject prefab = isCritical ? CritHit : Hit; //이름 참일경우 CritHit 아닐경우 Hit

        GameObject effect = PoolManager.Instance.GetObject(prefab, position, effectRotation); //이름,좌표,방향

        if (isCritical)SoundManager.Instance.PlaySound2D("Crit"); //크리가 참일떄
        else SoundManager.Instance.PlaySound2D("DM-CGS-47"); //거짓일떄

        StartCoroutine(ReturnEffectToPool(effect, 1f));//1초 뒤에 비활성화
    }

    private IEnumerator ReturnEffectToPool(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.ReturnObject(effect);
    }
}