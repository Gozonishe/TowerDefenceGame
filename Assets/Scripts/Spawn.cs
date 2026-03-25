using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform homeLocation;
    public float startDelay = 1f;
    public float spawnRate = 0.3f;
    public int maxCount = 10;
    int enemyCount = 0;
    void Start()
    {
        InvokeRepeating("Spawner", startDelay, spawnRate);
    }

    void Spawner()
    {
        GameObject goob = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        goob.GetComponent<FindHome>().destination = homeLocation;
        enemyCount++;
        if (enemyCount >= maxCount)
            CancelInvoke("Spawner");
    }
}
