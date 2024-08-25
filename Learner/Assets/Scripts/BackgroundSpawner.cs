using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    public int MaxSimultaneousObjects;
    public BackgroundObject ObjectPrefab;
    public float SpawnVelocity;
    public float MaxSpawnAngularVelocity;
    public float SpawnFrequency;
    public float MinimumSpawnSize;
    public float MaximumSpawnSize;
    private void Start()
    {
        InvokeRepeating(nameof(SpawnCycle), 0, SpawnFrequency);
    }

    void SpawnCycle()
    {
        if (transform.childCount < MaxSimultaneousObjects)
        {
            BackgroundObject spawnedObject = Instantiate(ObjectPrefab, transform);
            spawnedObject.transform.position = GetRandomPosOffScreen();
            spawnedObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
            float spawnSize = Random.Range(MinimumSpawnSize, MaximumSpawnSize);
            spawnedObject.transform.localScale = Vector3.one * spawnSize;
            spawnedObject.rb.velocity = Mathf.Min(1 - (spawnSize - MinimumSpawnSize) / (MaximumSpawnSize - MinimumSpawnSize), 0.3f) * SpawnVelocity * (Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(-0.9f, 0.9f), Random.Range(-0.9f, 0.9f))) - spawnedObject.transform.position);
            spawnedObject.rb.angularVelocity = Mathf.Pow(Random.value, 2.5f) * MaxSpawnAngularVelocity;
        }
    }
    private Vector3 GetRandomPosOffScreen()
    {
        float x = Random.Range(-0.2f, 0.2f);
        float y = Random.Range(-0.2f, 0.2f);
        if (x >= 0) x += 1;
        if (y >= 0) y += 1;
        Vector3 randomPoint = new(x, y);

        randomPoint.z = 10f;
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomPoint);

        return worldPoint;
    }
}
