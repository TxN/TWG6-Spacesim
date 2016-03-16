using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour 
{
	public Image img;
	public float fadeTime;

	bool fade = false; // 1 - opaque, 0 - transparent

	void Start() 
    {
		img.enabled = true;
        FadeWhite(fadeTime);
	}


	void Update () 
    {

		
		if (!fade) {
			//Debug.Log("spam");
						Color col = img.color;
						col.a = col.a - Time.deltaTime * (1 / fadeTime);
						col.a = Mathf.Clamp (col.a, 0f, 1f);
						img.color = col;

				} else {
					Color col = img.color;
					col.a = col.a + Time.deltaTime * (1 / fadeTime);
					col.a = Mathf.Clamp (col.a, 0f, 1f);
					img.color = col;

				}
	}

	public void FadeBlack(float time) {
		fade = true;
		fadeTime = time;
	}

	public void FadeWhite(float time) {
		fade = false;
		fadeTime = time;
	}

}
