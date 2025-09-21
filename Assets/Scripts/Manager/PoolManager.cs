using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; //싱글톤

    //키: prefab 이름, 값: 해당 prefab의 인스턴스를 보관하는 큐
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //풀에서 prefab의 인스턴스를 가져오거나, 없으면 새로 생성.
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string key = prefab.name;
    
        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0) //프리팹 이름으로.
        {
            GameObject obj = poolDictionary[key].Dequeue();

            if (obj == null) return Instantiate(prefab, position, rotation); //참조가 날라간 경우.

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true); 
            // 풀링은 꺼낸 다음에 초기화한 후 활성화(기본)
            
            return obj; // 활성화된 오브젝트.
        }
        else
        {
            GameObject newObj = Instantiate(prefab, position, rotation);
            newObj.name = prefab.name;
            return newObj;
        }
    }


    //사용이 끝난 오브젝트를 풀에 반환. obj: 반환할 오브젝트.
    public void ReturnObject(GameObject obj)
    {
        string key = obj.name;
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        obj.transform.SetParent(this.transform); //이 오브젝트로 생성.

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}

