using UnityEngine;
using System.Collections;

public class ShipJumpAnim : MonoBehaviour 
{
    public GameObject effect;
    public GameObject flash;

    public float phase1Time = 2f;
    public float phase2Time = 3.2f;
    public float stretchScale = 1.25f;
    public bool hideParent = false;
    public float cameraShake = 0.08f;

    int phaseNum = 0;
    Vector3 phase0LocalScale;

    public AnimationCurve stretchCurve;
    float startTime;
    float startFOV;

    public bool scaleX = true;
    public bool scaleY = false;
    public bool scaleZ = false;

    bool first = true;

    public bool addShake = true;
    public bool changeFOV = true;

    void Awake()
    {
        startTime = Time.time;
        phase0LocalScale = effect.transform.localScale * stretchScale;
        if (addShake)
        {
            ScreenShake camShake = Camera.main.gameObject.AddComponent<ScreenShake>();
            camShake.startShake = true;
            camShake.InitDecay = 0;
            camShake.InitIntensity = cameraShake;
        }
       
       startFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        if (first)
        {
            first = false;
            startTime = Time.time;
        }
        if (phaseNum == 0)
        {
            effect.transform.localScale = Vector3.Lerp(effect.transform.localScale, phase0LocalScale, phase1Time * Time.deltaTime);
          //  Debug.Log("Phase1");
            if (Time.time > (startTime + phase1Time))
            {
                phaseNum++;
            }
        }
            else if (phaseNum == 1)
            {
                Vector3 scale = effect.transform.localScale;
                if (scaleX)
                {
                    scale.x = stretchCurve.Evaluate((Time.time - startTime) - phase1Time) * stretchScale;
                }

                if (scaleY)
                {
                   // scale.y = stretchCurve.Evaluate((Time.time - startTime) - phase1Time) * stretchScale;
                    scale.y = Mathf.Lerp(scale.y, 0.95f, (phase2Time - phase1Time)*Time.deltaTime);
                }

                if (scaleZ)
                {
                    scale.z = stretchCurve.Evaluate((Time.time - startTime) - phase1Time) * stretchScale;
                }

                effect.transform.localScale = scale;

                if (changeFOV)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startFOV + 15, ((Time.time - startTime) - phase1Time) * 0.15f);
                }
                
              //  Debug.Log("Phase2");
                if (Time.time > (startTime + phase2Time))
                {
                    phaseNum++;
                }
            }
            else if (phaseNum == 2)
            {
                flash.SetActive(true);
                flash.transform.parent = null;
                if (hideParent)
                {
                    transform.parent.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
				GetComponent<TrailRenderer>().time = 0.6f;
             //   Debug.Log("Phase3");
            }
   }
}
	

