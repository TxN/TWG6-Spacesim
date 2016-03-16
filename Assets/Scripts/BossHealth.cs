using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public GameObject[] hitboxes = new GameObject[6];
    public int health = 1500;
    public int maxHealth = 9000;
    public FadeScreen fader;

    public GameObject flames;

    public RectTransform hpBarLeft;
    public RectTransform hpBarRight;
    float initBarWidth;

    void Start()
    {
        initBarWidth = hpBarLeft.rect.width;

    }

    void Update()
    {

        float percent = Mathf.Clamp((float)health / maxHealth, 0, 1);
        hpBarLeft.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * initBarWidth);
        hpBarRight.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * initBarWidth);

        if (health < (0.3f * maxHealth))
        {
            flames.SetActive(true);
        }

        if (health <= 0)
        {
            Invoke("LoadEnding", 5f);
            fader.FadeBlack(4.5f);
            AudioListener.volume = Mathf.Lerp(AudioListener.volume, 0, Time.deltaTime * 0.5f);
        }



    }

    public void AddDamage(int damage)
    {
        health -= damage;
    }

    void LoadEnding()
    {
        Application.LoadLevel("CuboidDefeatEnding");
    }
	
}
