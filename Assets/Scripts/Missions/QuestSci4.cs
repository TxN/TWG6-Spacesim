using UnityEngine;
using System.Collections;

public class QuestSci4 : MonoBehaviour, IQuest 
{
    public static QuestTextData questData;
    public string questName = "QuestSci-4";
    int reward = 2500;
    public bool active = true;
    bool questAccepted = false;
    bool goalAchieved;

    int prevReplica = 0;
    int currentReplica = 0;
    int endReplica = 4;
    int waitReplica = 3;

    int collectedReactors = 0;
    int collectedGrabbers = 0;

    void Awake()
    {
        questData = QuestTextData.Load(Application.dataPath + "/QuestData/" + questName + ".xml");
    }

    void OnLevelWasLoaded(int level)
    {
        string curLevel = Application.loadedLevelName;

        if (!questAccepted)
        {
            PlayerStats.instance.hasActiveQuest = false;
            Destroy(this.gameObject);
            return;
        }

        if (curLevel == "ScienceBaseSector")
        {
            if (collectedReactors < 2)
            {
                GUIManager.instance.AddChatMessage(new ChatMessage("Вам осталось добыть " + (2 - collectedReactors).ToString() + " реактора", Color.yellow));
            }

            if (collectedGrabbers < 5)
            {
                GUIManager.instance.AddChatMessage(new ChatMessage("Вам осталось добыть " + (5 - collectedGrabbers).ToString() + " грабберов", Color.yellow));
            }
        }

        string baseLevel = "ScienceBase";
        if (curLevel == baseLevel)
        {
            if (collectedReactors < 2)
            {
                collectedReactors += PlayerStats.instance.RemoveAllInInventory("Equipment1");
            }

            if (collectedGrabbers < 5)
            {
                collectedGrabbers += PlayerStats.instance.RemoveAllInInventory("Equipment2");
            }

            if((collectedGrabbers >=5)&&(collectedReactors >= 2))
            {
                goalAchieved = true;
            }

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
    }

    public bool IsQuestActive()
    {
        return active;
    }

    public void GoalAchieved()
    {
        goalAchieved = true;
        // currentReplica = endReplica;
    }
   

}
