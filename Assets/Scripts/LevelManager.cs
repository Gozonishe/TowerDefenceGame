using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameObject[] spawnPoints;
    static int totalEnemies = 0;
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawn");
        foreach (GameObject sp in spawnPoints)
        {
            totalEnemies += sp.GetComponent<Spawn>().maxCount;
        }
    }

    public static void RemoveEnemy()
    {
        totalEnemies--;
        if (totalEnemies <= 0)
            Debug.Log("Level Over");
    }

    void Update()
    {
        
    }
}
