using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SaveInfo")]
public class SaveInfo  {

    public string name = "Mingebag";
    public int money = 150;
    public int health = 1500;
    public int maxHealth = 1500;
    public int maxcargo = 2500;

    public float maxSpeed = 30;
    public float acceleration = 3f;
    public float yawPitchFactor = 1.2f;
    public float rollFactor = 1.2f;

    public string wS1Name = "";
    public int wS1Num = 0;

    public string wS2Name = "";
    public int wS2Num = 0;

    public string[] auxName = new string[4];
    public int[] auxNum = new int[4];

    public bool spawnInNextScene = false;
    public int spawnPositionIndex = 1;
    public bool populateLocations = false;

    public bool spawned = false;
    public bool hasActiveQuest = false;
    public int base1QuestNum = 0;
    public int base2QuestNum = 0;
    public string loadedLevel;

    public bool[] openedLocations = new bool[4];

    [XmlArray("InventoryItems")]
    [XmlArrayItem("Item")]
    public List<Item> inventory = new List<Item>();


    public void Save(string path) {
        var serializer = new XmlSerializer(typeof(SaveInfo));
		using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

    public static SaveInfo Load(string path) {
        var serializer = new XmlSerializer(typeof(SaveInfo));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as SaveInfo;
        }
    }

    public SaveInfo() { }
}
