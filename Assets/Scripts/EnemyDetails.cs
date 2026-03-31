using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDetails", order = 1)]
public class EnemyDetails : ScriptableObject
{
    public string cName;
    public float speed;
    public int maxHealth;
    public int reward;
}
