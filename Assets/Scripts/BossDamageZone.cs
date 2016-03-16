using UnityEngine;
using System.Collections;

public class BossDamageZone : MonoBehaviour,ISelectableTarget,IDamageReciever
{
    public string name = "Двигатель мазершипа";
    public int health = 9000;
    int maxHealth;

    void Start()
    {
        maxHealth = health;
    }


    public void Destruct()
    {
        Destroy(this.gameObject);
    }

    public void DoDamage(int amount)
    {
        transform.parent.GetComponent<BossHealth>().AddDamage(amount);
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
