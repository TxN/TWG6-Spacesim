using UnityEngine;
using System.Collections;

public class FlyTrigger : MonoBehaviour 
{
    bool triggered = false;

    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject == PlayerStats.instance.player)&&(!triggered))
        {
            triggered = true;
            PlayerStats.instance.gameObject.GetComponentInChildren<IQuest>().GoalAchieved();
        }

    }

}
