using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;
    static int totalEnemies = 0;

    static int numberOfWaves = 3;
    static int wavesEmitted = 0;
    static bool levelOver = false;
    static bool nextWave = false;

    int timeBetweenWaves = 5;

    public ParticleSystem deathParticlePrefab;
    public static IObjectPool<ParticleSystem> deathParticlePool;
    void Start()
    {
        Time.timeScale = 20;
        GameObject[] spawnP = GameObject.FindGameObjectsWithTag("spawn");
        spawnPoints = new Spawn[spawnP.Length];
        for (int i = 0; i < spawnP.Length; i++)
        {
            spawnPoints[i] = spawnP[i].GetComponent<Spawn>();
            totalEnemies += spawnPoints[i].maxCount;
        }

        deathParticlePool = new ObjectPool<ParticleSystem>(CreateDeathExplosion, OnTakeFromPool, OnReturnedToPool, null, true, 10, 20);
    }

    ParticleSystem CreateDeathExplosion()
    {
        ParticleSystem particleSystem = Instantiate(deathParticlePrefab);
        particleSystem.Stop();
        return particleSystem;
    }

    void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }

    public static void DisplayDeathExplosion(Vector3 position)
    {
        ParticleSystem deathExp = deathParticlePool.Get();
        if (deathExp)
        {
            deathExp.transform.position = position;
            deathExp.Play();
        }
    }

    public static void RemoveEnemy()
    {
        totalEnemies--;
        if (totalEnemies <= 0)
            {
                wavesEmitted++;
                nextWave = true;

                if (wavesEmitted >= numberOfWaves)
                    {
                        Debug.Log("Level Over");
                        levelOver = true;
                        nextWave = false;
                    }
            }
        }

    void ResetSpawners()
    {
        foreach (Spawn sp in spawnPoints)
        {
            totalEnemies += sp.maxCount;
            sp.Restart();
        }
    }

    void Update()
    {
        if (nextWave)
        {
            nextWave = false;
            Invoke("ResetSpawners", timeBetweenWaves);
        }
    }
}
