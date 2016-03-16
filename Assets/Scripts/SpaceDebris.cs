using UnityEngine;
using System.Collections;

public class SpaceDebris : MonoBehaviour, IDamageReciever, ISelectableTarget {
    public string name = "Nothing";
    public int health = 100;
    int maxHealth;
    public bool destructible = true;
    public bool autodestruct = false;
    public float destructTime = 10f;
    public GameObject explosion;

    void Start()
    {
        maxHealth = health;

        if (autodestruct)
        {
            Invoke("Destruct", destructTime);
        }
    }


    public void Destruct()
    {
        Destroy(this.gameObject);
    }

    public void DoDamage(int amount)
    {
        if (destructible)
        {
            health -= amount;
        }
        
        if (health <= 0)
        {
            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.parent = null;
               
            }
            Destroy(this.gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public string GetName()
    {
        return name;
    }
}
