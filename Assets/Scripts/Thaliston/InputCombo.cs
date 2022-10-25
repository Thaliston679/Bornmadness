using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCombo : MonoBehaviour
{
    [SerializeField] private int tapCounts;
    [SerializeField] private float tapTimer;
    [SerializeField] private float currentTapTimer;

    private void Update()
    {
        if(currentTapTimer <= tapTimer) //Aciona contador do combo
        {
            currentTapTimer += Time.deltaTime;
        }

        if(currentTapTimer >= tapTimer)  //Se passar muito tempo, reseta o combo
        {
            tapCounts = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q)) //Aplica o golpe, adiciona 1 à variavel do combo e reseta o tempo entre os golpes do combo
        {
            currentTapTimer = 0;
            tapCounts++;

            //Chamar golpe do combo de acordo com a variavel tapCounts Ex: animAtck[tapCounts];
        }

        if (tapCounts >= 5/*(Máximo de golpes no combo)*/)
        {
            tapCounts = 0;
        }
    }
}