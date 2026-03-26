using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTarget;
    public GameObject core;
    public GameObject gun;
    Quaternion coreStartRotation;
    Quaternion gunStartRotation;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("goob") && currentTarget == null)
        {
            currentTarget = col.gameObject;
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
        gunStartRotation = gun.transform.rotation;
    }

    void Update()
    {
        if(currentTarget != null)
        {
            Vector3 aimAt = new Vector3(currentTarget.transform.position.x,
                                        core.transform.position.y,
                                        currentTarget.transform.position.z);
            //gun.transform.LookAt(currentTarget.transform.position);

            float distToTarget = Vector3.Distance(aimAt, gun.transform.position);
            Vector3 relativeTargetPosition = gun.transform.position +
                                        (gun.transform.forward * distToTarget);
            relativeTargetPosition = new Vector3(relativeTargetPosition.x,
                                currentTarget.transform.position.y,
                                relativeTargetPosition.z);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation,
                                                    Quaternion.LookRotation(relativeTargetPosition - gun.transform.position),
                                                    Time.deltaTime);
            //core.transform.LookAt(aimAt);
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                                Quaternion.LookRotation(aimAt - core.transform.position),
                                Time.deltaTime);
        }
        else
        {
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation,
                                                    gunStartRotation,Time.deltaTime);
            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                                                    coreStartRotation,
                                                    Time.deltaTime);
        }
    }
}
