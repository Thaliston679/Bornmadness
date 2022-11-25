using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamDDT : MonoBehaviour
{
    public float speedV;
    private float pV;
    // Update is called once per frame
    [SerializeField]bool cursorIsA;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RotateLook();
        PauseGame();
    }

    void RotateLook()
    {
        pV -= speedV * Input.GetAxis("Mouse Y");
        FixRotation();
        transform.localEulerAngles = new Vector3(pV, 0, 0);
    }

    void FixRotation()
    {
        if (pV > 60)
        {
            pV = 60;
        }
        else if (pV < -60)
        {
            pV = -60;
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableMouse();
        }
        if (Input.GetMouseButtonDown(1))
        {
            DisableMouse();
        }
    }

    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
