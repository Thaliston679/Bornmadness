using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int slotSave;
    int highScore;
    [SerializeField]
    SaveLoad saveLoad;
    // Start is called before the first frame update
    void Start()
    {
        SaveG(slotSave);
        LoadG(slotSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveG(int slot)
    {
        Save s = new();
        //
        s.highScore = highScore;
        //

        string i = "/savegameslot"+slot.ToString()+".save";

        saveLoad.SaveGame(s,i);

        Debug.Log("HighScore = " + highScore);
    }

    public void LoadG(int slot)
    {
        string slotS = "/savegameslot" + slot.ToString() + ".save";

        Save load = saveLoad.LoadGame(slotS);

        if (load != null) //Caso tiver algum valor salvo, carrega ele no jogo
        {
            highScore = load.highScore;
        }
        else //Caso não tiver nenhum valor salvo, põe valores padrão no jogo
        {
            highScore = 0;
        }

        Debug.Log("HighScore = " + highScore);
    }
}
