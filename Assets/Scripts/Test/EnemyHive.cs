using UnityEngine;
using System.Collections;

public class EnemyHive : MonoBehaviour
{
    public float spawnTime = 10f;
    public float spawnMaxRadius = 500f;
    public float spawnMinRadius = 50f;

    public GameObject enemyFab;

    float lastSpawnT = 0;

    void Update()
    {
        if (Time.time > lastSpawnT + spawnTime)
        {
            GameObject enemy = Instantiate(enemyFab);
            enemy.transform.position = Random.insideUnitSphere * spawnMaxRadius;
            enemy.transform.rotation = Quaternion.LookRotation((enemy.transform.position - transform.position).normalized);
            lastSpawnT = Time.time;
        }
    }

}
