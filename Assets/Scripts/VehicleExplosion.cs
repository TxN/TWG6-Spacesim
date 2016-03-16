using UnityEngine;
using System.Collections;

public class VehicleExplosion : MonoBehaviour {
    public float damageRadius = 6f;
    public GameObject explosion;
    public int damage = 5;

	void Start () 
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
        explosion.transform.parent = null;
        explosion.AddComponent<Autodestruction>().Set(5f);

        gameObject.AddComponent<Autodestruction>().Set(15f);
	}
	

}
