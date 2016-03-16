using UnityEngine;
using System.Collections;

public class GuidedLauncher : MonoBehaviour, IWeapon 
{
    public GameObject[] rockets = new GameObject[9];
    public Transform shootPos;

    public string name = "GuidedLauncher";
    public string title = "Управляемые ракеты Р-66 Шерешень";
    public string desc = "Высокоманевренные ракеты малой дальности, предназначенные для поражения истребителей. Слабое поражающее действие компенсируется высокой точностью и большим количеством ракет в блоке.";
    public int num = 9;
    public int maxNum = 9;
    public GameObject rocketPrefab;
    public float velocity = 25f;
    public float cooldownTime = 0.5f;
    public Vector3 jointOffset = Vector3.zero;
    float lastShootT = 0;
    GameObject parent;
    public AudioClip launchSound;
    public AudioClip noAmmo;
    public int price = 1200;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(2, 2, 2);
  

    void Start()
    {
        parent = transform.parent.gameObject;
    }


    public void Shoot(Transform target)
    {
        if (Time.time > (lastShootT + cooldownTime))
        {
            if (num > 0)
            {
                lastShootT = Time.time;
                GameObject proj =  Instantiate(rocketPrefab) as GameObject;
                if (target != null)
                {
                    proj.GetComponent<GuidedRocket>().target = target.gameObject;
                }
                
                proj.transform.position = shootPos.transform.position;
                proj.transform.rotation = transform.rotation;
                Rigidbody body = proj.GetComponent<Rigidbody>();
                body.velocity = parent.GetComponent<Rigidbody>().velocity + transform.TransformDirection(-Vector3.up) * velocity;
                GetComponent<AudioSource>().PlayOneShot(launchSound);
                rockets[num - 1].SetActive(false);
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

        foreach (GameObject obj in rockets)
        {
            obj.SetActive(true);
        }
    }

    public int GetAmmoNum()
    {
        return num;
    }

    public void SetAmmoNum(int n)
    {
        n = Mathf.Clamp(n, 0, maxNum);
        for (int i = 0; i < rockets.Length; i++)
        {
            if (i < n)
            {
                rockets[i].SetActive(true);
            }
            else
            {
                rockets[i].SetActive(false);
            }
        }
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
