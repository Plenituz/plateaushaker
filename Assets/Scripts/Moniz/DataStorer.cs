using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataStorer {

    public static void Store(string path, string data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/" + path);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public static string Read(string path)
    {
        path = Application.persistentDataPath + "/" + path;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path, FileMode.Open);
            string data = (string)bf.Deserialize(fs);
            fs.Close();
            return data;
        }
        Debug.LogError("no file at " + path);
        return "null";
    }

    public static void InitFile(string path, string initData)
    {
        if (File.Exists(Application.persistentDataPath + "/" + path))
            return;
        else
            Store(path, initData);
    }
	
}
