using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float waveInterval = 3f;
    [SerializeField] private int enemiesPerWave = 5;

    [Header("Spawn Zone Settings")]
    [SerializeField] private float spawnOffsetY = 1.5f;
    [SerializeField] private float spawnZonePadding = 0.5f;
    [SerializeField] private float enemySpacing = 1.5f;

    [Header("Enemy Distribution")]
    [SerializeField] private int singleShotCount = 2;
    [SerializeField] private int tripleShotCount = 2;
    [SerializeField] private int quintupleShotCount = 1;

    [Header("Timing Randomization")]
    [SerializeField] private float minSpawnDelay = 0.1f;
    [SerializeField] private float maxSpawnDelay = 0.5f;

    private Camera mainCamera;
    private bool isSpawning = false;
    private float leftBound, rightBound, baseSpawnY;

    private List<float> availableXPositions; 

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
        PreGeneratePositions();
        StartSpawning();
    }

    private void CalculateScreenBounds()
    {
        float zDistance = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        leftBound = bottomLeft.x + spawnZonePadding;
        rightBound = topRight.x - spawnZonePadding;
        baseSpawnY = topRight.y + spawnOffsetY;
    }

    private void PreGeneratePositions()
    {
        availableXPositions = new List<float>();
        float width = rightBound - leftBound;
        int maxSlots = Mathf.FloorToInt(width / enemySpacing);

        for (int i = 0; i <= maxSlots; i++)
        {
            float x = leftBound + i * enemySpacing;
            availableXPositions.Add(x);
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        StartCoroutine(SpawnWaves());
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;

        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnWaves()
    {
        while (isSpawning)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private IEnumerator SpawnWave()
    {
        List<float> spawnPositionsX = new List<float>(availableXPositions);
        ShuffleList(spawnPositionsX);
        List<EnemyType> enemyTypes = GenerateEnemyTypes();
        int spawnCount = Mathf.Min(enemyTypes.Count, spawnPositionsX.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = new Vector3(spawnPositionsX[i], baseSpawnY, 0);
            EnemyController enemy = EnemyFactory.CreateEnemy(enemyTypes[i], pos);
            enemy.Initialize();

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    private List<EnemyType> GenerateEnemyTypes()
    {
        List<EnemyType> types = new();

        for (int i = 0; i < singleShotCount; i++) types.Add(EnemyType.SingleShot);
        for (int i = 0; i < tripleShotCount; i++) types.Add(EnemyType.TripleShot);
        for (int i = 0; i < quintupleShotCount; i++) types.Add(EnemyType.QuintupleShot);

        ShuffleList(types);
        return types;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }
}
