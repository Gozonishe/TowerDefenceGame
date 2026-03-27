using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTarget;
    FindHome currentTargetCode;
    public GameObject core;
    public GameObject gun;
    public TurretDetails turretDetails;
    Quaternion coreStartRotation;
    Quaternion gunStartRotation;
    bool cooldown = true;
    public AudioSource firingSound;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("goob") && currentTarget == null)
        {
            currentTarget = col.gameObject;
            currentTargetCode = currentTarget.GetComponent<FindHome>();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject == currentTarget)
            currentTarget = null;
    }

    void Start()
    {
        coreStartRotation = core.transform.rotation;
        gunStartRotation = gun.transform.localRotation;
    }

    void CoolDown()
    {
        cooldown = true;
    }

    void ShootTarget()
    {
        if (currentTarget && cooldown)
            currentTargetCode.Hit((int)turretDetails.damage);
            firingSound.Play();
            cooldown = false;
            Invoke("CoolDown", turretDetails.reloadTime);
    }

    void Update()
    {
        if(currentTarget != null)
        {
            Vector3 aimAt = new Vector3(currentTarget.transform.position.x,
                                        core.transform.position.y,
                                        currentTarget.transform.position.z);

            float distToTarget = Vector3.Distance(aimAt, gun.transform.position);
            Vector3 relativeTargetPosition = gun.transform.position +
                                        (gun.transform.forward * distToTarget);
            relativeTargetPosition = new Vector3(relativeTargetPosition.x,
                                currentTarget.transform.position.y,
                                relativeTargetPosition.z);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation,
                                                    Quaternion.LookRotation(relativeTargetPosition - gun.transform.position),
                                                    Time.deltaTime * turretDetails.turnSpeed);

            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                                Quaternion.LookRotation(aimAt - core.transform.position),
                                Time.deltaTime * turretDetails.turnSpeed);

            Vector3 directionToTarget = currentTarget.transform.position - gun.transform.position;

            if(Vector3.Angle(directionToTarget, gun.transform.forward) < 10) // 10 is the accuracy
                if(Random.Range(0, 100) < turretDetails.accuracy)
                    ShootTarget();
        }
        else
        {
            gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation,
                                                    gunStartRotation,Time.deltaTime * turretDetails.turnSpeed);
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                                                    coreStartRotation,Time.deltaTime * turretDetails.turnSpeed);
        }
    }
}
