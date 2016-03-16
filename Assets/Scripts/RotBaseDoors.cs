using UnityEngine;
using System.Collections;

public class RotBaseDoors : MonoBehaviour 
{
    public GameObject Door1;
    public GameObject Door2;
    AudioSource audioSource;

    public Vector3 rotAxis = new Vector3(0, 0, 1);
    public float angle = 35f;

    public float curAngle1 = 0f;
    public float curAngle2 = 0f;
    float direction = 1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        curAngle1 += direction * Time.deltaTime * 20f;
        curAngle2 -= direction * Time.deltaTime * 20f;

        curAngle1 = Mathf.Clamp(curAngle1, -angle, 0);
        curAngle2 = Mathf.Clamp(curAngle2, 0, angle);

        Door1.transform.localRotation = Quaternion.Euler(0,0, curAngle1);
        Door2.transform.localRotation = Quaternion.Euler(0, 0, curAngle2);
    }


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject == PlayerStats.instance.player)
        {
            Debug.Log("Enter");
            audioSource.Play();
            direction = -1;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == PlayerStats.instance.player)
        {
            Debug.Log("Exit");
            audioSource.Play();
            direction = 1;
        }
    }
	
}
