using UnityEngine;
using System.Collections;

public class QuestAst4 : MonoBehaviour, IQuest
{
    public static QuestTextData questData;
    public string questName = "QuestAst-4";
    int reward = 950;
    public bool active = true;
    bool questAccepted = false;
    bool goalAchieved;

    int prevReplica = 0;
    int currentReplica = 0;
    int endReplica = 3;
    int waitReplica = 2;


    public GameObject questBox;
    public string questDeviceName = "FlyByTrigger";
    public Vector3 itemPos = new Vector3(-15, -20, 213);

    void Awake()
    {
        questData = QuestTextData.Load(Application.dataPath + "/QuestData/" + questName + ".xml");
    }

    void OnLevelWasLoaded(int level)
    {
        string curLevel = Application.loadedLevelName;
        string trgLevel = "CuboidBossSector";

        if (!questAccepted)
        {
            PlayerStats.instance.hasActiveQuest = false;
            Destroy(this.gameObject);
            return;
        }

        if (curLevel == trgLevel)
        {
            GameObject trigger = (GameObject)Instantiate(Resources.Load(questDeviceName) as GameObject);
            trigger.transform.position = itemPos;
            questBox = trigger;

        }

        string baseLevel = "OurBase";
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
        PlayerStats.instance.base1QuestNum += 1;
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
        PlayerStats.instance.openedLocations[3] = true;
        PlayerStats.instance.populateLocations = true;
    }

    public bool IsQuestActive()
    {
        return active;
    }

    public void GoalAchieved()
    {
        goalAchieved = true;
        GUIManager.instance.AddChatMessage(new ChatMessage("Вы пролетели достаточно близко от материнского корабля кубоидов.", Color.yellow));
        GUIManager.instance.AddChatMessage(new ChatMessage("Теперь возвращайтесь в домашний сектор!", Color.yellow));
    }
   

}
