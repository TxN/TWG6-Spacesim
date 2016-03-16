using UnityEngine;
using System.Collections;

public class MoveForwardAndRespawn : MonoBehaviour 
{
    float respawnT = 2;

    bool first = true;
    Vector3 startPos;

    float lastT = 0f;

    float speed = 60;

    void Start()
    {
        speed += Random.Range(-10, 10);
    }

    void Update()
    {
        if (first)
        {
            first = false;
            startPos = transform.position;
            lastT = Time.time;
        }

        if (Time.time > lastT + respawnT)
        {
            transform.position = startPos;
            lastT = Time.time + Random.Range(-0.2f, 0.5f) ;
        }

        transform.Translate(0,0,speed * Time.deltaTime);
    }
}
