using UnityEngine;
using System.Collections;

public class Machinegun : MonoBehaviour, IWeapon
{
    public string name = "Machinegun";
    public string title = "Твердотельная пушка";
    public string desc = "Проверенное временем оружие, стреляющее осколочно-фугасными снарядами. Малая скорость снарядов компенсируется их сильным фугасным воздействием.";
    public int price = 500;
    public int energyToShot = 30;
    public int num = 0;
    public int maxNum = 0;
    public GameObject bullet;
    public GameObject shootDummy;
    public Vector3 spawnOffset;
    public Vector3 jointOffset = Vector3.zero;
    public GameObject effDummy;
    public GameObject effDummy2;
    public float velocity = 70f;
    GameObject parent;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(1, 1, 1);

    public AudioSource shootSound;
    public AudioSource endShoot;

    bool shooting = false;
    public float cooldownTime = 0.1f;
    float lastShootT = 0;

    

    void Start()
    {
        parent = transform.parent.gameObject;
    }


    void Update()
    {
        PlayerStats stats = PlayerStats.instance;
        if (shooting)
        {
            effDummy.SetActive(true);
            effDummy2.SetActive(true);
            shooting = false;
        }
        else
        {

            if (shootSound.isPlaying)
            {
                effDummy.SetActive(false);
                effDummy2.SetActive(false);
                shootSound.Stop();
                endShoot.Play();
            }

        }
    }

    public void Shoot(Transform target)
    {
        PlayerStats stats = PlayerStats.instance;

        if (stats.energy > energyToShot)
        {
            if (Time.time > (lastShootT + cooldownTime))
            {
                stats.energy -= energyToShot;
                lastShootT = Time.time;
                GameObject proj = Instantiate(bullet) as GameObject;
                proj.transform.position = shootDummy.transform.position + spawnOffset;
                Vector3 endPos = parent.transform.position + transform.TransformDirection(Vector3.right) * 100;
                proj.transform.rotation = Quaternion.LookRotation(endPos - shootDummy.transform.position);
                Rigidbody body = proj.GetComponent<Rigidbody>();
                body.velocity = parent.GetComponent<Rigidbody>().velocity + (endPos - shootDummy.transform.position).normalized * velocity;
            }

            if (!shootSound.isPlaying)
            {
                shootSound.Play();
            }
            shooting = true;
        }

    }


    public void Refill()
    {
        num = maxNum;
    }

    public int GetWeaponPrice()
    {
        return price;
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
