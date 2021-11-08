using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Footer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (MenuManager.Instance.canSwitchMenu && MenuManager.Instance.actualMenuOn != Menu.MAIN)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                print("continue");
                StartCoroutine(Continue());
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                print("back");
                StartCoroutine(Return());
            }
        }
      
    }

    IEnumerator Continue()
    {
        MenuManager.Instance.canSwitchMenu = false;
        switch (MenuManager.Instance.actualMenuOn)
        {
            case Menu.CHARACTER:
                MenuManager.Instance.OpenMenuFromFooter(Menu.MAP);
                break;
            case Menu.MAP:
                //MenuManager.Instance.OpenMenuFromFooter(Menu.GAME);
                break;
        }
        yield return new WaitForSeconds(MenuManager.Instance.switchMenuDuration);
        MenuManager.Instance.canSwitchMenu = true;
    }

    IEnumerator Return()
    {
        MenuManager.Instance.canSwitchMenu = false;
        MenuManager.Instance.lastMenu = MenuManager.Instance.actualMenuOn;

        switch (MenuManager.Instance.actualMenuOn)
        {
            case Menu.OPTIONS:
                MenuManager.Instance.actualMenuOn = Menu.MAIN;
                MenuManager.Instance.OpenMainMenu();
                break;
            case Menu.CHARACTER:
                MenuManager.Instance.actualMenuOn = Menu.MAIN;
                MenuManager.Instance.OpenMainMenu();
                break;
            case Menu.MAP:
                MenuManager.Instance.actualMenuOn = Menu.CHARACTER;
                MenuManager.Instance.OpenMenuFromFooter(Menu.CHARACTER);
                break;
            case Menu.TUTO:
                MenuManager.Instance.actualMenuOn = Menu.MAIN;
                MenuManager.Instance.OpenMainMenu();
                break;
        }
        yield return new WaitForSeconds(MenuManager.Instance.switchMenuDuration);
        MenuManager.Instance.canSwitchMenu = true;
    }
}
