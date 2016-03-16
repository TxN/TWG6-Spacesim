using UnityEngine;
using System.Collections;

public class MassSpawner : MonoBehaviour 
{
    public int maxNum = 10;
    public int initNum = 10;
    public bool respawnOnKill = false;
    public float spawnRadius = 300;
    public float timedSpawnDelay = 10f;
    public GameObject enemy1;
    public GameObject enemy2;
    int num = 0;
    float lastSpawnT = 0;

    public bool TriggerMissionGoal = true;
    public int triggerNum = 2;
    bool triggered = false;

           

    void Start()
    {
        for (int i = 0; i <initNum; i++)
        {
            SpawnEnemy();
        }

        lastSpawnT = Time.time;
    }

    void Update()
    {
        if (respawnOnKill)
        {
            if (num < maxNum)
            {
                if (Time.time > lastSpawnT + timedSpawnDelay)
                {
                    SpawnEnemy();
                    lastSpawnT = Time.time;
                }
            }
        }

        if ((TriggerMissionGoal)&&(!triggered))
        {
            if (num <= triggerNum)
            {
                PlayerStats.instance.gameObject.GetComponentInChildren<IQuest>().GoalAchieved();
                triggered = true;
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy;
        if (Random.Range(0, 3) >= 2)
        {
            enemy = Instantiate(enemy1) as GameObject;
        }
        else
        {
            enemy = Instantiate(enemy2) as GameObject;
        }

        enemy.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
        enemy.transform.parent = this.transform;
        enemy.transform.rotation = Random.rotationUniform;
        enemy.AddComponent<DestroyWatchdog>();
        num++;
    }

    public void NPCDestroyed()
    {
        num--;
    }



	
}
