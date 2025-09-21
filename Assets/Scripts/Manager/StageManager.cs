using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    // 1(챕터) - 1(스테이지/레벨)
    public EnemySO[] enemySO;
    public int stagesPerChapter = 10;

    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public TextMeshProUGUI stageText;
    public TextMeshProUGUI enemyNameText;

    int currentStage = 1;

    void Awake()
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

    public void StartStage(int stageNum)
    {
        // 챕터 인덱스 계산
        int chapterIndex = (stageNum - 1) / stagesPerChapter;
        chapterIndex = Mathf.Clamp(chapterIndex, 0, enemySO.Length - 1);

        EnemySO currentEnemySO = enemySO[chapterIndex];

        stageText = GameObject.FindGameObjectWithTag("StageText").GetComponent<TextMeshProUGUI>();
        enemyNameText = GameObject.FindGameObjectWithTag("EnemyNameText").GetComponent<TextMeshProUGUI>();

        // 챕터에 따른 이름 변화
        stageText.text = $"{chapterIndex + 1} - {((stageNum - 1) % stagesPerChapter) + 1}";
        enemyNameText.text = currentEnemySO.enemyName;

        // 적 생성
        if (enemyPrefab == null)
        {
            return;
        }
        SpawnEnemy(currentEnemySO);
    }

    public void SpawnEnemy(EnemySO enemyData)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();

        // 태그로 이미지 찾기
        Image hpBar = GameObject.FindGameObjectWithTag("EnemyHpBar").GetComponent<Image>();

        if (enemyScript != null)
        {
            enemyScript.Init(enemyData, this, hpBar);
        }
    }

    public void OnEnemyDead()
    {
        currentStage++;
        StartCoroutine(SpawnNextStageWithDelay(0.8f));
    }

    // 적 죽는 애니메이션 실행 후 다시 소환
    IEnumerator SpawnNextStageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartStage(currentStage);
    }
}