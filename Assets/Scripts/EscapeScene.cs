using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class EscapeScene : MonoBehaviour 
{
    public FadeScreen fader;
    public GameObject ship;
    public GameObject warping;

    public GameObject enemy;
    public GameObject enemyWarping;

    public Transform lookTarget;

    public Image title;

    float startWarpTime = 2;

    bool camLook = false;
    bool enemyWarped = false;
    Vector3 enemyStartScale;

    bool showTitle = false;
    bool mute = false;
	
	void Awake()
	{
		Time.timeScale = 1f;
	}

    void Start()
    {
        enemyStartScale = enemyWarping.transform.localScale;

        fader.FadeWhite(5);

        Invoke("StartWarp", 4f);
        Invoke("HideBase", 12f);
        Invoke("HideWarp", 12.2f);
        Invoke("CamLook", 13.5f);
        Invoke("BossWarp", 14.5f);
        Invoke("BossShow", 15f);
        Invoke("BossWarpHide", 16.5f);
        Invoke("FadeBlack", 25f);
        Invoke("Title", 40f);
        Invoke("Mute", 55f);
        Invoke("Load", 65f);
    }

    void Update()
    {
        if (camLook)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation((lookTarget.position - Camera.main.transform.position).normalized), 1.5f * Time.deltaTime);
        }

        if (enemyWarped)
        {
            enemyWarping.transform.localScale = Vector3.Lerp(enemyWarping.transform.localScale,enemyStartScale * 0.4f , 2f);
        }

        if (showTitle)
        {
            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 1, .2f * Time.deltaTime);
            title.color = col;
        }

        if (mute)
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, .4f * Time.deltaTime);

            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime);
            title.color = col;
        }
    }

    void StartWarp()
    {
        warping.SetActive(true);
    }

    void HideBase()
    {
        ship.SetActive(false);
    }

    void HideWarp()
    {
        warping.SetActive(false);
    }

    void CamLook()
    {
        camLook = true;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>().focalTransform = lookTarget;
    }

    void BossWarp()
    {
        enemyWarping.SetActive(true);
    }

    void BossShow()
    {
        enemyWarped = true;
        enemy.SetActive(true);
    }

    void BossWarpHide()
    {
        enemyWarping.SetActive(false);
    }

    void FadeBlack()
    {
        fader.FadeBlack(8f);
    }

    void Title()
    {
        showTitle = true;
    }

    void Mute()
    {
        mute = true;
    }

    void Load()
    {
        Application.LoadLevel("MainMenu");
    }
}
