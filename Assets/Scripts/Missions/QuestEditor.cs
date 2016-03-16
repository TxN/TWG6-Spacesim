using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestEditor : MonoBehaviour {
    public InputField filename;

    public QuestTextData quest;

    public InputField replicaText;
    public InputField replicaAnwser1;
    public InputField replicaAnwser2;
    public InputField replicaAnwser3;
    public InputField replicaGoto1;
    public InputField replicaGoto2;
    public InputField replicaGoto3;

    public Replica currentReplica;
    public int replicaNum = 0;

    public Text numText;

    void Awake()
    {
        quest = new QuestTextData();
    }

    void Update()
    {
        numText.text = replicaNum.ToString();
    }

    public void AddReplica()
    {
        Replica replica = new Replica();
        replica.text = replicaText.text;
        replica.anwser1 = replicaAnwser1.text;
        replica.anwser2 = replicaAnwser2.text;
        replica.anwser3 = replicaAnwser3.text;
        replica.number = replicaNum;
        replica.num1 = int.Parse(replicaGoto1.text);
        replica.num2 = int.Parse(replicaGoto2.text);
        replica.num3 = int.Parse(replicaGoto3.text);

        replicaText.text ="";
        replicaAnwser1.text = ""; 
        replicaAnwser2.text = ""; 
        replicaAnwser3.text = ""; 
        replicaGoto1.text = ""; 
        replicaGoto2.text = ""; 
        replicaGoto3.text = ""; 

        quest.Replicas.Add(replica);

        replicaNum++;
    }

    public void Submit()
    {
        quest.Save("QuestData/" + filename.text + ".xml");
        quest = new QuestTextData();
        replicaNum = 0;
    }

    public void Load()
    {
        quest = QuestTextData.Load("QuestData/" + filename.text + ".xml");
        replicaNum = 0;
        ShowReplica(replicaNum);
    }

    public void ShowReplica(int num)
    {
        if (num < quest.Replicas.Count)
        {
            replicaText.text = quest.Replicas[num].text;
            replicaAnwser1.text = quest.Replicas[num].anwser1;
            replicaAnwser2.text = quest.Replicas[num].anwser2;
            replicaAnwser3.text = quest.Replicas[num].anwser3;
            replicaGoto1.text = quest.Replicas[num].num1.ToString();
            replicaGoto2.text = quest.Replicas[num].num2.ToString();
            replicaGoto3.text = quest.Replicas[num].num3.ToString();
        }
        else
        {
            replicaText.text = "";
            replicaAnwser1.text = "";
            replicaAnwser2.text = "";
            replicaAnwser3.text = "";
            replicaGoto1.text = "";
            replicaGoto2.text = "";
            replicaGoto3.text = ""; 
        }
      
    }

    public void NextReplica()
    {
        replicaNum++;
        ShowReplica(replicaNum);

    }

    public void PrevReplica()
    {
        replicaNum--;
        ShowReplica(replicaNum);
    }
	
}
