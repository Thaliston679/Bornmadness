using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NameTranfer : MonoBehaviour
{
    public string theName;

    public string nomeSave1;
    public string nomeSave2;
    public string nomeSave3;

    public int contaSave1;
    public int contaSave2;
    public int contaSave3;

    public GameObject inputField;
    public GameObject textDisplay;

    MenuAnimation menu;
    public GameObject[] saves;
    public GameObject[] savesSes;
    public TMP_Text saveText1;

    public int slotSave;
    [SerializeField] SaveLoad saveLoad;

    void Start()
    {
        SaveG(slotSave);
        LoadG(slotSave);
        
        menu = GetComponent<MenuAnimation>();

        UpdateSave();
    }
    public void UpdateSave()
    {
        saveText1.text = $"Save1: {nomeSave1}";
    }
    public void StoreName(int save)
    {      
        if(contaSave1 < 1)
        {
            nomeSave1 = inputField.GetComponent<TMP_Text>().text;
            textDisplay.GetComponent<TMP_Text>().text = "" + nomeSave1 + "";
        }    
    }

    public void SelectSave(int save)
    {
        if(contaSave1 < 1)
        {
            saves[save].SetActive(true);
        }
        else if(contaSave1 == 1)
        {
            savesSes[save].SetActive(true);
        }
    }

    public void DeleteSave(int save)
    {
        contaSave1 = 0;
        nomeSave1 = "";
    }

    public void HideSave(int save)
    {
        saves[save].SetActive(false);
    }

    public void NameSave()
    {
        contaSave1 = 1;
    }

    public void SaveG(int slot)
    {
        Save s = new();
        // Adiconar variaveis a serem salvas
        s.nomeSave1 = nomeSave1;
        s.nomeSave2 = nomeSave2;
        s.nomeSave3 = nomeSave3;
        s.contaSave1 = contaSave1;
        s.contaSave2 = contaSave2;
        s.contaSave3 = contaSave3;

    // --

    string i = "/savegameslot" + slot.ToString() + ".save";

        saveLoad.SaveGame(s, i);
    }

    public void LoadG(int slot)
    {
        string slotS = "/savegameslot" + slot.ToString() + ".save";

        Save load = saveLoad.LoadGame(slotS);

        if (load != null) //Caso tiver algum valor salvo, carrega ele no jogo
        {
            nomeSave1 = load.nomeSave1;
            nomeSave2 = load.nomeSave2;
            nomeSave3 = load.nomeSave3;
            contaSave1 = load.contaSave1;
            contaSave2 = load.contaSave2;
            contaSave3 = load.contaSave3;
        }
        else //Caso não tiver nenhum valor salvo, põe valores padrão no jogo
        {
            nomeSave1 = "";
            nomeSave2 = "";
            nomeSave3 = "";
            contaSave1 = 0;
            contaSave2 = 0;
            contaSave3 = 0;
        }
    }
}
