using UnityEngine;
using UnityEngine.AI;

public class FindHome : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent ai;
    public EnemyDetails enemyDetails;
    int currentHealth;
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.SetDestination(destination.position);
        ai.speed = enemyDetails.speed;
        currentHealth = enemyDetails.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            LevelManager.RemoveEnemy();
            ai.ResetPath();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
