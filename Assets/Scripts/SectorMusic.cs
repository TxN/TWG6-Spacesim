using UnityEngine;
using System.Collections;

public class SectorMusic : MonoBehaviour 
{
    public float volume = 0.5f;
    AudioSource audio;

    bool musicPlaying = false;

	void Start () 
    {
        audio = GetComponent<AudioSource>();
        Invoke("StartMusic", Random.Range(5, 15));
	}

	void Update () 
    {
        if (musicPlaying)
        {
            audio.volume = Mathf.Lerp(audio.volume, volume, 0.3f * Time.deltaTime);
        }

	}

    void StartMusic()
    {
      //  audio.Play();
        musicPlaying = true;
    }
}
