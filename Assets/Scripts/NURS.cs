using UnityEngine;
using System.Collections;

public class NURS : MonoBehaviour {
    public float selfDestructTime = 5f;
    public float safetyDelay = 0.1f;
    public float damageRadius = 6f;
    public int damage = 100;
    public Collider col;
    bool armed = false;
    public Transform smoke;
    public GameObject explosion;
	
	void Start () 
    {
        Invoke("SelfDestruct", selfDestructTime);
        Invoke("Arm", safetyDelay);
	}

    void SelfDestruct()
    {
        smoke.parent = null;
        smoke.gameObject.AddComponent<Autodestruction>().Set(5f);
        smoke.gameObject.GetComponent<ParticleSystem>().enableEmission = false;

        explosion.transform.parent = null;
        explosion.SetActive(true);

        Destroy(this.gameObject);
    }

    void Arm()
    {
        armed = true;
        col.enabled = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (armed)
        {
            
            foreach (Collider c in Physics.OverlapSphere(transform.position, damageRadius))
            {
                if (c.GetComponent<Rigidbody>())
                {
                    c.GetComponent<Rigidbody>().AddExplosionForce(20, this.transform.position, damageRadius);
                    c.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f)));
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


}
