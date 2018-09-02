using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
    public static GUIManager instance;

    public Canvas mainCanvas;

    public Sprite miniMarker;
    public Sprite selectionCorner;
    public Sprite miniMarkerEnemy;

    public Text curWeapon;
    public Text ammoNum;

    public RectTransform healthBar;
    float hpBarWidth;
    public Text healthText;
    public Text energyText;
    public RectTransform speedBar;
    float spdBarWidth;
    public RectTransform cargoBar;
    float cargoBarWidth;
    public RectTransform energyBar;
    float energyBarWidth;

    public Material curvedHPMat;
    public Material curvedEnMat;

    public Text trgName;
    public Text trgDist;
    public Text trgSpd;
    public Text trgType;

    public Text cargoText;

    public GameObject warpWindow;
    public GameObject warpLoc1;
    public GameObject warpLoc2;
    public GameObject warpLoc3;
    public GameObject warpLoc4;

    public GameObject escMenu; 

    ChatMessage[] chat = new ChatMessage[4];
    public Text[] chatStrings = new Text[4];

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        hpBarWidth = healthBar.rect.width;
        spdBarWidth = speedBar.rect.width;
        cargoBarWidth = cargoBar.rect.width;
        energyBarWidth = energyBar.rect.width;
    }

    void Update() {
        RefreshHealth();
        RefreshCargoMass();
        RefreshEnergy();

        if (warpWindow.activeInHierarchy) {
            PlayerStats stats = PlayerStats.instance;
            warpLoc1.SetActive(stats.openedLocations[0]);
            warpLoc2.SetActive(stats.openedLocations[1]);
            warpLoc3.SetActive(stats.openedLocations[2]);
            warpLoc4.SetActive(stats.openedLocations[3]);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleEscMenu();
        }
    }

    public bool ToggleWarpWindow() {
        if (warpWindow.activeInHierarchy) {
            warpWindow.SetActive(false);
            return false;
        } else {
            warpWindow.SetActive(true);
            return true;
        }
    }

    public bool ToggleEscMenu() {
        ShipControl controls = null;
        if (PlayerStats.instance.player != null) {
            controls = PlayerStats.instance.player.GetComponent<ShipControl>();
        }
       
        if (escMenu.activeInHierarchy) {
            Time.timeScale = 1f;
            escMenu.SetActive(false);
            if (controls != null) {
                controls.enable = true;
            }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return false;
        } else {
            Time.timeScale = 0f;
            escMenu.SetActive(true);
            if (controls != null) {
                controls.enable = false;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return true;
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void MainMenu() {
        Destroy(PlayerStats.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame() {
        foreach (Transform obj in PlayerStats.instance.transform.GetComponentsInChildren<Transform>()) {
            if (obj.name != "Main") {
                Destroy(obj.gameObject);
            }
            
        }
        PlayerStats.instance.LoadFromFile(PlayerStats.instance.name);
        SceneManager.LoadScene(PlayerStats.instance.currentLevel);
    }

    public void SetWarpTarget(string target) {
        PlayerStats.instance.player.GetComponent<ShipUtility>().SetJumpTarget(target);
    }

    public void ActivateJumpDrive() {
        PlayerStats.instance.player.GetComponent<ShipUtility>().JumpStart();
    }

    public void SetSpeed(float percent) {
        speedBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * spdBarWidth);
    }


    public void RefreshHealth() {
       PlayerStats stats = PlayerStats.instance;
        if (stats != null) {
            if (stats.player != null) {
                healthText.text = stats.health.ToString() + " / " + stats.maxHealth.ToString();
                SetHealth(Mathf.Clamp((float)stats.health / stats.maxHealth,0,1));

                float percent = Mathf.Clamp((float) stats.GetCargoMass() / stats.maxcargo, 0, 1);
                cargoBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * cargoBarWidth);
            }
        }
    }

    public void RefreshEnergy() {
        PlayerStats stats = PlayerStats.instance;
        if (stats != null) {
            if (stats.player != null) {
                energyText.text = stats.energy.ToString() + " / " + stats.maxEnergy.ToString();
                float percent =  (float)stats.energy / stats.maxEnergy;
                curvedEnMat.SetFloat("_Angle", percent);
                energyBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * energyBarWidth);
            }
        }

    }

    public void RefreshCargoMass() {
          PlayerStats stats = PlayerStats.instance;
          if (stats != null) {
              if (stats.player != null) {
                  cargoText.text = stats.GetCargoMass() + " / " + stats.maxcargo;
              }
          }
    }

    public void SetHealth(float percent) {
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * hpBarWidth);
        curvedHPMat.SetFloat("_Angle", percent);
    }

    public void SetCurrentWeaponName(string name) {
        curWeapon.text = name;
    }

    public void SetAmmoNum(int num) {
        ammoNum.text = num.ToString();
    }

    public void AddChatMessage(ChatMessage message) {
        chat[0] = chat[1];
        chat[1] = chat[2];
        chat[2] = chat[3];
        chat[3] = message;

        int chatStr = 0;
        for (int i = 0; i < chat.Length; i++) {
            if (chat[i] != null) {
                chatStrings[chatStr].text = chat[i].text;
                chatStrings[chatStr].color = chat[i].color;
                chatStr++;                
            }
        }
    }
}

