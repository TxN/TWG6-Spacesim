using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipUtility : MonoBehaviour, IDamageReciever {
    public PlayerStats stats = PlayerStats.instance;
    public AudioClip   pickupSnd;
    public AudioClip   picupFailSnd;
    public AudioClip   dropSnd;
    public AudioClip   jumpRejectSnd;
    public AudioClip   damageSnd;
    public AudioClip   lowEnergy;
    public GameObject  deadFab;
    public GameObject  jumpFab;

    GameObject  jumper;
	AudioSource audioSource;

	string jumpScene;
    bool   dead = false;

	const int ENERGY_RESTORE_PER_TICK = 4;

	void Start () {
        stats = PlayerStats.instance;
        audioSource = GetComponent<AudioSource>();
	}

    public int GetHealth() {
        return stats.health;
    }

    public int GetMaxHealth() {
        return stats.maxHealth;
    }

    public void DoDamage(int amount) {
        stats.health -= amount;
        audioSource.PlayOneShot(damageSnd);

        if ((stats.health <= 0)&&(!dead)) {
            dead = true;
            if (deadFab != null) {
                CancelInvoke();
                Camera.main.transform.parent = null;
                Instantiate(deadFab, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
        }
    }

    public void GrabItems() {
        foreach (Collider col in Physics.OverlapSphere(transform.position, 10f)) {
            if (PickupItem(col.gameObject)) {
                audioSource.PlayOneShot(pickupSnd);
                return;
            }
	    } 
    }


    public void DropItem(Item it) {
        GameObject item = Instantiate(Resources.Load(it.name), transform.position + new Vector3(0, 0, -5), Quaternion.identity)as GameObject;
        item.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        audioSource.PlayOneShot(dropSnd);
        stats.inventory.Remove(it);
    }

    public bool PickupItem(GameObject itemObj) {
        Itm it = itemObj.GetComponent<Itm>();
        if (it) {
            if (stats.AddToInventory(new Item(it) ) ) {
                GUIManager.instance.AddChatMessage(new ChatMessage(it.title + " помещен в трюм", Color.green));
                Destroy(itemObj);
                return true;
            } else {
                audioSource.PlayOneShot(pickupSnd);
                return false;
            }
        } else {
            return false;
        }
    }

    public void SetJumpTarget(string target) {
        jumpScene = target;
    }

   void Jump() {
        PlayerStats stats = PlayerStats.instance;
        stats.spawnPositionIndex = 1;
        stats.spawnInNextScene = true;
        stats.RefreshWeapons();
        SceneManager.LoadScene(jumpScene);
    }

  public  void JumpStart() {
        if ((GetComponent<ShipControl>().trgSpd > PlayerStats.instance.maxSpeed * 0.9f)&&(jumpScene != SceneManager.GetActiveScene().name)) {
            ToggleJumpMenu();
            jumper = Instantiate(jumpFab) as GameObject;
            jumper.transform.position = transform.position;
            jumper.transform.parent = transform;
            jumper.transform.rotation = transform.rotation;
            Camera.main.transform.parent = PlayerStats.instance.player.transform;
            Camera.main.gameObject.GetComponent<CamFollow>().enabled = false;
            GetComponent<ShipControl>().enable = false;
            Invoke("Jump", 3.7f);
        } else {
            GUIManager.instance.AddChatMessage(new ChatMessage("Прыжок отменен. Недостаточная скорость или некорректная точка назначения.", Color.red));
            audioSource.PlayOneShot(jumpRejectSnd);
        }
    }

    void ToggleJumpMenu() {
        ShipControl controls = GetComponent<ShipControl>();
        bool state = GUIManager.instance.ToggleWarpWindow();
        if (state) { //меню было открыто
			controls.enable = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            controls.enable = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void FixedUpdate(){
        stats.energy = Mathf.Clamp(stats.energy + ENERGY_RESTORE_PER_TICK, 0, stats.maxEnergy);
    }

    void Update() {
        if (stats == null) {
            stats = PlayerStats.instance;
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            GrabItems();
        }

        if (Input.GetKeyDown(KeyCode.G)) {
			if (stats.inventory.Count > 0) {
				DropItem(stats.inventory[0]);
			}
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            ToggleJumpMenu();
        }
    }
}
