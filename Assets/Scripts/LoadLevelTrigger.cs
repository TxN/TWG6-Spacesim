using UnityEngine;
using System.Collections;

public class LoadLevelTrigger : MonoBehaviour
{
    public string sceneToLoad;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == PlayerStats.instance.player)
        {

            PlayerStats.instance.RefreshWeapons();
            Application.LoadLevel(sceneToLoad);

        }
    }

}
