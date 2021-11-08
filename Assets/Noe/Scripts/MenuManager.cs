using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    public float Xoffset, Yoffset;
    public Menu actualMenuOn, lastMenu;
    public EventSystem eventsys;
    [Range(0.1f, 2)]
    public float switchMenuDuration;
    [SerializeField]
    private Vector3 cameraPos = new Vector3(0, 0 , -10);

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        actualMenuOn = Menu.MAIN;
        lastMenu = Menu.MAIN;

        GameObject canvas = GameObject.Find("Canvas");
        GameObject groupMenu = canvas.transform.Find("MenuGroup").gameObject;
        eventsys = EventSystem.current;

        main = groupMenu.transform.Find("MainMenu").gameObject;
        charac = groupMenu.transform.Find("ChooseCharacterMenu").gameObject;
        map = groupMenu.transform.Find("ChooseMapMenu").gameObject;
        options = groupMenu.transform.Find("OptionMenu").gameObject;
        tuto = groupMenu.transform.Find("TutorialMenu").gameObject;
        //credits = groupMenu.transform.Find("CreditsMenu").gameObject;
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
                    Camera.main.transform.DOMove((Vector2)charac.transform.position , switchMenuDuration);
                    break;
                case Menu.MAP:
                    if(actualMenuOn == Menu.CHARACTER)
                        Camera.main.transform.DOMove((Vector2)map.transform.position , switchMenuDuration);
                    break;
                case Menu.OPTIONS:
                    OpenOptions();
                    break;
                case Menu.CREDITS:
                    //Camera.main.transform.DOMove(credits.transform.position, switchMenuDuration);
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
                    //Camera.main.transform.DOMove(credits.transform.position, switchMenuDuration);
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
