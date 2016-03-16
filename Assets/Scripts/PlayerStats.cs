using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;
   
    [Header("Stats")]
    public string name = "Mingebag";
    public int money = 150;
    public int health = 1500;
    public int maxHealth = 1500;
    public int maxcargo = 2500;
    public int energy = 2000;
    public int maxEnergy = 2000;

    public List<Item> inventory = new List<Item>();

    [Header("Controls")]
    public float maxSpeed = 30;
    public float acceleration = 3f;
    public float yawPitchFactor = 1.2f;
    public float rollFactor = 1.2f;

    [Header("Weapons")]
    public GameObject weaponSlot1;

    public string wS1Name = "";
    public int wS1Num = 0;

    public GameObject weaponSlot2;

    public string wS2Name = "";
    public int wS2Num = 0;

    public GameObject[] auxSlot = new GameObject[4];

    public string[] auxName = new string[4];
    public int[] auxNum = new int[4];


    [Header("Other")]
    public bool spawnInNextScene = false;
    public int spawnPositionIndex = 1;

    public GameObject playerPrefab;
    public GameObject player;
    public bool spawned = false;
    public string currentLevel ="";
    public bool[] openedLocations = new bool[4];
    public bool populateLocations = false;

    [Header("Quests")]
    
    public bool hasActiveQuest = false;
    public string[] questsBase1;
    public string[] questsBase2;
    public int base1QuestNum = 0;
    public int base2QuestNum = 0;


    public void SaveToFile(string filename)
    {
        SaveInfo save = new SaveInfo();
        save.name = name;
        save.money = money;
        save.health = health;
        save.maxHealth = maxHealth;
        save.maxcargo = maxcargo;
        save.inventory = inventory;
        save.maxSpeed = maxSpeed;
        save.acceleration = acceleration;
        save.yawPitchFactor = yawPitchFactor;
        save.rollFactor = rollFactor;
        save.wS1Name = wS1Name;
        save.wS1Num = wS1Num;
        save.wS2Name = wS2Name;
        save.wS2Num = wS2Num;
        save.auxName = auxName;
        save.auxNum = auxNum;
        save.spawnInNextScene = spawnInNextScene;
        save.spawnPositionIndex = spawnPositionIndex;
        save.spawned = spawned;
        save.hasActiveQuest = hasActiveQuest;
        save.base1QuestNum = base1QuestNum;
        save.base2QuestNum = base2QuestNum;
        save.loadedLevel = currentLevel;
        save.openedLocations = openedLocations;
        save.populateLocations = populateLocations;

        save.Save(Application.dataPath + "/Save/"+filename+".xml");
        
    }

    public void LoadFromFile(string filename)
    {
   

        SaveInfo save = SaveInfo.Load(Application.dataPath + "/Save/" + filename + ".xml");
        name = save.name;
        money = save.money;
        health = save.health;
        maxHealth = save.maxHealth;
        maxcargo = save.maxcargo;
        inventory = save.inventory;
        maxSpeed = save.maxSpeed;
        acceleration = save.acceleration;
        yawPitchFactor = save.yawPitchFactor;
        rollFactor = save.rollFactor;
        wS1Name = save.wS1Name;
        wS1Num = save.wS1Num;
        wS2Name = save.wS2Name;
        wS2Num = save.wS2Num;
        auxName = save.auxName;
        auxNum = save.auxNum;
        spawnInNextScene = save.spawnInNextScene;
        spawnPositionIndex = save.spawnPositionIndex;
        spawned = save.spawned;
        hasActiveQuest = save.hasActiveQuest;
        base1QuestNum = save.base1QuestNum;
        //Debug.Log("Load quest:"+base1QuestNum);
        base2QuestNum = save.base2QuestNum;
        currentLevel = save.loadedLevel;
        openedLocations = save.openedLocations;
        populateLocations = save.populateLocations;

        
    }


    public void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            SaveToFile("testSave");
        }
    }

    public void GetQuest(int baseNum) 
    {
        string folderPath = "Quests/";
        if (!hasActiveQuest)
        {
            if (baseNum == 1)
            {
                folderPath += questsBase1[base1QuestNum];
            }
            else
            {
                folderPath += questsBase2[base2QuestNum];
            }

            GameObject quest = Instantiate((GameObject)Resources.Load(folderPath)) as GameObject;
            quest.transform.parent = transform;
            hasActiveQuest = true;
        }
    }

    public void RefreshWeapons()
    {
        if (weaponSlot1 != null)
        {
            wS1Name = weaponSlot1.GetComponent<IWeapon>().GetWeaponName();
            wS1Num = weaponSlot1.GetComponent<IWeapon>().GetAmmoNum();
        }

        if (weaponSlot2 != null)
        {
            wS2Name = weaponSlot2.GetComponent<IWeapon>().GetWeaponName();
            wS2Num = weaponSlot2.GetComponent<IWeapon>().GetAmmoNum();
        }

        for (int i = 0; i < 4; i++)
        {
            if (auxSlot[i] != null)
            {
                auxName[i] = auxSlot[i].GetComponent<IWeapon>().GetWeaponName();
                auxNum[i] = auxSlot[i].GetComponent<IWeapon>().GetAmmoNum();
            }
            
        }
    }


    public void SpawnPlayer(Vector3 pos)
    {
        player = Instantiate(playerPrefab, pos, Quaternion.identity) as GameObject;
        
      //  player.transform.position = pos;
        GameObject ship = player.transform.FindChild("Ship").gameObject;
        ship.GetComponent<ShipEquip>().EquipPlayer();
        player = ship;
        Camera cam = transform.GetComponentInChildren<Camera>();
    }

    public int RemoveAllInInventory(string itName)
    {
        int count = 0;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == itName)
            {
                count++;
                inventory.Remove(inventory[i]);
                i--; // так как список смещается.
            }
        }

        return count;
    }

    public bool AddToInventory(Item it)
    {
        if (GetCargoMass() >= maxcargo)
        {
            return false;
        }
        else
        {
            inventory.Add(it);
            return true;
        }
    }

    public int GetCargoMass()
    {
        int mass = 0;
        if (!(inventory.Count == 0))
        {
            foreach (Item itm in inventory)
            {
                mass += itm.weight;
            }
        }

        return mass;
    }

    public void SetName(string nom)
    {
        name = nom;
        PlayerPrefs.SetString("Name", name);
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Awake()
    {
        if ((instance != null) && (instance != this.gameObject))
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        if (instance == null)
        {
            instance = this;
        }

        string nom = PlayerPrefs.GetString("Name");
        if (nom == null)
        {
            PlayerPrefs.SetString("Name", name);

        }
        else
        {
            name = nom;
        }

       DontDestroyOnLoad(this.gameObject);

       
    }

    void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1f;
        currentLevel = Application.loadedLevelName;
        DontDestroyOnLoad(this.gameObject);
        if (spawnInNextScene)
        {
            spawnInNextScene = false;
            Vector3 spawnPos = GameObject.Find("SceneManager").GetComponent<SceneParameters>().PlayerSpawnPoint[spawnPositionIndex];
            SpawnPlayer(spawnPos);
            player.transform.parent.rotation *= Quaternion.Euler(0, 180, 0);
        }

        if (populateLocations)
        {
            if ((currentLevel == "ScienceBaseSector") || (currentLevel == "1"))
            {
                GUIManager.instance.gameObject.GetComponent<MassSpawner>().enabled = true;
            }
        }

    }
	
}
