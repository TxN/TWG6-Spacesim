using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuGlobal : MonoBehaviour 
{
    public int baseNum = 1;
    public string baseLoc = "1";
    public static MenuGlobal instance;
    public AudioSource audio;
    public Canvas shopCanvas;
    public GameObject questWindow;
    public GameObject hangarWindow;
    public GameObject sellPanel;
    public GameObject inventoryPanel;
    public int activeInventorySlot = -1;
    public Text questText;
    public string questInitText;
    public Text itemInfo;
    public Text infoText;
    public Text money;

    public Text anwser1ButtonText;
    public Text anwser2ButtonText;
    public Text anwser3ButtonText;
    public string anwser1InitText;
    public string anwser2InitText;
    public string anwser3InitText;

    public List<DragItem> items = new List<DragItem>();

    public AudioClip sellSound;
    public AudioClip buySound;
    public AudioClip repairSound;
    public AudioClip refillSound;
    public AudioClip failSound;
    public AudioClip selectSound;
    public AudioClip clickSound;
    public AudioClip buttonClick;
    public AudioClip questTaken;
    public AudioClip questComplete;

    bool hasQuestActive = false;

    bool first = true;

    IQuest quest;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        anwser1InitText = anwser1ButtonText.text;
        anwser2InitText = anwser2ButtonText.text;
        anwser3InitText = anwser3ButtonText.text;
        questInitText = questText.text;

        
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (instance == null)
        {
            instance = this;
        }

        DragItem[] itemsInScene = inventoryPanel.transform.GetComponentsInChildren<DragItem>();
        foreach (DragItem item in itemsInScene)
        {
          //  Debug.Log("add");
            items.Add(item);
        }


  
    }

    void Update()
    {
        if (first)
        {
            first = false;
            
            RefreshInventoryList();
            PlayerStats stats = PlayerStats.instance;

            if (!stats.hasActiveQuest)
            {
              //  PlayerStats.instance.SaveToFile(PlayerStats.instance.name);
            }
            else
            {
                quest = PlayerStats.instance.GetComponentInChildren<IQuest>();
                if (quest.IsQuestActive())
                {
                    SetNextReplica();
                }
            }
           
        }

        money.text = PlayerStats.instance.money.ToString();
    }

    public void SetNextReplica()
    {
        if (quest.IsQuestActive())
        {
            questText.text = quest.GetReplica();
            anwser1ButtonText.text = quest.GetAnwser(1);
            anwser2ButtonText.text = quest.GetAnwser(2);
            anwser3ButtonText.text = quest.GetAnwser(3);
        }

    }

    public void ClearQuestWindow()
    {
       // Debug.Log("clear");
        anwser1ButtonText.text = anwser1InitText;
        anwser2ButtonText.text = anwser2InitText;
        anwser3ButtonText.text = anwser3InitText;
        questText.text = questInitText;
    }

    public void RefreshInventoryList()
    {
        int index = 0;
        foreach (DragItem item in items)
        {
            item.UnsetItem();

        }

        foreach (Item item in PlayerStats.instance.inventory)
        {
            items[index].itemName = item.name;
            items[index].itemPrice = item.price;
            items[index].itemTitle = item.title;
            items[index].itemDescription = item.desc;
            items[index].index = index;
            items[index].SetItem(item.name);
            index++;
        }
    }

    public void SetActiveInventoryItem(int slot)
    {
        if (activeInventorySlot != -1)
        {
            items[activeInventorySlot].DeselectItem();
        }
        

        activeInventorySlot = slot;
    }

    public void SellInventoryItem()
    {
        if (items[activeInventorySlot].itemName != "")
        {
            PlayerStats.instance.money += items[activeInventorySlot].itemPrice;
            PlayerStats.instance.inventory.RemoveAt(activeInventorySlot);
            items[activeInventorySlot].UnsetItem();
            items[activeInventorySlot].DeselectItem();
            RefreshInventoryList();
        }
        
   

    }

    public void SetDescriptionText(string text)
    {
        infoText.text = text;
        audio.PlayOneShot(selectSound);
    }

    public void SetItemDescriptionText(string text)
    {
        itemInfo.text = text;
        audio.PlayOneShot(selectSound);
    }

    public bool BuyItem(int slotNum, int price, string weaponName, int ammoNum)
    {
        PlayerStats stats = PlayerStats.instance;
       // Debug.Log("Trying to buy");
        if (stats.money >= price)
        {
            stats.money -= price;
            SaveWeaponInfo(slotNum, weaponName, ammoNum);
            audio.PlayOneShot(buySound);
            return true;
        }
        return false;
    }

    public void SaveWeaponInfo(int slotNum, string weaponName, int ammoNum)
    {
        PlayerStats stats = PlayerStats.instance;
        switch (slotNum)
            {
                case 1:
                    stats.wS1Name = weaponName;
                    stats.wS1Num = ammoNum;

                    break;
                case 2:
                    stats.wS2Name = weaponName;
                    stats.wS2Num = ammoNum;
                    break;
                case 3:
                    stats.auxName[0] = weaponName;
                    stats.auxNum[0] = ammoNum;
                    break;
                case 4:
                    stats.auxName[1] = weaponName;
                    stats.auxNum[1] = ammoNum;
                    break;
                case 5:
                    stats.auxName[2] = weaponName;
                    stats.auxNum[2] = ammoNum;
                    break;
                case 6:
                    stats.auxName[3] = weaponName;
                    stats.auxNum[3] = ammoNum;
                    break;

                default:
                    return;

            }
    }

    public bool SellItem(int slotNum, int fullPrice, string weaponName)
    {
        PlayerStats stats = PlayerStats.instance;
        switch (slotNum)
        {
            case 1:
                stats.weaponSlot1 = null;
                stats.wS1Name = "";
                stats.wS1Num = 0;

                break;
            case 2:
                stats.weaponSlot1 = null;
                stats.wS2Name = "";
                stats.wS2Num = 0;
                break;
            case 3:
                stats.auxSlot[0] = null;
                stats.auxNum[0] = 0;
                stats.auxName[0] = "";
                break;
            case 4:
                stats.auxSlot[1] = null;
                stats.auxNum[1] = 0;
                stats.auxName[1] = "";
                break;
            case 5:
                stats.auxSlot[2] = null;
                stats.auxNum[2] = 0;
                stats.auxName[2] = "";
                break;
            case 6:
                stats.auxSlot[3] = null;
                stats.auxNum[3] = 0;
                stats.auxName[3] = "";
                break;

            default:
                return false;
                
        }

        stats.money += Mathf.CeilToInt(fullPrice * 0.7f);
        audio.PlayOneShot(sellSound);
        return true;
    }

    public void SwapItems(DragWeapon item1, DragWeapon item2)
    {
     /*   if ((item1.name == "") || (item1.name == null))
        {
            item1.name = item2.name;
            //it
            item1.ammoNum = item2.ammoNum;
            item1.SetWeapon(item1.name);
            item2.name = "";
            item2.ammoNum = 0;
            item2.maxNum = 0;
            SaveWeaponInfo(item1.slot, item1.weaponName, item1.ammoNum);
            item2.Init();
        }
        */



        int tNum;
        string tName;
        string tNom;

        tName = item2.weaponName;
        tNum = item2.ammoNum;
        tNom = item2.name;

        SaveWeaponInfo(item1.slot, tName, tNum);
        SaveWeaponInfo(item2.slot, item1.weaponName, item1.ammoNum);
        
        item2.weaponName = item1.weaponName;
        item2.SetWeapon(item1.weaponName);
        item2.ammoNum = item1.ammoNum;

        
        item1.weaponName = tName;
        item1.SetWeapon(tName);
        item1.ammoNum = tNum;


    }


    public void Refill()
    {
        int cost = 100;
        PlayerStats stats = PlayerStats.instance;
        if (stats.money >= cost)
        {
            DragWeapon[] weps = shopCanvas.transform.GetComponentsInChildren<DragWeapon>();
            foreach (DragWeapon wep in weps)
            {
                if (!wep.selling)
                {
                    wep.ammoNum = wep.maxNum;
                    SaveWeaponInfo(wep.slot, wep.weaponName, wep.ammoNum);
                }
            }
            stats.money -= cost;
            audio.PlayOneShot(refillSound);
        }
        else
        {
            audio.PlayOneShot(failSound);
        }

    }

    public void Repair()
    {
        int cost = 100;
         PlayerStats stats = PlayerStats.instance;
         if ((stats.money >= cost)&&(stats.health < stats.maxHealth))
         {
             stats.money -= cost;
             stats.health = stats.maxHealth;
             audio.PlayOneShot(repairSound);
         }
         else
         {
             audio.PlayOneShot(failSound);
         }
    }

    public void SellGoods()
    {
        int earntMoney = 0;
        PlayerStats stats = PlayerStats.instance;
        foreach (Item it in stats.inventory)
        {
            earntMoney += it.price;
        }
        stats.inventory.Clear();

        stats.money += earntMoney;

        RefreshInventoryList();

        audio.PlayOneShot(sellSound);

    }

    public void Flight()
    {
        PlayerStats stats = PlayerStats.instance;
        stats.spawnPositionIndex = 0;
        stats.spawnInNextScene = true;
        Application.LoadLevel(baseLoc);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SwitchToHangar()
    {
        questWindow.SetActive(false);
        hangarWindow.SetActive(true);
        audio.PlayOneShot(clickSound);
    }

    public void SwitchToQuest()
    {
        questWindow.SetActive(true);
        hangarWindow.SetActive(false);
        audio.PlayOneShot(clickSound);
    }

    public void GetQuest()
    {
        PlayerStats.instance.GetQuest(baseNum);
        
        quest = PlayerStats.instance.GetComponentInChildren<IQuest>();
        SetNextReplica();

    }

    public void QuestTaken()
    {
        hasQuestActive = true;
        audio.PlayOneShot(questTaken);
    }

    public void QuestComplete()
    {
        PlayerStats.instance.SaveToFile(PlayerStats.instance.name);
        audio.PlayOneShot(questComplete);
    }

    public void DialogButton1()
    {
        audio.PlayOneShot(buttonClick);
         if (!PlayerStats.instance.hasActiveQuest)
        {
            
        } else if(quest != null) 
        {
            quest.Anwser1();
            SetNextReplica();
        }
    }

    public void DialogButton2()
    {
        audio.PlayOneShot(buttonClick);
        if (!PlayerStats.instance.hasActiveQuest)
        {
            GetQuest();
        } else if(quest != null) 
        {
            quest.Anwser2();
            SetNextReplica();
        }
    }

    public void DialogButton3()
    {
        audio.PlayOneShot(buttonClick);
        if (!PlayerStats.instance.hasActiveQuest)
        {
            SwitchToHangar();
        } else if(quest != null) 
        {
            quest.Anwser3();
            SetNextReplica();
        }
    }

}