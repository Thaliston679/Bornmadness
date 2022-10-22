using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{
    public void SaveGame(Save s, string slot)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath;
        FileStream file = File.Create(path + slot);

        bf.Serialize(file, s);
        file.Close();

        Debug.Log("Jogo salvo!");
    }

    public Save LoadGame(string slot)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath;
        FileStream file;

        if (File.Exists(path + slot))
        {
            file = File.Open(path + slot, FileMode.Open);

            Save load = (Save)bf.Deserialize(file);
            file.Close();

            Debug.Log("Jogo carregado!");
            return load;
        }
        Debug.Log("Sem dados salvos!");
        return null;
    }
}
