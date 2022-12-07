using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameTranfer : MonoBehaviour
{
    public string theName;
    public GameObject inputField;
    public GameObject textDisplay;
    private int conta=0;
    MenuAnimation menu;
    public GameObject[] saves;
    public Text[] savesTexts;

    void Start()
    {
        menu = GetComponent<MenuAnimation>();
    }
    public void StoreName(int save)
    {      
            theName = inputField.GetComponent<Text>().text;
            textDisplay.GetComponent<Text>().text = "Então " + theName + " é o seu nome?";
            conta=1;
    }

    public void SelectSave(int save)
    {
        saves[save].SetActive(true);
    }

    public void HideSave(int save)
    {
        saves[save].SetActive(false);
    }
}
