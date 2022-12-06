using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Variaveis salvas
    public bool[] cristaisLevel1;
    public bool[] cristaisLevel2;
    public bool[] cristaisLevel3;
    public bool[] cristaisLevel4;
    public bool[] cristaisLevel5;
    public int hpMax;
    public float forceMax;
    public int goToLevel;
    public int goToPoint;
    //
    /*cristaisLevel1;
    cristaisLevel2;
    cristaisLevel3;
    cristaisLevel4;
    cristaisLevel5;
    hpMax;
    forceMax;
    goToLevel;
    goToPoint;*/

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

        s.cristaisLevel1 = cristaisLevel1;
        s.cristaisLevel2 = cristaisLevel2;
        s.cristaisLevel3 = cristaisLevel3;
        s.cristaisLevel4 = cristaisLevel4;
        s.cristaisLevel5 = cristaisLevel5;
        s.hpMax = hpMax;
        s.forceMax = forceMax;
        s.goToLevel = goToLevel;
        s.goToPoint = goToPoint;

        // --

        string i = "/savegameslot"+slot.ToString()+".save";

        saveLoad.SaveGame(s,i);
    }

    public void LoadG(int slot)
    {
        string slotS = "/savegameslot" + slot.ToString() + ".save";

        Save load = saveLoad.LoadGame(slotS);

        if (load != null) //Caso tiver algum valor salvo, carrega ele no jogo
        {
            cristaisLevel1 = load.cristaisLevel1;
            cristaisLevel2 = load.cristaisLevel2;
            cristaisLevel3 = load.cristaisLevel3;
            cristaisLevel4 = load.cristaisLevel4;
            cristaisLevel5 = load.cristaisLevel5;
            hpMax = load.hpMax;
            forceMax = load.forceMax;
            goToLevel = load.goToLevel;
            goToPoint = load.goToPoint;
        }
        else //Caso não tiver nenhum valor salvo, põe valores padrão no jogo
        {
            cristaisLevel1 = new bool[500];
            cristaisLevel2 = new bool[500];
            cristaisLevel3 = new bool[500];
            cristaisLevel4 = new bool[500];
            cristaisLevel5 = new bool[500];
            hpMax = 10;
            forceMax = 1;
            goToLevel = 1;
            goToPoint = 1;
        }
    }
}
