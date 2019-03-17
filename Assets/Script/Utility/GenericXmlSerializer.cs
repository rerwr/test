namespace Assets.Scripts.Logic.Utility
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class GenericXmlSerializer
    {
        public static T LoadFromXmlFile<T>(string fileName) where T: class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(fileName, FileMode.Open);
            return (serializer.Deserialize(stream) as T);
        }

        public static T ReadFromXmlString<T>(string xmlString) where T: class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            return (serializer.Deserialize(stream) as T);
        }

        public static void SaveToXmlFile(object obj, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            FileStream stream = new FileStream(fileName, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize((TextWriter) writer, obj);
            stream.Close();
        }

        public static string WriteToXmlString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(textWriter, obj, namespaces);
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
    }
}

