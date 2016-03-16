using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CamFollow : MonoBehaviour {
   // public Vector3 offset = Vector3.zero;
    public Transform Target;
    public float MaxFOVDelta = 10f;
    Camera cam;
    float baseFov;
    Vector3 lastPos;
	void Start () 
    {
        
        cam = GetComponent<Camera>();
        baseFov = cam.fieldOfView;
        if (Target != null)
        {
            lastPos = Target.position;
        }
        
	}
	
     
	void FixedUpdate () 
    {
        if (Target != null)
        {
            // transform.position = Target.position + offset;
            //Quaternion lookRot = Quaternion.LookRotation(Target.transform.TransformDirection(Vector3.right));
            Quaternion lookRot = Target.rotation * Quaternion.Euler(0, 90, 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, 0.3f);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 90f);
            //transform.position = Vector3.Lerp(transform.position, Target.position, 0.8f);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, 5f);
            cam.fieldOfView = baseFov + CalcFOVdelta();
            lastPos = Target.position;
        }
       
	}

    float CalcFOVdelta()
    {
        float d = 0;
        float deltaDist;
        Vector3 deltaV = Target.position - lastPos;
        deltaDist = deltaV.magnitude;
        deltaDist = Mathf.Abs(deltaDist);
        deltaDist = deltaDist / Time.deltaTime;
        d = Mathf.Clamp(deltaDist / 10, 0, MaxFOVDelta);
        return d;
    }
}
