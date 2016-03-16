using UnityEngine;
using System.Collections;

public class DropDetector : MonoBehaviour 
{
    public string targetLevel = "CuboidSector";

    void OnAwake()
    {
        if (Application.loadedLevelName == targetLevel)
        {
            PlayerStats.instance.gameObject.GetComponentInChildren<IQuest>().GoalAchieved();
        }

    }
    void OnDestroy()
    {
            
        if (Application.loadedLevelName == targetLevel)
        {
            PlayerStats.instance.gameObject.GetComponentInChildren<IQuest>().GoalAchieved();
        }
    }
}
