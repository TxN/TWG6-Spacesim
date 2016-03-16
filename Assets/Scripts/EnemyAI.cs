using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour,IDamageReciever,ISelectableTarget {

    public string name = "Кубоид (тип не опознан)";
    public int health = 100;
    int maxHealth;

    public float maxAngVel = 5f;
    public float maxSpeed = 20f;
    float currentMaxSpeed = 10f;
    public float acceleration = 1f;
    float curSpeed = 0f;
    public AnimationCurve steeringCurve;

    Rigidbody body;

    public GameObject explosion;

    public LayerMask detectMask;
    public LayerMask obstacleMask;
    public Waypoints waypointHolder;

    float lastTime = 0f;
    float deltaTime = 0.1f;

    float targetVelocity = 0f;
    Quaternion targetRotation;

    GameObject enemy;
    bool movingToLastKnown = false;
    bool seeEnemy = false;
    bool strictMode = false;
    Vector3 lastKnownPos;
    public bool useWaypoints = false;
    Vector3 moveTarget;
    bool targetReached = true;
    float targetThreshold = 20f;

    bool dead = false;

    IWeapon gun;

    public float detectDist = 250;
    public float backDetectDist = 45;

    bool enable = true;

    void Start()
    {
        maxHealth = health;
        body = GetComponent<Rigidbody>();
        gun = GetComponent<IWeapon>();

    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = PlayerStats.instance.player;
        }
    }

    void FixedUpdate()
    {
        if (Time.time > lastTime + deltaTime)
        {
            lastTime = Time.time;
            DoAI();
        }
        if (enable)
        {
            Move();
        }
        

    }

    void DoAI()
    {
        Vector3 ourPos = transform.position;
        Vector3 fwdDir = transform.TransformDirection(Vector3.forward);
        if ((enemy != null)&&(!strictMode))
        {
            Vector3 enemyPos = PlayerStats.instance.player.transform.position;
            Vector3 moveDir = enemyPos - ourPos;
            float dirSign = Vector3.Dot(fwdDir, moveDir);
            float distance = moveDir.magnitude;

          //  Debug.DrawLine(ourPos, enemyPos, Color.red);
            if(CheckVisibility(enemy)) //нет ли серьезных причин не видеть врага
            {
                if (dirSign > 0) //враг в передней полусфере
                {
                    targetReached = true;
                    seeEnemy = true;
                    movingToLastKnown = false;
                }
                else if(distance < backDetectDist) //если враг подкрался сзади совсем близко
                {
                    targetReached = true;
                    movingToLastKnown = false;
                    seeEnemy = true;
                }
                else
                {
                    if (seeEnemy == true) //в прошлый раз мы его видели
                    {
                        movingToLastKnown = true;
                        lastKnownPos = enemy.transform.position; // запишем где мы его видели (не совсем так, но 0.1с разницы особо не влияют
                        moveTarget = lastKnownPos;
                    }
                    seeEnemy = false;
                }
            }
            else
            {
                if (seeEnemy == true) //в прошлый раз мы его видели
                {
                    movingToLastKnown = true;
                    lastKnownPos = enemy.transform.position; // запишем где мы его видели (не совсем так, но 0.1с разницы особо не влияют
                    moveTarget = lastKnownPos;
                }
                seeEnemy = false;
            }

            if (seeEnemy)
            {
                float enemySpeed = enemy.GetComponent<Rigidbody>().velocity.magnitude;
                if (distance < 80)
                {
                    currentMaxSpeed = 0.25f* maxSpeed + enemySpeed;
                }
               // currentMaxSpeed = maxSpeed * 0.25f + (Mathf.Clamp(distance - 70, 0, 70) / 70);
                currentMaxSpeed = Mathf.Clamp(currentMaxSpeed, 0, maxSpeed);
                moveTarget = enemyPos;
                MoveTowardsPosition(enemyPos, 20f);
                CheckShootConditions(enemyPos);
                return;
            }

            if (movingToLastKnown)
            {
                
                float d = (lastKnownPos - ourPos).magnitude;
                if (d < 20f)
                {
                    movingToLastKnown = false;
                }
                else
                {
                    MoveTowardsPosition(lastKnownPos, targetThreshold);
                }
                return;
            }

            //Раз мы дошли до сюда, неотложных дел у нас нет

            PeacefulMove(); //Значит движемся по целевым точкам


        }
        else //либо мы в стрикт-моде, либо ГГ здох
        {
            if (!strictMode)
            {
                PeacefulMove();
            }
            else
            {
                MoveTowardsPosition(moveTarget, targetThreshold);
            }
        }

        CheckTarget();

    }

    void PeacefulMove()
    {
        currentMaxSpeed = maxSpeed * 0.5f;
       // Debug.Log("aga");
        if (targetReached)
        {
            if (!useWaypoints)
            {
                SetWanderingTarget();
                MoveTowardsPosition(moveTarget, targetThreshold);
                targetReached = false;
            }
            else //целевая точка достигнута, но мы используем навигацию по вейпоинтам
            {
                WaypointNavigation();
                // бла-бла бла, достать из загашника новый вейпоинт
            }

        }
        else
        {
            MoveTowardsPosition(moveTarget, targetThreshold);
        }
    }

    bool DetectObstacles()
    {
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(transform.position, dir);

        if (Physics.SphereCast(ray, 1f, curSpeed * 4f, obstacleMask))
        {
            return true;
        }

        return false;
    }

    void AvoidObstacle() 
    {
        Vector3[] avoidWays = new Vector3[4];
        Vector3 baseVector = transform.position + transform.TransformDirection(Vector3.forward * curSpeed * 2f);
        avoidWays[0] = baseVector + transform.TransformDirection(Vector3.right * curSpeed * 2f);
        avoidWays[1] = baseVector - transform.TransformDirection(Vector3.right * curSpeed * 2f);
        avoidWays[2] = baseVector + transform.TransformDirection(Vector3.up * curSpeed * 2f);
        avoidWays[3] = baseVector - transform.TransformDirection(Vector3.up * curSpeed * 2f);

        Vector3 escapeVector = Vector3.zero;

        for (int i = 0; i < avoidWays.Length; i++)
        {
            Vector3 dir = avoidWays[i].normalized;
            Ray ray = new Ray(transform.position, dir);

            if (Physics.SphereCast(ray, 2f, curSpeed * 3f, obstacleMask))
            {
                continue;
            }
            else
            {
                //нашли походу
                escapeVector = avoidWays[i];
                break;
            }
        }

        if (escapeVector == Vector3.zero) //впереди засада
        {
            escapeVector = transform.position - transform.TransformDirection(Vector3.forward * curSpeed * 3f); //так что валим обратно
        }

        strictMode = true;
        targetReached = false;
        moveTarget = escapeVector;
    }

    void WaypointNavigation()
    {
        moveTarget = waypointHolder.GetNextWaypoint();
        targetReached = false;
        MoveTowardsPosition(moveTarget, targetThreshold);
    }

    void SetWanderingTarget()
    {
        float range = 150f;
        Vector3 target = transform.position + Random.insideUnitSphere * range;
        moveTarget = target;
        targetReached = false;
    }

    bool CheckTarget()
    {
        if ((moveTarget - transform.position).magnitude < 20)
        {

            if (strictMode)
            {
                strictMode = false; //Мы достигли нашего обязательного вейпоинта
            }

            targetReached = true;

            return true;
        }

        return false;
    }

    void CheckShootConditions(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Vector3 fwdDir = transform.TransformDirection(Vector3.forward);
        float distance = dir.magnitude;

        if (distance < 120f)
        {
            if (Vector3.Cross(fwdDir, dir).magnitude < 1.2f)
            {
                gun.Shoot(null);
                //стрелять можно. Близко, и отклонение небольшое
            }
        }
    }

    void Move()
    {
        float steerCoef = steeringCurve.Evaluate(curSpeed / maxSpeed) * 2f * Time.deltaTime;
        if ((seeEnemy)&&(enemy != null))
        {
            body.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(enemy.transform.position - transform.position),steerCoef));
           // Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(enemy.transform.position - transform.position), 120 * Time.deltaTime);
        }
        else
        {
           body.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, steerCoef));

        }
        

        Vector3 fwdDir = transform.TransformDirection(Vector3.forward);
        body.angularVelocity = Vector3.zero;

        body.velocity = Vector3.Slerp(body.velocity,fwdDir * curSpeed , 0.8f);
    }

    public void DoDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
        }
        if((health <= 0) &&(!dead))
        {
            dead = true;
            if (explosion != null)
            {
                GameObject expl = Instantiate(explosion) as GameObject;
                expl.transform.position = transform.position;
                expl.transform.rotation = transform.rotation;
                //explosion.transform.parent = null;

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


    float lastDist = 100f;
    void MoveTowardsPosition(Vector3 pos, float threshold)
    {
        if (DetectObstacles()&&(!strictMode))  //Если препятствие обнаружено, и мы не в стрикт-моде (который говорит боту двигаться в точку, несмотря ни на что, также обозначает, что обход препятствия уже активен), то ищем подходящие пути обхода.
        {
            AvoidObstacle();
        }

        Vector3 ourPos = transform.position;
        Vector3 moveDir = pos - ourPos;
        float dist = moveDir.magnitude;

        float spd = (dist - lastDist) / deltaTime;
        lastDist = dist;
        //float estTime = dist / spd;
        //Debug.Log(spd);

        if (dist < (threshold * 2))
        {
            if (curSpeed > spd + 6)
            {
                Deccelerate();
            }

              if (dist < threshold)
            {
                Deccelerate();
            }
        }
        else
        {
                Accelerate();
        }

        Vector3 fwdDir = transform.TransformDirection(Vector3.forward);


        moveDir.Normalize();
        targetRotation = Quaternion.LookRotation(moveDir);

    }

    bool CheckVisibility(GameObject obj)
    {
        Vector3 startPos = transform.position;
        Vector3 dir = obj.transform.position - startPos;
        dir.Normalize();
        Ray ray = new Ray(startPos, dir);
        Debug.DrawRay(startPos, dir,Color.blue);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, detectDist, detectMask))
        {
            if (hit.collider.gameObject == obj)
            {
              //  Debug.Log(hit.distance);
                return true;
            }
        }

        return false;

    }

    void Accelerate()
    {
        curSpeed += acceleration * deltaTime;
        curSpeed = Mathf.Clamp(curSpeed, 0,currentMaxSpeed);
    }

    void Deccelerate()
    {
        curSpeed -= acceleration * deltaTime * 2;
        curSpeed = Mathf.Clamp(curSpeed, 0, currentMaxSpeed);
    }

    void OnCollisionEnter(Collision col)
    {

        Vector3 colNormal = col.contacts[0].normal;
        Vector3 vel = body.velocity.normalized;
        float mul = Vector3.Cross(vel, colNormal).magnitude;

        float volume = (col.relativeVelocity.magnitude / maxSpeed) + 0.25f;
        volume = Mathf.Clamp(volume, 0, 1f);

        if (col.collider.attachedRigidbody != null)
        {
            Vector3 impulse = col.collider.attachedRigidbody.mass * col.relativeVelocity;
            Vector3 ourImpulse = body.mass * body.velocity;
            if (ourImpulse.magnitude > 1)
            {
                float part = (impulse.magnitude / ourImpulse.magnitude);
                part = Mathf.Clamp(part, 0, 1);
                curSpeed -= curSpeed * part;
              //  DoDamage(Mathf.CeilToInt(part * maxHealth));
                if (col.rigidbody.mass > 1)
                {
                    DoDamage(Mathf.CeilToInt(maxHealth * 0.2f));
                    enable = false;
                    Invoke("ReEnable", 2f);
                }
               
                
            }
            else
            {
                curSpeed = 0f;
            }

            

        }
        else
        {
            int damage = 0;
            int baseDamage = 35;
            float angle = Vector3.Cross(col.relativeVelocity.normalized, body.velocity.normalized).magnitude;

            if (col.relativeVelocity.magnitude > 3f)
            {
                damage = Mathf.CeilToInt(baseDamage * angle * curSpeed);
            }

            if (angle < 0.5f)
            {
                curSpeed -= Mathf.Clamp(0.1f + angle, 0, 1) * curSpeed;
            }
            else
            {
                curSpeed = 0f;
            }

            DoDamage(damage);
        }

    }

    void ReEnable()
    {
        enable = true;
    }
	
}
