using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendingCode : MonoBehaviour
{
    public GameObject HUD;
    public GameObject ShopMenu;
    public GameObject openMenuPopUp;
    public bool playerClose = false;
    public bool shopOpened = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) playerClose = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9) playerClose = false;
    }

    private void Update()
    {
        if (playerClose && !shopOpened) 
        {
            openMenuPopUp.active = true;
        }
        else
        {
            openMenuPopUp.active = false;
        }

        if (playerClose && !shopOpened && Input.GetKeyDown(KeyCode.B)) { 
            ShopState(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (!playerClose || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.B)) { 
            ShopState(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void ShopState(bool state)
    {
        shopOpened = state;
        ShopMenu.active = state;
        HUD.active = !state;
    }
}
