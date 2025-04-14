using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("dialogue")]
public class Dialogue
{
    [XmlElement("node")]
    public Node[] Nodes { get; set; }

    public static Dialogue Load(TextAsset xml)
    {
        var serializer = new XmlSerializer(typeof(Dialogue));
        var reader = new StringReader(xml.text);
        var dialogue = serializer.Deserialize(reader) as Dialogue;
        return dialogue;
    }

    [Serializable]
    public class Node
    {
        [XmlAttribute("end")]
        public bool IsEnd { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }
    }
}