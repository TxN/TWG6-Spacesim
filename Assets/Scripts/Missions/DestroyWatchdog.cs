using UnityEngine;
using System.Collections;

public class DestroyWatchdog : MonoBehaviour 
{
    void OnDestroy()
    {
        transform.parent.GetComponent<MassSpawner>().NPCDestroyed();

    }
	
}
