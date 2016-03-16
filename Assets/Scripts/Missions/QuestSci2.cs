using UnityEngine;
using System.Collections;

public class QuestSci2 : MonoBehaviour, IQuest 
{
    public static QuestTextData questData;
    public string questName = "QuestSci-2";
    int reward = 750;
    public bool active = true;
    bool questAccepted = false;
    bool goalAchieved;

    int prevReplica = 0;
    int currentReplica = 0;
    int endReplica = 4;
    int waitReplica = 3;


    public GameObject questBox;

    public string questBoxName = "SpySat";


    void Awake()
    {
        questData = QuestTextData.Load(Application.dataPath + "/QuestData/" + questName + ".xml");
    }

    void OnLevelWasLoaded(int level)
    {
        string curLevel = Application.loadedLevelName;
        string trgLevel = "CuboidSector";


        if (!questAccepted)
        {
            PlayerStats.instance.hasActiveQuest = false;
            Destroy(this.gameObject);
            return;
        }

        if (curLevel == trgLevel)
        {
            GUIManager.instance.AddChatMessage(new ChatMessage("Вы в секторе кубоидов. Сбрасывайте спутник, и драпайте отсюда!", Color.yellow));
            GUIManager.instance.AddChatMessage(new ChatMessage("(Выбросить груз - клавиша G)", Color.yellow));

        }


        string baseLevel = "ScienceBase";
        if (curLevel == baseLevel)
        {

            if (goalAchieved)
            {
                PlayerStats.instance.money += reward;
                currentReplica = endReplica;
            }
        }


    }

    public string GetReplica()
    {
        if (currentReplica >= 0)
        {
            return questData.Replicas[currentReplica].text;
        }
        else
        {
            if (currentReplica == -2)
            {
                return questData.Replicas[waitReplica].text;
            }

            if (currentReplica == -3)
            {
                return questData.Replicas[endReplica].text;
            }

            return "...";
        }

    }

    public string GetAnwser(int num)
    {
        string anwser = "";
        int anwserReplica = 0;
        if (currentReplica >= 0)
        {
            anwserReplica = currentReplica;
        }
        else if (currentReplica == -2)
        {
            anwserReplica = waitReplica;
        }
        else if (currentReplica == -3)
        {
            anwserReplica = endReplica;
        }

        if (num == 1)
        {
            anwser = questData.Replicas[anwserReplica].anwser1;
        }
        if (num == 2)
        {
            anwser = questData.Replicas[anwserReplica].anwser2;
        }
        if (num == 3)
        {
            anwser = questData.Replicas[anwserReplica].anwser3;
        }
        return anwser;
    }


    public string GetQuestName()
    {
        return this.ToString();
    }


    public void Anwser1()
    {
        prevReplica = currentReplica;
        currentReplica = questData.Replicas[currentReplica].num1;
        DoReplicaStuff();
    }

    public void Anwser2()
    {
        prevReplica = currentReplica;
        currentReplica = questData.Replicas[currentReplica].num2;
        DoReplicaStuff();
    }

    public void Anwser3()
    {
        prevReplica = currentReplica;
        currentReplica = questData.Replicas[currentReplica].num3;
        DoReplicaStuff();
    }

    void DoReplicaStuff()
    {
        if (currentReplica == -3)
        {
            QuestCompleted();
        }
        else if (currentReplica == -1)
        {
            currentReplica = prevReplica;
        }
        else if (currentReplica == -2)
        {
            QuestTaken();
        }
    }

    void QuestCompleted()
    {
        PlayerStats.instance.base2QuestNum += 1;
        PlayerStats.instance.hasActiveQuest = false;
        Destroy(this.gameObject);
        MenuGlobal.instance.QuestComplete();
        MenuGlobal.instance.ClearQuestWindow();
        active = false;
    }

    void QuestTaken()
    {
        MenuGlobal.instance.QuestTaken();
        questAccepted = true;

        Item item = new Item();
        item.name = questBoxName;
        item.quantity = 1;
        item.price = 0;
        item.weight = 300;
        item.title = "Шпионский спутник";
        item.desc = "Устаревший разведывательный спутник, использующий продвинутую технологию Стелс";
        PlayerStats.instance.inventory.Add(item);
        MenuGlobal.instance.RefreshInventoryList();
        PlayerStats.instance.populateLocations = true;

        PlayerStats.instance.openedLocations[0] = true;
        PlayerStats.instance.openedLocations[2] = true;
    }

    public bool IsQuestActive()
    {
        return active;
    }

    public void GoalAchieved()
    {
        goalAchieved = !goalAchieved;
        if (goalAchieved)
        {
            GUIManager.instance.AddChatMessage(new ChatMessage("Спутник выгружен. Пора отсюда убегать.", Color.yellow));
        }
        // currentReplica = endReplica;
    }
   

	
}
