using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int highScore;

    public int slotSave;
    [SerializeField] SaveLoad saveLoad;

    void Start()
    {
        SaveG(slotSave);
        LoadG(slotSave);
    }

    public void SaveG(int slot)
    {
        Save s = new();
        // Adiconar variaveis a serem salvas
        s.highScore = highScore;
        // --

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
