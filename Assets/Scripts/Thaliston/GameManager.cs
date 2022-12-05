using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    int highScore;

    public int slotSave;
    [SerializeField] SaveLoad saveLoad;

    public GameObject mainCrystals;
    public GameObject mainEnemies;
    public int crystalsTotal;
    public int enemiesTotal;
    public int crystalsCurrent;
    public int enemiesCurrent;

    public TextMeshProUGUI cristaisC;
    public TextMeshProUGUI enemiesC;

    void Start()
    {
        SaveG(slotSave);
        LoadG(slotSave);
        enemiesTotal = GameObject.FindGameObjectsWithTag("Enemy").Length;
        crystalsTotal = GameObject.FindGameObjectsWithTag("cristal").Length;
        Debug.Log(enemiesTotal);
        Debug.Log(crystalsTotal);
        UpdateEnemCris();
    }

    public void UpdateEnemCris()
    {
        cristaisC.text = $"Cristais: {crystalsCurrent} / {crystalsTotal}";
        enemiesC.text = $"Inimigos derrotados: {enemiesCurrent} / {enemiesTotal}";
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
