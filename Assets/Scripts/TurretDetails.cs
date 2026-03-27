using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "ScriptableObjects/TurretDetails", order = 1)]
public class TurretDetails : ScriptableObject
{
    public float damage;
    public float accuracy;
    public float turnSpeed;
    public float reloadTime;
}