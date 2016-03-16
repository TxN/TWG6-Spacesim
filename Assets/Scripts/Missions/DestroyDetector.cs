using UnityEngine;
using System.Collections;

public class DestroyDetector : MonoBehaviour 
{

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnLevelWasLoaded(int level)
    {
        active = false;
        Destroy(this.gameObject);
    }

    bool active = true;

    void OnDestroy()
    {
        if (active)
        {
            Debug.Log("DestroyDetector have been destroyed");
            PlayerStats.instance.gameObject.GetComponentInChildren<IQuest>().GoalAchieved();
        }
        
        
    }

}
