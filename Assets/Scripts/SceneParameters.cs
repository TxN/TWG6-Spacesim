using UnityEngine;
using System.Collections;

public class SceneParameters : MonoBehaviour {
    public static SceneParameters instance;
    public Vector3[] PlayerSpawnPoint = new Vector3[5];


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
