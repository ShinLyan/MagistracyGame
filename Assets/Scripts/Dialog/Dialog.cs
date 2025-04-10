using UnityEngine;
using System.Xml.Serialization; // Запись и чтение XML файла
using System.IO;

/// <summary> Диалог.</summary>
/// <remarks> Класс взаимодействует с XML файлом, а именно считывает данные с файла.</remarks>
[XmlRoot("dialogue")]
public class Dialogue
{
    /// <summary> Узлы.</summary>
    [XmlElement("node")]
    public Node[] Nodes { get; set; }

    /// <summary> Загрузить диалоги.</summary>
    /// <param name="xml"> XML файл.</param>
    /// <returns> Возвращает диалоги.</returns>
    public static Dialogue Load(TextAsset xml)
    {
        var serializer = new XmlSerializer(typeof(Dialogue));
        var reader = new StringReader(xml.text);
        var dialogue = serializer.Deserialize(reader) as Dialogue;
        return dialogue;
    }

    /// <summary> Узел.</summary>
    /// <remarks> Класс представляет собой часть диалога.</remarks>
    [System.Serializable]
    public class Node
    {
        /// <summary> Конец диалога.</summary>
        [XmlAttribute("end")]
        public bool IsEnd { get; set; }

        /// <summary> Текст узла.</summary>
        [XmlElement("text")]
        public string Text { get; set; }
    }
}