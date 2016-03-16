using UnityEngine;
using System.Collections;

public class Autodestruction : MonoBehaviour {

    public float destrTime = 0f;

    void Start()
    {
        if (destrTime > 0)
        {
            Invoke("Destruct", destrTime);
        }
    }

    public void Set(float time)
    {
        Invoke("Destruct", time);
    }

    public void Destruct()
    {
        Destroy(this.gameObject);
    }
	
}
