using UnityEngine;
using System.Collections;

public class ExplosionForce : MonoBehaviour
{
    public float damageRadius = 100;
    public float force = 200;


    void Awake()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, damageRadius))
        {
            if (c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddExplosionForce(force + Random.Range(-0.2f*force, 0.2f*force), this.transform.position, damageRadius);
                c.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-force, force), Random.Range(-30f, 30f), Random.Range(-30f, 30f)));
            }

        }
    }

}
