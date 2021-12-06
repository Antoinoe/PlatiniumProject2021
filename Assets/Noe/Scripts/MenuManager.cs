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
        Debug.Log(ReInput.controllers.joystickCount);
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            //Debug.Log(ReInput.controllers.GetControllerCount(ControllerType.Joystick));
            ReInput.players.Players[i].controllers.AddController(ReInput.controllers.Joysticks[i], true);
            Debug.Log(ReInput.players.GetPlayer(i).controllers.GetController(ControllerType.Joystick, i).hardwareName);
        }
        player = ReInput.players.GetPlayer(0);
        Debug.Log("Player in charge is:" + player.controllers.GetController(ControllerType.Joystick, 0).hardwareName);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject groupMenu = canvas.transform.Find("MenuGroup").gameObject;

        main = groupMenu.transform.Find("MainMenu").gameObject;
        charac = groupMenu.transform.Find("ChooseCharacterMenu").gameObject;
        map = groupMenu.transform.Find("ChooseMapMenu").gameObject;
        options = groupMenu.transform.Find("OptionMenu").gameObject;
        tuto = groupMenu.transform.Find("TutorialMenu").gameObject;
        //credits = groupMenu.transform.Find("CreditsMenu").gameObject;
        ReInput.ControllerConnectedEvent += OnControllerConnected;
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

    private static void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        ReInput.players.Players[ReInput.controllers.joystickCount - 1].controllers.AddController(ReInput.controllers.Joysticks[ReInput.controllers.joystickCount - 1], true);
        Debug.Log(ReInput.controllers.GetControllerCount(ControllerType.Joystick));
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
                    Camera.main.transform.DOMove((Vector2)charac.transform.position, switchMenuDuration);
                    break;
                case Menu.MAP:
                    eventsys.SetSelectedGameObject(null);
                    Camera.main.transform.DOMove((Vector2)map.transform.position, switchMenuDuration);
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
                    Camera.main.transform.DOMove((Vector2)charac.transform.position, switchMenuDuration);
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
