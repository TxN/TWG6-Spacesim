using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour 
{
    public Text title;

    bool fadeWhite = true;
    bool fadeBlack = false;

	void Start ()
    {
        Invoke("FadeBlack", 13f);
        Invoke("Load", 16f);
	}
	
	
	void Update () 
    {
        if (fadeWhite)
        {
            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 1, .2f * Time.deltaTime);
            title.color = col;
        }
        if (fadeBlack)
        {
            Color col = title.color;
            col.a = Mathf.Lerp(col.a, 0, 2*Time.deltaTime);
            title.color = col;
        }
	}

    void FadeBlack() 
    {
        fadeWhite = false;
        fadeBlack = true;
    }

    void Load()
    {
        Application.LoadLevel("OurBase");
    }

}
