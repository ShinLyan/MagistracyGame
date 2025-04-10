using UnityEngine;
using System.Xml.Serialization; // ������ � ������ XML �����
using System.IO;

/// <summary> ������.</summary>
/// <remarks> ����� ��������������� � XML ������, � ������ ��������� ������ � �����.</remarks>
[XmlRoot("dialogue")]
public class Dialogue
{
    /// <summary> ����.</summary>
    [XmlElement("node")]
    public Node[] Nodes { get; set; }

    /// <summary> ��������� �������.</summary>
    /// <param name="xml"> XML ����.</param>
    /// <returns> ���������� �������.</returns>
    public static Dialogue Load(TextAsset xml)
    {
        var serializer = new XmlSerializer(typeof(Dialogue));
        var reader = new StringReader(xml.text);
        var dialogue = serializer.Deserialize(reader) as Dialogue;
        return dialogue;
    }

    /// <summary> ����.</summary>
    /// <remarks> ����� ������������ ����� ����� �������.</remarks>
    [System.Serializable]
    public class Node
    {
        /// <summary> ����� �������.</summary>
        [XmlAttribute("end")]
        public bool IsEnd { get; set; }

        /// <summary> ����� ����.</summary>
        [XmlElement("text")]
        public string Text { get; set; }
    }
}