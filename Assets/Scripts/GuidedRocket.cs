using UnityEngine;
using System.Collections;

public class GuidedRocket : MonoBehaviour 
{
    public float selfDestructTime = 5f;
    public float safetyDelay = 0.5f;
    public float damageRadius = 6f;
    public int damage = 120;
    public float acceleration;
    public float steerCoef = 0.09f;
    float maxSpeed = 10f;
    public Collider col;
    bool armed = false;
    public Transform smoke;
    public GameObject explosion;
    public GameObject target;

    Vector3 estimatedPosition;
    Vector3 targetVelocity;
    Rigidbody targetBody;
    public Vector3 fwd = -Vector3.up;
    public Vector3 fixAngle = new Vector3(-90, 0, 0);

    float startSpeed;
    bool first = true;
    Rigidbody body;

    public LayerMask hitMask;

    void Start()
    {
        Invoke("SelfDestruct", selfDestructTime);
        Invoke("Arm", safetyDelay);
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (first)
        {
            first = false;
            startSpeed = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).magnitude;
            maxSpeed = startSpeed * 2;
            if (target != null)
            {
                targetBody = target.GetComponent<Rigidbody>();
            }
        }

        Vector3 fwdDir = transform.TransformDirection(fwd);

        if (armed)
        {
            startSpeed += Time.deltaTime * acceleration;

            if (target != null)
            {
                Vector3 enemyPos = target.transform.position;
                Vector3 dir = enemyPos - transform.position;

                if (targetBody != null)
                {
                    float distance = dir.magnitude;
                    Vector3 tgVel = targetBody.velocity;
                    estimatedPosition = target.transform.position + (distance / startSpeed) * tgVel; //примерная точка упреждения
                    dir = estimatedPosition - transform.position;
                }
                else
                {

                }

                Quaternion targetRotation = Quaternion.LookRotation(dir) * Quaternion.Euler(fixAngle);
                body.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, steerCoef));

            }
   
        }
      
        body.velocity = Vector3.Lerp(body.velocity,fwdDir * startSpeed , 0.5f);

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
            Debug.Log(col.gameObject.name);
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
