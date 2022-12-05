using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    public Animator menuAnim;
    public GameObject menu;
    float tempo;
    public float tempoMax;

    void Start()
    {
        menuAnim = GetComponent<Animator>();
    }

    public void MenuOpen()
    {
        menu.SetActive(true);
        menuAnim.SetTrigger("Open");
    }

    public void MenuClosed()
    {
        menuAnim.SetTrigger("Close");

        tempo += Time.deltaTime;
        if(tempo >= tempoMax)
        {
            menu.SetActive(false);
            tempo = 0;
        }
    }
}
