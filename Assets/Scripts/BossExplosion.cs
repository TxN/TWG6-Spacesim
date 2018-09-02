using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossExplosion : MonoBehaviour {
    public GameObject  boss;
    public GameObject  explosion;
    public FadeScreen  fader;
    public AudioSource audio;
    public GameObject  ship;
    public GameObject  shipWarp;
    public Image       title;

    bool shipMove = false;
    bool camFollow = false;

    bool showTitle = false;
    bool mute = false;

    float startFOV;

	void Awake() {
		Time.timeScale = 1f;
	}
	
    void Start() {
        startFOV = Camera.main.fieldOfView;
        fader.gameObject.SetActive(true);
        Invoke("ExplodeBoss", 6f);
       
        Invoke("ShipComeIn", 12f);
        Invoke("ShipFollow", 12.4f);
        Invoke("StartWarp", 12.5f);
        Invoke("HideJump", 16.2f);
        Invoke("Fade", 17f);
        Invoke("Title", 30f);
        Invoke("Mute", 45f);
        Invoke("Load", 55f);
    }

    void Update() {
        if (shipMove) {
            ship.transform.Translate(25f * Time.deltaTime, 0, 0);
        }

        if (camFollow) {
            Vector3 lookDir = (ship.transform.position - Camera.main.transform.position).normalized;
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(lookDir), 0.15f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startFOV - 25f, 2f * Time.deltaTime);
        }

        if (showTitle) {
            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 1, .2f * Time.deltaTime);
            title.color = col;
        }

        if (mute) {
            audio.volume = Mathf.Lerp(audio.volume, 0, .4f * Time.deltaTime);

            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 0,  2* Time.deltaTime);
            title.color = col;
        }
    }

    void ExplodeBoss() {
        boss.SetActive(false);
        explosion.SetActive(true);
        Camera.main.GetComponent<ScreenShake>().InitIntensity = 2;
        Camera.main.GetComponent<ScreenShake>().InitDecay = 0.008f;
        Camera.main.GetComponent<ScreenShake>().Shake();

        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>().focalTransform = ship.transform;
    }

    void ShipComeIn() {
        shipMove = true;
    }

    void ShipFollow() {    
        camFollow = true;
    }

    void StartWarp() {
        shipWarp.SetActive(true);
    }

    void HideJump() {
        ship.SetActive(false);
        shipWarp.SetActive(false);
    }

    void Fade() {
        fader.FadeBlack(5f);
    }

    void Title() {
        showTitle = true;
    }

    void Mute() {
        mute = true;
    }

    void Load() {
        SceneManager.LoadScene("MainMenu");
    }

}
