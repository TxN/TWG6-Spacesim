using UnityEngine;
using System.Collections;

public class NPCLaser : MonoBehaviour, IWeapon 
{
    public string name = "Laser";
    public string title = "Гамма-лазер";
    public string desc = "Мощный и компактный лазерный излучатель постоянного действия. Из-за расхождения лазерного пучка на больших расстояниях дальность действия ограничена.";
    public int price = 2999;
    public int damage = 1;
    public int num = 0;
    public int maxNum = 0;
    public GameObject EmissionDummy;
    public GameObject[] EmissionEffect;
    public Vector3 jointOffset = Vector3.zero;
    public AudioSource shootSound;
    public AudioSource endShoot;
    float effectiveDist = 120;
    public GameObject hitmarkFab;
    GameObject hitmark;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(2, 2, 2);

    public LayerMask hitMask;

    bool shooting = false;

    void Start()
    {
        hitmark = Instantiate(hitmarkFab) as GameObject;
        hitmark.SetActive(false);
    }

    void Update()
    {
       // Shoot(null);

        if (shooting)
        {
            foreach (GameObject obj in EmissionEffect)
            {
                obj.SetActive(true);
            }
            
          
            shooting = false;
        }
        else
        {
            foreach (GameObject obj in EmissionEffect)
            {
                obj.SetActive(false);
            }
            
            hitmark.SetActive(false);

            if (shootSound.isPlaying)
            {
                shootSound.Stop();
                endShoot.Play();
            }

        }
    }

    public void Shoot(Transform target)
    {
        if (!shootSound.isPlaying)
        {
            shootSound.Play();
        }

        hitmark.SetActive(false);

        Vector3 startPos = EmissionDummy.transform.position;
        Vector3 endPos = startPos + transform.TransformDirection(new Vector3(0, 0, 1)) * effectiveDist;

        Debug.DrawLine(startPos, endPos, Color.cyan);
        RaycastHit hit;

        if (Physics.Raycast(startPos, transform.TransformDirection(new Vector3(0, 0, 1)), out hit, effectiveDist, hitMask))
        {
                hitmark.transform.position = hit.point;
                hitmark.SetActive(true);
                hitmark.transform.rotation = EmissionEffect[0].transform.rotation;

                SetLineEffects(hit.point);

                if (hit.transform.gameObject.GetComponent<IDamageReciever>() != null)
                {
                    hit.transform.gameObject.GetComponent<IDamageReciever>().DoDamage(damage);
                }

        }
        else
        {
            SetLineEffects(endPos);
        }

        shooting = true;
    }

    void SetLineEffects(Vector3 end)
    {
        foreach (GameObject obj in EmissionEffect)
        {
            LineRenderer line = obj.GetComponent<LineRenderer>();
            line.SetPosition(0, obj.transform.position);
            line.SetPosition(1, end); 
        }
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

    public int GetWeaponPrice()
    {
        return price;
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
