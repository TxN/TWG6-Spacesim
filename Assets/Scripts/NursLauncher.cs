using UnityEngine;
using System.Collections;


public class NursLauncher : MonoBehaviour,IWeapon {
    public string name = "NursLauncher";
    public string title = "Неуправляемые ракеты С-18";
    public string desc = "Блок из восемнадцати легких неуправляемых ракет с бесконтактным взрывателем. За счет полного отсутствия систем управления, эти небольшие ракеты наносят урон, сравнимый с более тяжелыми ракетами Шершень.";
    public int num = 18;
    public int maxNum = 18;
    public GameObject nursPrefab;
    public Vector3 spawnOffset = Vector3.zero;
    public float velocity = 50f;
    public float cooldownTime = 0.3f;
    public Vector3 jointOffset = Vector3.zero;
    float lastShootT = 0;
    GameObject parent;
    public AudioClip launchSound;
    public AudioClip noAmmo;
    public int price = 550;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(2, 2, 2);
	
	void Start ()
    {
        parent = transform.parent.gameObject;
	}
	
	
	void Update () 
    {
       // Shoot(null);
	}

    public void Shoot(Transform target)
    {
        if (Time.time > (lastShootT + cooldownTime))
        {
            if (num > 0)
            {
                lastShootT = Time.time;
                GameObject proj = Instantiate(nursPrefab) as GameObject;
                proj.transform.position = transform.position + spawnOffset;
                proj.transform.rotation = transform.rotation;
                Rigidbody body = proj.GetComponent<Rigidbody>();
                body.velocity = parent.GetComponent<Rigidbody>().velocity + transform.TransformDirection(-Vector3.up) * velocity;
                GetComponent<AudioSource>().PlayOneShot(launchSound);
                num--;
            }
            else
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().clip = noAmmo;
                    GetComponent<AudioSource>().Play();
                }

                return;
            } 
        }
        else
        {
            return;
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
