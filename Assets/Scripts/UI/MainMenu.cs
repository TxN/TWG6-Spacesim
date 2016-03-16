using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public GameObject optionsWindow;
   public GameObject fader;
   public InputField nameField;
   
    

    void Start()
    {
        fader.SetActive(true);
        Invoke("TurnOffFader", 4f);
        nameField.text = PlayerStats.instance.name;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void TurnOffFader()
    {
        fader.SetActive(false);
    }

    public void Load()
    {
        foreach (Transform obj in PlayerStats.instance.transform.GetComponentsInChildren<Transform>())
        {
            //Debug.Log(obj.name);
            if (obj.name != "Main")
            {
                Destroy(obj.gameObject);
            }

        }
        PlayerStats.instance.LoadFromFile(PlayerStats.instance.name);
        Application.LoadLevel(PlayerStats.instance.currentLevel);
    }

    public void New()
    {
        PlayerStats.instance.currentLevel = "OurBase";
        PlayerStats.instance.SaveToFile(PlayerStats.instance.name);
        Application.LoadLevel("Intro");
    }

    public void Options()
    {
        optionsWindow.SetActive(true);
    }

    public void HideOptions()
    {
        optionsWindow.SetActive(false);
    }

    public void OptionsOk()
    {
        PlayerStats.instance.SetName(nameField.text);

        optionsWindow.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }

}
