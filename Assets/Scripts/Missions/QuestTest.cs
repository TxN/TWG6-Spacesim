using UnityEngine;
using System.Collections;

public class QuestTest : MonoBehaviour, IQuest 
{
    public static string name = "QuestTest";
    public static string title = "Тестовый квест";
    public static string description = "Это тестовый квест. Тебе надо будет притащить сюда ящик, валяющийся возле разбитого корабля неподалеку от нашей базы. Берешься?";

    public static string acceptAnwser1 = "Окей, берусь. Что мне за это будет?";
    public static string acceptAnwser2 = "Дай мне отдохнуть. Я только что с рейда.";
    public static string acceptAnwser3 = "...";

    public static QuestTextData questData;

    public bool active = true;
    bool questAccepted = false;
    bool goalAchieved;

    int prevReplica = 0;
    int currentReplica = 0;
    int endReplica = 5;
    int waitReplica = 6;


    public GameObject questBox;
    public string questBoxName = "CargoBox";
    public Vector3 boxPos = new Vector3(-47,-34,-313);

    void Awake()
    {
        
        questData = QuestTextData.Load(Application.dataPath + "/QuestData/" + name + ".xml");
    }

    void Update()
    {

    }

    void OnLevelWasLoaded(int level)
    {
        string curLevel = Application.loadedLevelName;
        string trgLevel = "1";

        if (!questAccepted)
        {
            PlayerStats.instance.hasActiveQuest = false;
            Destroy(this.gameObject);
            return;
        }

        if (curLevel == trgLevel)
        {
            GUIManager.instance.AddChatMessage(new ChatMessage("Подберите ящик с запчастями (Клавиша F)", Color.yellow));
            GUIManager.instance.AddChatMessage(new ChatMessage("Квестовые предметы отмечаются зелеными метками на экране.", Color.yellow));
            GameObject box = (GameObject) Instantiate(Resources.Load(questBoxName) as GameObject);
            box.transform.position = boxPos;
            questBox = box;
        }

        string baseLevel = "OurBase";
        if (curLevel == baseLevel)
        {
            if (PlayerStats.instance.RemoveAllInInventory(questBoxName) > 0)
            {
                goalAchieved = true;
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
        Debug.Log("Getting Anwser");
        string anwser = "";
        int anwserReplica = 0;
        if (currentReplica >= 0)
        {
            anwserReplica = currentReplica;
        } else if(currentReplica == -2) 
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

    public string GetQuestTitle()
    {
        return title;
    }
    public string GetQuestDescription()
    {
        return description;
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
        else if(currentReplica == -2)
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
    }

   public bool IsQuestActive()
    {
        return active;
    }

   public void GoalAchieved()
   {
       goalAchieved = true;
   }

}
