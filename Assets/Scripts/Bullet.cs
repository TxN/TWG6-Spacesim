using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float selfDestructTime = 5f;
   // public float safetyDelay = 0.1f;
    public float damageRadius = 1f;
    public int damage = 30;

    public GameObject explosion;

    void Start()
    {
        Invoke("SelfDestruct", selfDestructTime);
    }

    void SelfDestruct()
    {
        explosion.transform.parent = null;
        explosion.SetActive(true);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {

            foreach (Collider c in Physics.OverlapSphere(transform.position, damageRadius))
            {
                if (c.GetComponent<Rigidbody>())
                {
                    c.GetComponent<Rigidbody>().AddExplosionForce(3, this.transform.position, damageRadius);
                    c.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
                }

                if (c.gameObject.GetComponent<IDamageReciever>() != null)
                {
                    c.gameObject.GetComponent<IDamageReciever>().DoDamage(damage);
                }
            }

            CancelInvoke("SelfDestruct");
            SelfDestruct();

    }
}
