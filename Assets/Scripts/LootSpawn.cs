using UnityEngine;
using System.Collections;

public class LootSpawn : MonoBehaviour 
{
    public string[] fabNames;

    void Start()
    {
        GameObject loot = Instantiate(Resources.Load(fabNames[Random.Range(0, fabNames.Length - 1)]),transform.position, Quaternion.identity) as GameObject;
    }
}
