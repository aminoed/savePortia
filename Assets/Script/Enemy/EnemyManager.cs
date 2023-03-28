using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool spawnEnemy = true;
    [SerializeField] private GameObject[] enemyPrefabs;
    //每一波敌人生成间隔时间
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeBetweenWaves = 1f;
    //等待生成间隔时间
    [SerializeField] private int minNum = 4;
    [SerializeField] private int maxNum = 10;
    [SerializeField] private GameObject keyPrefab;
    private Vector2 keyPoint;
    public int waveNum = 1;
    //每一波敌人数量
    int enemyNum;
    WaitForSeconds waitTimeBetweenWaves;
    WaitForSeconds waitTimeBetweemSpawns;
    WaitUntil waitUntilNoEnemy;
    public List<GameObject> enemyList;
    private Vector2 genPoint;
    void Awake()
    {
        enemyList = new List<GameObject>();
        waitTimeBetweemSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(NoEnemy);
    }

    bool NoEnemy()
    {
        return enemyList.Count == 0;
    }

    IEnumerator Start()
    {
        keyPoint = gameObject.transform.position;
        while (spawnEnemy)
        {
            yield return waitUntilNoEnemy;
            yield return waitTimeBetweenWaves;
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyNum = Mathf.Clamp(enemyNum, minNum + waveNum / 3, maxNum);
        for (int i = 0; i < enemyNum; i++)
        {
            genPoint = new Vector2(gameObject.transform.position.x + Random.Range(-2.85f, 1.85f), gameObject.transform.position.y + Random.Range(-1.81f, 2.37f));
            GameObject ene=ObjectPool.Instance.GetObject(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            ene.transform.position = genPoint;
            enemyList.Add(ene);
            yield return waitTimeBetweemSpawns;
        }
        waveNum++;

        if (waveNum >= 4)
        {
            spawnEnemy = false;
        }
    }
    public void RemoveFromList(GameObject enemy)
    {
        if (enemyList.Count >= 0)
        {
            enemyList.Remove(enemy);
            // Debug.Log("移除了" + (i++) + "个敌人");
            if (waveNum == 4 && enemyList.Count == 0)
            {
                GameObject key = ObjectPool.Instance.GetObject(keyPrefab);
                key.transform.position = keyPoint;
                Debug.Log(key.transform.position);
            }
        }
    }
}
