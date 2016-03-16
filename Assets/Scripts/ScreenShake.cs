using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	GameObject orPos;
	Vector3 originPosition;
	Quaternion originRotation;
	float shake_decay;
	float shake_intensity;

    public float InitDecay = 0.002f;
    public float InitIntensity = 0.15f;

    public bool EnableDebug = false;
    public bool startShake = false;
	
	void Start ()
    {
		orPos = new GameObject ();
		orPos.transform.position = this.transform.position;
		orPos.transform.parent = this.transform.parent;

        if (startShake)
        {
            Shake();
        }
	}

    void Update()
    {
        //Debug shake call
        if (Input.GetKey("q")&&(EnableDebug))
        {
            Shake();
        }

       
    }


	void LateUpdate()
    {
        if (shake_intensity > 0f)
        {
            transform.position = orPos.transform.position + Random.insideUnitSphere * shake_intensity;

            shake_intensity -= shake_decay;
        }
	}
	
	public void Shake()
    {
		originPosition = transform.position;
		originRotation = transform.rotation;
        shake_intensity = InitIntensity;
        shake_decay = InitDecay;
	}
}
