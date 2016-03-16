using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidFieldGenerator : MonoBehaviour {
    public GameObject[] asteroids = new GameObject[6];
    public int seed = 42;
    public List<GameObject> objects = new List<GameObject>();
    public int number = 120;
    public Vector3 range = new Vector3 (2000, 500, 2000);
    GameObject root;

    void Start()
    {
        root = new GameObject();
        root.transform.position = Vector3.zero;
        root.name = "Asteroid Field";

        Random.seed = seed;
        for (int i = 0; i < number; i++)
        {
            GameObject aster = Instantiate(asteroids[Random.Range(0, asteroids.Length - 1)], new Vector3(Random.Range(-range.x, range.x),Random.Range(-range.y, range.y), Random.Range(-range.z, range.z)), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
            aster.transform.localScale = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
            aster.GetComponent<MeshCollider>().sharedMesh = aster.GetComponent<MeshFilter>().mesh;
            aster.transform.parent = root.transform;
            objects.Add(aster);
        }
    }

}
