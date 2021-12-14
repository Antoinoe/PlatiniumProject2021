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
    public Menu actualMenuOn;
    public EventSystem eventsys;
    public GameObject selectedObject; 
    [Range(0.1f, 2)]
    public float switchMenuDuration;
    public Rewired.Player player;
    bool navLock = false;
    bool navLockVer = false;
    public GameObject[] charaArr;
    public bool[] selectedChara;
    public GameObject[] validateImg;
    public bool switching;
    public bool[] playersWait = { false, false, false, false };

    private void Start()
    {
        Data.isDebug = false;
        FindObjectOfType<AudioManager>().Play("MenuMusic");
        actualMenuOn = Menu.MAIN;
        //Debug.Log(ReInput.controllers.joystickCount);
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            //Debug.Log(ReInput.controllers.GetControllerCount(ControllerType.Joystick));
            ReInput.players.Players[i].controllers.AddController(ReInput.controllers.Joysticks[i], true);
            //Debug.Log(ReInput.players.GetPlayer(i).controllers.GetController(ControllerType.Joystick, i).hardwareName);
            charaArr[i].SetActive(true);   
        }
        selectedChara = new bool[ReInput.controllers.joystickCount];
        for (int i = 0; i < selectedChara.Length; i++)
            selectedChara[i] = false;
        Data.playerNbr = ReInput.controllers.joystickCount;
        Data.SetSprites(ReInput.controllers.joystickCount);
        if (ReInput.controllers.joystickCount > 0)
        {
            player = ReInput.players.GetPlayer(0);
            //Debug.Log("Player in charge is:" + player.controllers.GetController(ControllerType.Joystick, 0).hardwareName);
        }
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
        if (player == null) return;
        float nav = player.GetAxisRaw("MoveHorizontal");
        if (navLock && nav == 0) navLock = false;
        float navVer = player.GetAxisRaw("MoveVertical");
        if (navLockVer && navVer == 0) navLockVer = false;
        if (actualMenuOn == Menu.MAIN && !navLockVer)
        {
            if (navVer > 0)
            {
                FindObjectOfType<AudioManager>().Play("UIUp");
                if (eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnUp())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnUp().gameObject);
                navLockVer = true;
            }
            else if (navVer < 0)
            {
                FindObjectOfType<AudioManager>().Play("UIDown");
                if (eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnDown())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnDown().gameObject);
                navLockVer = true;
            }

            if (player.GetButtonDown("Attack"))
            {
                FindObjectOfType<AudioManager>().Play("UISelect");
                eventsys.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }

        } 
        else if (actualMenuOn == Menu.CHARACTER && !navLock)
        {
            for (int i = 0; i < ReInput.controllers.joystickCount; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("Attack") && !switching)
                {
                    PlayerSelectChara(i);
                }
            }
        }
        else if (actualMenuOn == Menu.OPTIONS && !navLockVer)
        {
            navVer = player.GetAxis("MoveVertical");
            if (navVer > 0.5f)
            {
                if (eventsys.currentSelectedGameObject.GetComponent<Selectable>() && eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp().gameObject);
                navLockVer = true;
            }
            else if (navVer < -0.5f)
            {
                if (eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown())
                    eventsys.SetSelectedGameObject(eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown().gameObject);
                navLockVer = true;
            }

            if (eventsys.currentSelectedGameObject.GetComponent<Slider>())
            {
                if (nav < 0)
                    eventsys.currentSelectedGameObject.GetComponent<Slider>().value -= eventsys.currentSelectedGameObject.GetComponent<Slider>().maxValue / 200;
                else if (nav > 0)
                    eventsys.currentSelectedGameObject.GetComponent<Slider>().value += eventsys.currentSelectedGameObject.GetComponent<Slider>().maxValue / 200;
            }
        }

    }

    void PlayerSelectChara(int _p)
    {
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            if (i != _p)
            {
                Debug.Log(p.it[i] + "  " + p.it[_p]);
                if (selectedChara[i] && p.it[i] == p.it[_p]) return;
            }
        }
        FindObjectOfType<AudioManager>().Play("UISelect");
        validateImg[_p].SetActive(true);
        Data.pSprite[_p] = p.it[_p];
        //Debug.Log(_p + "  " + p.it[_p] + "   " + Data.pSprite[_p]);
        GameObject root = GameObject.Find("PlayersGrid");
        for (int i = 0; i < /*ReInput.controllers.joystickCount*/4; i++)
        {
            if (i != _p)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == p.it[_p])
                    {
                        Image img = root.transform.GetChild(i).GetChild(0).GetChild(j).GetComponent<Image>();
                        //img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
                        Image img2 = img.gameObject.transform.GetChild(0).GetComponent<Image>();
                        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0.5f);
                    }
                }
            }
        }
        selectedChara[_p] = true;
    }

    private static void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        FindObjectOfType<MenuManager>().AddController();
        Debug.Log(ReInput.controllers.GetControllerCount(ControllerType.Joystick));
        FindObjectOfType<MenuManager>().UnlockChara(ReInput.controllers.joystickCount - 1);
    }

    void AddController()
    {
        ReInput.players.Players[ReInput.controllers.joystickCount - 1].controllers.AddController(ReInput.controllers.Joysticks[ReInput.controllers.joystickCount - 1], true);
        Debug.Log(ReInput.controllers.joystickCount);
        if (ReInput.controllers.joystickCount == 1)
        {
            player = ReInput.players.GetPlayer(0);
            Debug.Log("Player in charge is:" + player.controllers.GetController(ControllerType.Joystick, 0).hardwareName);
        }
        Data.playerNbr++;
        System.Array.Resize<bool>(ref selectedChara, ReInput.controllers.joystickCount);
        selectedChara[selectedChara.Length] = false;
        Data.SetSprites(Data.playerNbr);
    }

    void UnlockChara(int i)
    {
        charaArr[i].SetActive(true);
    }

    public void OpenMenu()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            Menu menu = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<MenuSelector>().destination;
            actualMenuOn = menu;
            //print("going to " + menu);
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
                    SceneManager.LoadScene("Credits");
                    break;
                case Menu.TUTO:
                    eventsys.SetSelectedGameObject(null);
                    Camera.main.transform.DOMove((Vector2)tuto.transform.position, switchMenuDuration);
                    break;
                case Menu.QUIT:
                    Application.Quit();
                    break;
                default:
                    Debug.LogError("No Map Selected. . .");
                    break;
            }
            StartCoroutine(Switching());
        }
    }
    public void OpenMenuFromFooter(Menu menu)
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            print("going to " + menu);
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
                    //bool ok = true;
                    //for (int i = 0; i < selectedChara.Length; i++)
                    //{
                    //    if (selectedChara[i])
                    //        ok = false;
                    //}
                    //if (!ok) break;
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
            StartCoroutine(Switching());
        }
    }

    IEnumerator Switching()
    {
        switching = true;
        yield return new WaitForSeconds(switchMenuDuration);
        switching = false;
        yield return null;
    }


    public void OpenMainMenu()
    {
        Camera.main.transform.DOMove((Vector2)main.transform.position, switchMenuDuration);
        eventsys.SetSelectedGameObject(playButton);
        actualMenuOn = Menu.MAIN;
    }

    public void OpenOptions()
    {
        Camera.main.transform.DOMove((Vector2)options.transform.position, switchMenuDuration);
        eventsys.SetSelectedGameObject(generalSlider);
        actualMenuOn = Menu.OPTIONS;
    }
}
