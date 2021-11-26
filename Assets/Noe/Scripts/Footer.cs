using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Rewired;

public class Footer : MonoBehaviour
{

    public Rewired.Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }
    void Update()
    {
        if (MenuManager.Instance.canSwitchMenu && MenuManager.Instance.actualMenuOn != Menu.MAIN)
        {
            if (player.GetButtonDown("Attack"))
                StartCoroutine(Continue());
            if (player.GetButtonDown("MenuCan"))
                StartCoroutine(Return());
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
                try
                {
                    SceneManager.LoadScene(MenuManager.Instance.map.transform.GetChild(1).GetComponent<Selector>().SelectMap());
                }
                catch
                {
                    Debug.LogError("Could not load map : scene was not found. Verify name of the scene & the name of the map GameObject");
                }
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
