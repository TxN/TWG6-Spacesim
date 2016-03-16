using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rotate : MonoBehaviour {
	public float speed = 30f;
	

	void Update () 
	{
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
	}
}
