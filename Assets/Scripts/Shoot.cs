using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTarget;

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
        
    }

    void Update()
    {
        if(currentTarget != null)
            this.transform.LookAt(currentTarget.transform.position);
    }
}
