using UnityEngine;
using System.Collections;

public class TorpedoLauncher : MonoBehaviour,IWeapon 
{
    public string name = "TorpedoLauncher";
    public string title = "Аннигиляционная торпеда";
    public string desc = "Тяжелая торпеда, созданная в полевых условиях из уцелевшего оборудования со сбитых кораблей кубоидов. Слабее аналога, стоящего на воружении империи, но достаточно мощная, чтобы уничтожить средних размеров город.";
    public int num = 1;
    public int maxNum = 1;
    public GameObject rocketPrefab;
    public float velocity = 15f;
    public float cooldownTime = 0.5f;
    public Vector3 jointOffset = Vector3.zero;
    GameObject parent;
    public AudioClip launchSound;
    public AudioClip noAmmo;
    public int price = 6000;
    public Vector3 GUIRotation = new Vector3(0, 45, 0);
    public Vector3 GUIScale = new Vector3(2, 2, 2);

    float lashShootT = 0;
    float delay = 2;


    void Start()
    {
        parent = transform.parent.gameObject;
    }


    public void Shoot(Transform target)
    {
        if (Time.time > lashShootT + delay)
        {
            lashShootT = Time.time;

            if (num > 0)
            {
                GameObject proj = Instantiate(rocketPrefab) as GameObject;
                if (target != null)
                {
                    proj.GetComponent<GuidedRocket>().target = target.gameObject;
                }

                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                Rigidbody body = proj.GetComponent<Rigidbody>();
                body.velocity = parent.GetComponent<Rigidbody>().velocity +transform.TransformDirection(Vector3.up) * velocity;
                GetComponent<AudioSource>().PlayOneShot(launchSound);
                GetComponent<MeshRenderer>().enabled = false;

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
          
        
    }

    public int GetWeaponPrice()
    {
        return price;
    }

    public void Refill()
    {
        GetComponent<MeshRenderer>().enabled = true;
        num = maxNum;
    }

    public int GetAmmoNum()
    {
        return num;
    }

    public void SetAmmoNum(int n)
    {
        n = Mathf.Clamp(n, 0, maxNum);
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
