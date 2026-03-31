using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInvironment : MonoBehaviour
{
    public Shoot rocketTurret;
    public Shoot gatlingTurret;
    public Shoot flamerTurret;
    public GameObject turretMenu;
    public TMPro.TMP_Text waveText;
    public TMPro.TMP_Text moneyText;
    public TMPro.TMP_Text livesText;
    public Button slowSpeed;
    public Button mediumSpeed;
    public Button fastSpeed;

    GameObject itemPrefab;
    GameObject focusObj;
    public AudioSource wrongSound;
    void Start()
    {
        slowSpeed.onClick.AddListener(SlowSpeedClicked);
        mediumSpeed.onClick.AddListener(MediumSpeedClicked);
        fastSpeed.onClick.AddListener(FastSpeedClicked);
    }

    void SlowSpeedClicked()
    {
        LevelManager.OnSpeedChange(1);
    }
    void MediumSpeedClicked()
    {
        LevelManager.OnSpeedChange(5);
    }
    void FastSpeedClicked()
    {
        LevelManager.OnSpeedChange(10);
    }

    public void CreateRocket()
    {
        if(LevelManager.totalMoney >= rocketTurret.turretDetails.cost)
        {
            itemPrefab = rocketTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= rocketTurret.turretDetails.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateGatling()
    {
        if (LevelManager.totalMoney >= gatlingTurret.turretDetails.cost)
        {
            itemPrefab = gatlingTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= gatlingTurret.turretDetails.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }
    public void CreateFlamer()
    {
        if (LevelManager.totalMoney >= flamerTurret.turretDetails.cost)
        {
            itemPrefab = flamerTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= flamerTurret.turretDetails.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }
    void CreateItemForButton()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit))
            return;

        focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
        focusObj.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        if(LevelManager.wavesEmitted < LevelManager.numberOfWaves)
            waveText.text = (LevelManager.wavesEmitted + 1) + " of " + LevelManager.numberOfWaves;

        moneyText.text = "$" + LevelManager.totalMoney;

        if(LevelManager.totalLives >= 0)
            livesText.text = "" + LevelManager.totalLives;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) &&
                hit.collider.gameObject.CompareTag("turret"))
            {
                turretMenu.transform.position = Input.mousePosition;
                turretMenu.SetActive(true);
            }
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1<< LayerMask.NameToLayer("Turret"))))
            {
                focusObj.transform.position = hit.point;
            }
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Turret"))) && 
                hit.collider.gameObject.CompareTag("platform") &&
                hit.normal.Equals(new Vector3(0,1,0)))
            {
                hit.collider.gameObject.tag = "occupied";
                focusObj.transform.position = new Vector3(hit.collider.gameObject.transform.position.x,
                                                            focusObj.transform.position.y,
                                                            hit.collider.gameObject.transform.position.z);
                focusObj.GetComponent<BoxCollider>().enabled = true;
                focusObj.GetComponent<SphereCollider>().enabled = true;
            }
            else
            {
                LevelManager.totalMoney += focusObj.GetComponent<TurretDetails>().cost;
                Destroy(focusObj);
            }

            focusObj = null;
        }
    }
}
