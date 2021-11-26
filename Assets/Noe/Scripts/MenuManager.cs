using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    
    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();
            }

            return _instance;
        }
    }
    public bool canSwitchMenu = true;
    public GameObject main, charac, map, options, credits, tuto, playButton, generalSlider;
    public Menu actualMenuOn, lastMenu;
    public EventSystem eventsys;
    [Range(0.1f, 2)]
    public float switchMenuDuration;
    public Rewired.Player player;
    bool navLock = false;

    private void Start()
    {
        actualMenuOn = Menu.MAIN;
        lastMenu = Menu.MAIN;
        player = ReInput.players.GetPlayer(0);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject groupMenu = canvas.transform.Find("MenuGroup").gameObject;

        main = groupMenu.transform.Find("MainMenu").gameObject;
        charac = groupMenu.transform.Find("ChooseCharacterMenu").gameObject;
        map = groupMenu.transform.Find("ChooseMapMenu").gameObject;
        options = groupMenu.transform.Find("OptionMenu").gameObject;
        tuto = groupMenu.transform.Find("TutorialMenu").gameObject;
        //credits = groupMenu.transform.Find("CreditsMenu").gameObject;
    }

    private void Update()
    {
        float nav = player.GetAxisRaw("MoveHorizontal");
        if (navLock && nav == 0) navLock = false;
        if (actualMenuOn == Menu.MAIN && !navLock)
        {
            if (nav > 0)
            {
                if (eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight().gameObject);
                navLock = true;
            }
            else if (nav < 0)
            {
                if (eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnLeft())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnLeft().gameObject);
                navLock = true;
            }

            if (player.GetButtonDown("Attack"))
                eventsys.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();

        }
    }


    public void OpenMenu()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            lastMenu = actualMenuOn;
            Menu menu = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<MenuSelector>().destination;
            actualMenuOn = menu;
            switch (menu)
            {
                case Menu.MAIN:
                    OpenMainMenu();                    
                    break;
                case Menu.CHARACTER:
                    eventsys.SetSelectedGameObject(null);
                    Camera.main.transform.DOMove((Vector2)charac.transform.position , switchMenuDuration);
                    break;
                case Menu.MAP:
                    eventsys.SetSelectedGameObject(null);
                    Camera.main.transform.DOMove((Vector2)map.transform.position , switchMenuDuration);
                    break;
                case Menu.OPTIONS:
                    OpenOptions();
                    break;
                case Menu.CREDITS:
                    eventsys.SetSelectedGameObject(null);
                    SceneManager.LoadScene(1);
                    //Camera.main.transform.DOMove((Vector2)credits.transform.position, switchMenuDuration);
                    break;
                case Menu.TUTO:
                    eventsys.SetSelectedGameObject(null);
                    Camera.main.transform.DOMove((Vector2)tuto.transform.position, switchMenuDuration);
                    break;
                case Menu.QUIT:
                    //anim de quit?
                    Application.Quit();
                    break;
                default:
                    Debug.LogError("No Map Selected. . .");
                    break;
            }
        }
    }
    public void OpenMenuFromFooter(Menu menu)
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            lastMenu = actualMenuOn;
            actualMenuOn = menu;
            switch (menu)
            {
                case Menu.MAIN:
                    OpenMainMenu();
                    break;
                case Menu.CHARACTER:
                    Camera.main.transform.DOMove((Vector2)charac.transform.position , switchMenuDuration);
                    break;
                case Menu.MAP:
                    if (lastMenu == Menu.CHARACTER)
                        Camera.main.transform.DOMove((Vector2)map.transform.position, switchMenuDuration);
                    break;
                case Menu.OPTIONS:
                    OpenOptions();
                    break;
                case Menu.CREDITS:
                    SceneManager.LoadScene("Credits");
                    break;
                case Menu.TUTO:
                    Camera.main.transform.DOMove((Vector2)tuto.transform.position, switchMenuDuration);
                    break;
                case Menu.QUIT:
                    Application.Quit();
                    break;
                default:
                    Debug.LogError("No Map Selected. . .");
                    break;
            }
        }
    }

    public void OpenMainMenu()
    {
        Camera.main.transform.DOMove((Vector2)main.transform.position, switchMenuDuration);
        eventsys.SetSelectedGameObject(playButton);
    }

    public void OpenOptions()
    {
        Camera.main.transform.DOMove((Vector2)options.transform.position, switchMenuDuration);
        eventsys.SetSelectedGameObject(generalSlider);
    }
}
