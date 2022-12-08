using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NameTranfer : MonoBehaviour
{
    public string theName;
    public GameObject inputField;
    public GameObject textDisplay;
    public int conta=0;
    MenuAnimation menu;
    public GameObject[] saves;
    public GameObject[] savesSes;
    public TMP_Text[] savesTexts;

    void Start()
    {
        menu = GetComponent<MenuAnimation>();
    }
    public void StoreName(int save)
    {      
        if(conta<1)
        {
            theName = inputField.GetComponent<TMP_Text>().text;
            textDisplay.GetComponent<TMP_Text>().text = "" + theName + "";
        }    
    }

    public void SelectSave(int save)
    {
        if(conta<1)
        {
            saves[save].SetActive(true);
        }
        else if(conta==1)
        {
            savesSes[save].SetActive(true);
        }
    }

    public void DeleteSave(int save)
    {
        conta=0;
        theName = "";
    }

    public void HideSave(int save)
    {
        saves[save].SetActive(false);
    }

    public void NameSave()
    {
        conta = 1;
    }
}
