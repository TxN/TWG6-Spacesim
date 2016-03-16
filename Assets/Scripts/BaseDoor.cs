using UnityEngine;
using System.Collections;

public class BaseDoor : MonoBehaviour 
{
    public GameObject door;
    public float doorPositionMax = -3.5f;
    float curDoorPos = 0f;

    int direction = 1;

    void Update()
    {
        curDoorPos += direction * Time.deltaTime * 2;
        curDoorPos = Mathf.Clamp(curDoorPos, doorPositionMax, 0);
        Vector3 pos = door.transform.localPosition;
        pos.z = curDoorPos;
        door.transform.localPosition = pos;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == PlayerStats.instance.player)
        {

            gameObject.GetComponent<AudioSource>().Play();
          //  anim.Play();
            direction = -1;
           
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == PlayerStats.instance.player)
        {
            gameObject.GetComponent<AudioSource>().Play();
            direction = 1;

        }
    }

}
