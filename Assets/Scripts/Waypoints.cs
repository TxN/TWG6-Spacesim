using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour 
{
    public Transform[] waypoints;
    public enum RouteType
    {
        OneWay,
        Loop,
        Patrol
    }
    public RouteType type;

    int index = 0;
    bool increasing = true; //для типа "патруль". Если тру, то идем вверх по массиву, иначе вниз

    public Vector3 GetNextWaypoint()
    {
        Vector3 result;

        if (type != RouteType.Patrol)
        {
            if (index < waypoints.Length)
            {
                result = waypoints[index].position;
                index++;

            }
            else
            {
                if (type == RouteType.OneWay)
                {
                    return Vector3.zero;
                }
                else
                {
                    index = 0;
                    result = waypoints[index].position;
                    index++;
                }
            }
        }
        else
        {
            if (increasing)
            {
                if (index < waypoints.Length)
                {
                    result = waypoints[index].position;
                    index++;

                }
                else
                {
                    increasing = false;
                    index--;
                    result = waypoints[index].position;
                }
            }
            else
            {
                if (index > 0)
                {
                    result = waypoints[index].position;
                    index--;
                }
                else
                {
                    result = waypoints[index].position;
                    index++;
                    increasing = true;
                }
            }
        }
        
        return result;
    }

}
