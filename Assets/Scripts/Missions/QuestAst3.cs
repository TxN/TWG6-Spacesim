using UnityEngine;
using System.Collections;

public class QuestAst3 : MonoBehaviour, IQuest
{
    public static QuestTextData questData;
    public string questName = "QuestAst-3";
    public int reward = 300;
    public bool active = true;
    public bool questAccepted = false;
    bool goalAchieved;

    int prevReplica = 0;
    int currentReplica = 0;
    int endReplica = 4;
    int waitReplica = 4;

    int questStage = 0;

    void Awake()
    {
        questData = QuestTextData.Load(Application.dataPath + "/QuestData/" + questName + ".xml");
   
    }

    void OnLevelWasLoaded(int level)
    {

        if (!questAccepted)
        {
            PlayerStats.instance.hasActiveQuest = false;
            Destroy(this.gameObject);
            return;
        }

        string curLevel = Application.loadedLevelName;

        if (questStage == 0)
        {
            if (curLevel == "ScienceBase")
            {
                questStage++;
                currentReplica = 5;
            }

            if (curLevel == "1")
            {
                GUIManager.instance.AddChatMessage(new ChatMessage("Вам необходимо совершить гиперпрыжок.", Color.yellow));
                GUIManager.instance.AddChatMessage(new ChatMessage("Разгонитесь до максимальной скорости и нажмите J для выбора локации.", Color.yellow));
            }

            if (curLevel == "ScienceBaseSector")
            {
                GUIManager.instance.AddChatMessage(new ChatMessage("Летите к базе ученых.", Color.yellow));
                GUIManager.instance.AddChatMessage(new ChatMessage("Дружественные строения отображаются желтыми метками на экране.", Color.yellow));
            }
        }

        if (questStage == 1)
        {
            if (curLevel == "OurBase")
            {
                questStage++;
                currentReplica = 11;
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

        PlayerStats.instance.money += reward;
    }

    void QuestTaken()
    {
        MenuGlobal.instance.QuestTaken();
        questAccepted = true;
        PlayerStats.instance.openedLocations[0] = true;
        PlayerStats.instance.openedLocations[1] = true;
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
