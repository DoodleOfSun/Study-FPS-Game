using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletEffect;

    ParticleSystem ps;

    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public int weaponPower = 5;

    Animator anim;

    bool ZoomMode;

    public Text wModeText;

    public GameObject[] eff_Flash;

    public GameObject weapon01;
    public GameObject weapon02;


    public GameObject crosshair01;
    public GameObject crosshair02;


    public GameObject weapon01_Special;
    public GameObject weapon02_Special;

    public GameObject weapon02_SniperZoom;

    public bool sniperMod;

    // Start is called before the first frame update
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();

        sniperMod = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rifle
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon01.SetActive(true);
            crosshair01.SetActive(true);
            weapon02.SetActive(false);
            crosshair02.SetActive(false);
            weapon01_Special.SetActive(true);
            weapon02_Special.SetActive(false);
            sniperMod = false;
        }
        // Sniper
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon01.SetActive(false);
            crosshair01.SetActive(false);
            weapon02.SetActive(true);
            crosshair02.SetActive(true);
            weapon01_Special.SetActive(false);
            weapon02_Special.SetActive(true);
            sniperMod = true;
        }


        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // Fire bomb when Key G Down
        // Also, only in Rifle mod
        if (Input.GetKeyDown(KeyCode.G) && sniperMod == false)
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    print("광선 적에게 히트");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }

                else
                {
                    Debug.Log("Ray hit");
                    bulletEffect.transform.position = hitInfo.point;

                    bulletEffect.transform.forward = hitInfo.normal;

                    ps.Play();
                }
            }
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // Sniper Zoom
        if (sniperMod == true)
        {
            // Zoomed
            if (Input.GetMouseButton(1))
            {
                if (!ZoomMode)
                {
                    Camera.main.fieldOfView = 15f;
                    ZoomMode = true;
                    weapon02_SniperZoom.SetActive(true);
                    wModeText.text = "Weapon Zoomed";
                }

            }

            // Zoom Disabled
            if (Input.GetMouseButtonUp(1))
            {
                if (ZoomMode)
                {
                    Camera.main.fieldOfView = 60f;
                    ZoomMode = false;
                    weapon02_SniperZoom.SetActive(false);
                    wModeText.text = "";
                }
            }
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length);
        eff_Flash[num].SetActive(true);
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false);
    }
}
