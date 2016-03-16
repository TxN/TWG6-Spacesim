using UnityEngine;
using System.Collections;

public class ShipAudio : MonoBehaviour 
{
    public AudioSource idle;
    public AudioSource engine;
    public AnimationCurve spdCurve;
    public AnimationCurve engVolume;
    public ParticleSystem engineFlame;

    ShipControl control;
    PlayerStats stats;
    float maxSpeed;

    void Start()
    {
        control = transform.parent.GetComponent<ShipControl>();
        stats = PlayerStats.instance;
        if (stats != null)
        {
            maxSpeed = stats.maxSpeed;
        }
        
    }

    void Update()
    {
        if (stats == null)
        {
            stats = PlayerStats.instance;
            maxSpeed = stats.maxSpeed;
        }
        engine.volume = engVolume.Evaluate(Mathf.Abs(control.trgSpd) / maxSpeed);
        engine.pitch = spdCurve.Evaluate(Mathf.Abs(control.trgSpd) / maxSpeed);
        engineFlame.startSpeed = Mathf.Lerp(0.1f, 10, Mathf.Abs(control.trgSpd / maxSpeed));
    }

}
