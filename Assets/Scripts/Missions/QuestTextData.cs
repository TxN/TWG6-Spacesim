using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("QuestData")]
public class QuestTextData
{
    [XmlArray("Replicas")]
 	[XmlArrayItem("Replica")]
    public List<Replica> Replicas = new List<Replica>();

    public void Save(string path)
    {
        //var encoding = Encoding.GetEncoding("UTF-8");
        var serializer = new XmlSerializer(typeof(QuestTextData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

    public static QuestTextData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(QuestTextData));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as QuestTextData;
        }
    }

    public QuestTextData()
    {

    }
}
