using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;
    public static int totalEnemies = 0;

    public static int numberOfWaves = 3;
    public static int wavesEmitted = 0;
    public static int totalMoney = 500;
    public static int totalLives = 10;   
    public static bool levelOver = false;
    public static bool nextWave = false;

    public GameObject gameOverPanel;

    int timeBetweenWaves = 5;

    public ParticleSystem deathParticlePrefab;
    public static IObjectPool<ParticleSystem> deathParticlePool;
    void Start()
    {
        Time.timeScale = 1;
        GameObject[] spawnP = GameObject.FindGameObjectsWithTag("spawn");
        spawnPoints = new Spawn[spawnP.Length];
        for (int i = 0; i < spawnP.Length; i++)
        {
            spawnPoints[i] = spawnP[i].GetComponent<Spawn>();
            totalEnemies += spawnPoints[i].maxCount;
        }

        deathParticlePool = new ObjectPool<ParticleSystem>(CreateDeathExplosion, OnTakeFromPool, OnReturnedToPool, null, true, 10, 20);
    }

    public static void OnSpeedChange(int speed)
    {
        Time.timeScale = speed;
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

    public static void RemoveLife()
    {
        totalLives--;
        if(totalLives <= 0)
        {
            Debug.Log("Level Over");
            levelOver = true;
            nextWave = false;
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

        if(levelOver)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
