using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CandySpawner))]
public class Shelf : MonoBehaviour
{
    [Header("Sight Detection")]
    public Collider sightCollider; // The collider used for visibility raycasts
    public Collider physicsCollider; // The collider used for actual physics
    public LayerMask visionMask; // Includes walls, shelves, etc.

    [Header("Risk Calculation")]
    [Range(0f, 1f)] public float visibilityWeight = 0.6f;
    [Range(0f, 1f)] public float distanceWeight = 0.4f;
    public float maxDistanceScore = 30f; // Max path distance for scoring

    [Header("References")]
    public Transform[] registers; // Cashier positions
    public Transform[] floorClerkSpawns; // Floor clerk spawn points

    private CandySpawner candySpawner;
    private float riskFactor = 0f;

    private void Awake()
    {
        candySpawner = GetComponent<CandySpawner>();
    }

    private void Start()
    {
        RecalculateRisk();
        candySpawner.SetRiskFactor(riskFactor);
    }

    public void RecalculateRisk()
    {
        float visibilityScore = CalculateVisibilityScore();
        float distanceScore = CalculateDistanceScore();

        riskFactor = Mathf.Clamp01(visibilityScore * visibilityWeight + distanceScore * distanceWeight);
    }

    private float CalculateVisibilityScore()
    {
        int totalRays = 0;
        int blockedRays = 0;

        foreach (Transform t in registers)
        {
            totalRays++;
            if (!IsVisibleFrom(t)) blockedRays++;
        }

        foreach (Transform t in floorClerkSpawns)
        {
            totalRays++;
            if (!IsVisibleFrom(t)) blockedRays++;
        }

        float blockedRatio = (float)blockedRays / totalRays;
        return 1f - blockedRatio; // 1 means fully visible, 0 means fully hidden
    }

    private float CalculateDistanceScore()
    {
        float closest = float.MaxValue;
        foreach (Transform t in floorClerkSpawns)
        {
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < closest) closest = dist;
        }

        return Mathf.Clamp01(closest / maxDistanceScore);
    }

    private bool IsVisibleFrom(Transform observer)
    {
        Vector3 origin = observer.position + Vector3.up * 1.5f;
        Vector3 target = sightCollider.bounds.center;

        if (Physics.Linecast(origin, target, out RaycastHit hit, visionMask))
        {
            return hit.collider == sightCollider;
        }

        return false;
    }
}
