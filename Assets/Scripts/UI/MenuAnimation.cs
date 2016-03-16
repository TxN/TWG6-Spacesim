using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour 
{
    public GameObject Ship;

    public GameObject Camera;

    public int chance = 350;
    Vector3 moveTg;

    public float time = 0.85f;
    float lastMoveT = 0f;

	void Awake()
	{
		Time.timeScale = 1f;
	}

    void Start()
    {
        moveTg = Camera.transform.localPosition;
    }

    void Update()
    {

        if (Time.time > lastMoveT + time)
        {
            lastMoveT = Time.time;
            moveTg = Random.insideUnitSphere + Camera.transform.localPosition;
        }
            
        

        Camera.transform.localPosition = Vector3.Slerp(Camera.transform.localPosition, moveTg, Time.deltaTime * 0.1f);
        Ship.transform.Translate(Vector3.right * 0.5f);
      //  Camera.transform.LookAt(Ship.transform);

    }



}
