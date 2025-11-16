using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [Header("Candy Settings")]
    public GameObject candyPrefab;
    public Transform spawnPoint;
    public int maxCandyCount = 10;
    public Vector2 spawnArea = new Vector2(0.5f, 0.3f);

    [Header("Restock Settings")]
    public bool useGlobalRestock = true;
    public float perShelfRestockTime = 20f;
    public static float globalRestockTime = 30f;

    [Header("Risk Scaling")]
    public AnimationCurve batchSizeCurve = AnimationCurve.EaseInOut(0, 2, 1, 8);

    private float riskFactor;
    private List<GameObject> spawnedCandy = new List<GameObject>();
    private bool isRestocking = false;

    private void Start()
    {
        if (!useGlobalRestock)
            StartCoroutine(PerShelfRestock());
    }

    public void SetRiskFactor(float risk)
    {
        riskFactor = Mathf.Clamp01(risk);
    }

    public void TryRestock()
    {
        if (!isRestocking && spawnedCandy.Count < maxCandyCount)
            StartCoroutine(SpawnCandyBatch());
    }

    private IEnumerator PerShelfRestock()
    {
        while (true)
        {
            yield return new WaitForSeconds(perShelfRestockTime);
            TryRestock();
        }
    }

    public static IEnumerator GlobalRestockRoutine(List<CandySpawner> allSpawners)
    {
        while (true)
        {
            yield return new WaitForSeconds(globalRestockTime);
            foreach (var spawner in allSpawners)
                spawner.TryRestock();
        }
    }

    private IEnumerator SpawnCandyBatch()
    {
        isRestocking = true;

        int batchSize = Mathf.RoundToInt(batchSizeCurve.Evaluate(riskFactor));
        int toSpawn = Mathf.Min(batchSize, maxCandyCount - spawnedCandy.Count);

        for (int i = 0; i < toSpawn; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                0f,
                Random.Range(-spawnArea.y, spawnArea.y)
            );

            GameObject candy = Instantiate(candyPrefab, spawnPoint.position + offset, Quaternion.identity);
            spawnedCandy.Add(candy);
        }

        yield return new WaitForSeconds(0.5f);
        isRestocking = false;
    }

    public void NotifyCandyTaken(GameObject candy)
    {
        if (spawnedCandy.Contains(candy))
            spawnedCandy.Remove(candy);

        if (!useGlobalRestock)
            StartCoroutine(RestockAfterDelay(perShelfRestockTime));
    }

    private IEnumerator RestockAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TryRestock();
    }
}
