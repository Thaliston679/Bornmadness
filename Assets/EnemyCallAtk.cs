using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallAtk : MonoBehaviour
{
    public GameObject atkBox;
    public void MeleeAtkActive()
    {
        atkBox.SetActive(true);
    }

    public void MeleeAtkDesactive()
    {
        atkBox.SetActive(false);
    }
}
