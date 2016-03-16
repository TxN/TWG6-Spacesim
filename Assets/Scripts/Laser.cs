using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour,IWeapon {

    public string name = "Laser";
    public string title = "Гамма-лазер";
    public string desc = "Мощный и компактный лазерный излучатель постоянного действия. Из-за расхождения лазерного пучка на больших расстояниях дальность действия ограничена.";
    public int price = 1000;
    public int damage = 5;
    public int energyToShot = 10;
    public int num = 0;
    public int maxNum = 0;
    public GameObject EmissionDummy;
    public GameObject EmissionEffect;
    public Vector3 jointOffset = Vector3.zero;
    GameObject parent;
    public AudioSource shootSound;
    public AudioSource endShoot;
    LineRenderer line;
    float effectiveDist = 100;
    public GameObject hitmarkFab;
    GameObject hitmark;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(2, 2, 2);

    public LayerMask hitMask;

    bool shooting = false;

    float lastCDT = 0;
    float cooldownTime = 2f;

    void Start()
    {
        parent = transform.parent.gameObject;
        line = EmissionDummy.GetComponent<LineRenderer>();
        line.useWorldSpace = true;

        hitmark = Instantiate(hitmarkFab) as GameObject;
        hitmark.SetActive(false);
    }

    void Update()
    {
        if (shooting)
        {
            EmissionEffect.SetActive(true);
            line.enabled = true;
            shooting = false;
        }
        else
        {
            EmissionEffect.SetActive(false);
            line.enabled = false;
            hitmark.SetActive(false);
            if (shootSound.isPlaying)
            {
                shootSound.Stop();
                endShoot.Play();
            }
            
        }
    }

    Vector3 lastValidShootPos;

    public void Shoot(Transform target)
    {
        hitmark.SetActive(false);
        PlayerStats stats = PlayerStats.instance;

        if ((stats.energy > energyToShot)&&(Time.time > lastCDT + cooldownTime))
        {
            stats.energy -= energyToShot;
            Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height / 2, Screen.width / 2, 0));
            startPos.z = Camera.main.transform.position.z;
            Vector3 endPos = startPos + Camera.main.transform.TransformDirection(new Vector3(0, 0, 1)) * effectiveDist;

            if (!shootSound.isPlaying)
            {
                shootSound.Play();
            }


            Ray ray = new Ray(startPos, endPos);
            Debug.DrawLine(startPos, endPos, Color.cyan);
            RaycastHit hit;

            if (Physics.Raycast(startPos, Camera.main.transform.TransformDirection(new Vector3(0, 0, 1)), out hit, effectiveDist, hitMask))
            {
                Vector3 hitDir = hit.point - startPos;
                if (Vector3.Cross(Camera.main.transform.TransformDirection(new Vector3(0, 0, 1)), hitDir).magnitude < 3f)
                {
                    hitmark.transform.position = hit.point;
                    hitmark.SetActive(true);
                    hitmark.transform.rotation = EmissionEffect.transform.rotation;

                    line.SetPosition(0, EmissionDummy.transform.position);
                    line.SetPosition(1, hit.point);

                    if (hit.transform.gameObject.GetComponent<IDamageReciever>() != null)
                    {
                        hit.transform.gameObject.GetComponent<IDamageReciever>().DoDamage(damage);
                    }
                }

            }
            else
            {
                line.SetPosition(0, EmissionDummy.transform.position);
                line.SetPosition(1, endPos);
            }

            shooting = true;
        }
        else
        {
            lastCDT = Time.time;
          //  coolDown;
        }

     
    }

    public int GetWeaponPrice()
    {
        return price;
    }

    public void Refill()
    {
        num = maxNum;
    }

    public int GetAmmoNum()
    {
        return num;
    }

    public void SetAmmoNum(int n)
    {
        num = n;
    }

    public Vector3 GetJointOffset()
    {
        return jointOffset;
    }

    public string GetWeaponName()
    {
        return name;
    }

    public string GetWeaponTitle()
    {
        return title;
    }

    public string GetWeaponDesc()
    {
        return desc;
    }

    public Vector3 GetGUIRotation()
    {
        return GUIRotation;
    }

    public Vector3 GetGUIScale()
    {
        return GUIScale;
    }

	
}
