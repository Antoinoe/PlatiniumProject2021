//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEditor;
//using UnityEditor.AnimatedValues;


//public class TweakPart
//{
//    public bool button;
//    public AnimBool showPart;
//    public string buttonImg;
//    public bool buttonClicked;
//    public string buttonName;
//    public string buttonTootip;
//    public bool hasUnghangedValues = false;
//    public Dictionary<string, object> variables = new Dictionary<string, object>();

//    public TweakPart(string btnName, string btnImg, string btnTooltip = "Tooltip", Dictionary<string, object> _variables = null)
//    {
//        button = false;
//        buttonName = btnName;
//        buttonImg = btnImg;
//        showPart = new AnimBool(false);
//        buttonClicked = false;
//        buttonTootip = btnTooltip;
//        variables = _variables;
//    }
//}

///*public interface IChangeValues
//{
//    public void OnValuesChange(Dictionary<string, object> varDict);
//}*/




//public class Tweaker : EditorWindow
//{

//    public delegate void OnButtonClickDelegate();
//    public static OnButtonClickDelegate buttonClickDelegate;

//    public enum NbrOfPlayers
//    {
//        ONE,
//        TWO,
//        THREE,
//        FOUR
//    }

//    static Tweaker instance;

//    static string ok
//    {
//        get { return "d_winbtn_mac_max"; }
//    }

//    static string unsaved
//    {
//        get { return "d_winbtn_mac_close"; }
//    }

//    public static Tweaker Instance
//    {
//        get { return instance == null ? instance = new Tweaker() : instance; }
//    }


//    static TweakPart game = new TweakPart("Game", ok, "Tooltip", new Dictionary<string, object>()
//    {
//        { "NbrOfPlayers", new NbrOfPlayers() },
//        { "IAPerPlayer", new int() },

//    });
//    static TweakPart player = new TweakPart("Player", ok, "Tooltip", new Dictionary<string, object>() 
//    {
//        { "ShowGizmos", new bool() },
//        { "Speed", new float() },
//        { "Acceleration", new float() },
//        { "Decceleration", new float() },
//        { "KillCooldown", new float() },
//        { "KillCooldownOnAI", new float() },
//        { "BoxLength", new float() },

//    });
//    static TweakPart ia = new TweakPart("IA", ok, "Tooltip", new Dictionary<string, object>()
//    {
//        { "ShowGizmos", new bool() },
//        { "Speed", new float() },
//        { "MoveRange", new Vector2() },
//        { "MoveTime", new Vector2() },

//    });
//    static TweakPart dog = new TweakPart("Dog", ok);
//    static TweakPart lantern = new TweakPart("Lantern", ok);
//    static TweakPart fx = new TweakPart("FX", ok);

//    static List<TweakPart> tpArr = new List<TweakPart>() { game, player, ia, dog, lantern, fx };

//    GameManager gameManager;
//    PlayerController playerController;
//    AIController aiController;
//    Controller controller;
//    Attack attack;
//    NavMeshAgent navAgent;

//    GameObject gameManagerGo;
//    GameObject playerGo;
//    GameObject iaGo;

//    Color oldColor;

//    string m_String;
//    Color m_Color = Color.white;
//    int m_Number = 0;

//    [MenuItem("Ocultas/Tweaker")]
//    static void InitWindow()
//    {
//        Tweaker tweaker = GetWindow<Tweaker>();
//        tweaker.titleContent = new GUIContent("Tweaker");
//        tweaker.Show();
//    }
//    void OnEnable()
//    {
//        oldColor = GUI.backgroundColor;
        
//        gameManagerGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Charlelie/Prefabs/GameManager.prefab", typeof(GameObject));
//        playerGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Charlelie/Prefabs/Player.prefab", typeof(GameObject));
//        iaGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Loick/IA.prefab", typeof(GameObject));

//        gameManager = gameManagerGo.GetComponent<GameManager>();
//        playerController = playerGo.GetComponent<PlayerController>();
//        controller = playerGo.GetComponent<Controller>();
//        attack = playerGo.GetComponent<Attack>();

//        aiController = iaGo.GetComponent<AIController>();
//        navAgent = iaGo.GetComponent<NavMeshAgent>();

//        InitVariables();
//    }

//    void InitVariables()
//    {
//        //Debug.Log(gameManager.playerNbrs + "  " + gameManager.IAPerPlayer);
//        switch (gameManager.playerNbrs)
//        {
//            case 1:
//                game.variables["NbrOfPlayers"] = NbrOfPlayers.ONE;
//                break;

//            case 2:
//                game.variables["NbrOfPlayers"] = NbrOfPlayers.TWO;
//                break;

//            case 3:
//                game.variables["NbrOfPlayers"] = NbrOfPlayers.THREE;
//                break;

//            case 4:
//                game.variables["NbrOfPlayers"] = NbrOfPlayers.FOUR;
//                break;

//            default:
//                game.variables["NbrOfPlayers"] = NbrOfPlayers.ONE;
//                break;

//        }
//        game.variables["IAPerPlayer"] = gameManager.IAPerPlayer;


//        player.variables["ShowGizmos"] = controller.ShowGizmos;
//        player.variables["Speed"] = controller.Speed;
//        player.variables["Acceleration"] = controller.SpeedingRate;
//        player.variables["Decceleration"] = controller.SlowingRate;
//        player.variables["KillCooldown"] = playerController.killCooldown;
//        player.variables["KillCooldownOnAI"] = playerController.KillIAaddCooldown;
//        player.variables["BoxLength"] = attack.GetBoxLength;

//        ia.variables["ShowGizmos"] = aiController.ShowGizmos;
//        ia.variables["Speed"] = aiController.speed;
//        ia.variables["MoveRange"] = new Vector2(aiController.localMinMoveRange, aiController.localMaxMoveRange);
//        ia.variables["MoveTime"] = new Vector2(aiController.delayMin, aiController.delayMax);
//    }


//    Vector2 scrollPos;

//    private void OnGUI()
//    {
//        //for (int i = 0; i < tpArr.Count; i++)
//        //{
//        //    tpArr[i].button = EditorGUILayout.DropdownButton(new GUIContent(tpArr[i].buttonName/*, btnImg*/, "Yool"), FocusType.Keyboard);

//        //    if (tpArr[i].button)
//        //        tpArr[i].buttonClicked = !tpArr[i].buttonClicked;

//        //    tpArr[i].showPart.target = tpArr[i].buttonClicked;
//        //}

//        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 30));

//        #region Game

//        game.button = EditorGUILayout.DropdownButton(new GUIContent(game.buttonName, EditorGUIUtility.FindTexture(game.buttonImg), game.buttonTootip), FocusType.Keyboard);

//        if (game.button)
//            game.buttonClicked = !game.buttonClicked;

//        game.showPart.target = game.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(game.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            EditorGUILayout.PrefixLabel("Nbr of players");
//            game.variables["NbrOfPlayers"] = (NbrOfPlayers)EditorGUILayout.EnumPopup((NbrOfPlayers)game.variables["NbrOfPlayers"]);
//            EditorGUILayout.PrefixLabel("IA per players");
//            game.variables["IAPerPlayer"] = EditorGUILayout.IntField((int)game.variables["IAPerPlayer"]);
           
//            EditorGUI.indentLevel--;

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                game.buttonImg = unsaved;
//            }
//        }

//        EditorGUILayout.EndFadeGroup();


//        #endregion


//        #region Player


//        player.button = EditorGUILayout.DropdownButton(new GUIContent(player.buttonName, EditorGUIUtility.FindTexture(player.buttonImg), player.buttonTootip), FocusType.Keyboard);

//        if (player.button)
//            player.buttonClicked = !player.buttonClicked;

//        player.showPart.target = player.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(player.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            player.variables["ShowGizmos"] = EditorGUILayout.Toggle("Show Gizmos", (bool)player.variables["ShowGizmos"]);
//            EditorGUILayout.PrefixLabel("Speed");
//            player.variables["Speed"] = EditorGUILayout.Slider((float)player.variables["Speed"], 0, 10);
//            EditorGUILayout.PrefixLabel("Acceleration");
//            player.variables["Acceleration"] = EditorGUILayout.Slider((float)player.variables["Acceleration"], 0, 10);
//            EditorGUILayout.PrefixLabel("Decceleration");
//            player.variables["Decceleration"] = EditorGUILayout.Slider((float)player.variables["Decceleration"], 0, 10);
//            EditorGUILayout.PrefixLabel("Attack Cooldown");
//            player.variables["KillCooldown"] = EditorGUILayout.Slider((float)player.variables["KillCooldown"], 0, 10);
//            EditorGUILayout.PrefixLabel("Attack Cooldown when kill AI");
//            player.variables["KillCooldownOnAI"] = EditorGUILayout.Slider((float)player.variables["KillCooldownOnAI"], 0, 10);
//            EditorGUILayout.PrefixLabel("Attack Range");
//            player.variables["BoxLength"] = EditorGUILayout.Slider((float)player.variables["BoxLength"], 0, 10);

//            EditorGUI.indentLevel--;

//            if (Application.isPlaying)
//                foreach(Controller cont in FindObjectsOfType<Controller>())
//                    cont.OnValuesChanged((bool)player.variables["ShowGizmos"], (float)player.variables["Speed"], (float)player.variables["Acceleration"], (float)player.variables["Decceleration"], 
//                        (float)player.variables["KillCooldown"], (float)player.variables["KillCooldownOnAI"], 
//                        (float)player.variables["BoxLength"]);

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                player.buttonImg = unsaved;
//            }
//        }

//        EditorGUILayout.EndFadeGroup();

//        #endregion

//        #region IA

//        ia.button = EditorGUILayout.DropdownButton(new GUIContent(ia.buttonName, EditorGUIUtility.FindTexture(ia.buttonImg), ia.buttonTootip), FocusType.Keyboard);

//        if (ia.button)
//            ia.buttonClicked = !ia.buttonClicked;

//        ia.showPart.target = ia.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(ia.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            ia.variables["ShowGizmos"] = EditorGUILayout.Toggle("Show Gizmos", (bool)ia.variables["ShowGizmos"]);
//            EditorGUILayout.PrefixLabel("Speed");
//            ia.variables["Speed"] = EditorGUILayout.Slider((float)ia.variables["Speed"], 0, 10);
//            Vector2 vecSlider = new Vector2(((Vector2)ia.variables["MoveRange"]).x, ((Vector2)ia.variables["MoveRange"]).y);
//            EditorGUILayout.PrefixLabel("Move Range");           
//            EditorGUILayout.MinMaxSlider(ref vecSlider.x, ref vecSlider.y, 0, 10);
//            EditorGUILayout.Vector2Field("Range: ", new Vector2(vecSlider.x, vecSlider.y));
//            ia.variables["MoveRange"] = vecSlider;
//            ia.variables["MoveTime"] = new Vector2(((Vector2)ia.variables["MoveTime"]).x, ((Vector2)ia.variables["MoveTime"]).y);
//            EditorGUILayout.Vector2Field("Move time", (Vector2)ia.variables["MoveTime"]);
//            EditorGUI.indentLevel--;

//            foreach (AIController cont in FindObjectsOfType<AIController>())
//                cont.OnValuesChanged((bool)ia.variables["ShowGizmos"], (float)ia.variables["Speed"], (Vector2)ia.variables["MoveRange"], (Vector2)ia.variables["MoveTime"]);

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                ia.buttonImg = unsaved;
//            }

//        }

//        EditorGUILayout.EndFadeGroup();


//        #endregion

//        #region Dog

//        dog.button = EditorGUILayout.DropdownButton(new GUIContent(dog.buttonName, EditorGUIUtility.FindTexture(dog.buttonImg), dog.buttonTootip), FocusType.Keyboard);

//        if (dog.button)
//            dog.buttonClicked = !dog.buttonClicked;

//        dog.showPart.target = dog.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(dog.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            EditorGUILayout.PrefixLabel("Color");
//            m_Color = EditorGUILayout.ColorField(m_Color);
//            EditorGUILayout.PrefixLabel("Text");
//            m_String = EditorGUILayout.TextField(m_String);
//            EditorGUILayout.PrefixLabel("Number");
//            m_Number = EditorGUILayout.IntSlider(m_Number, 0, 10);
//            EditorGUI.indentLevel--;

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                dog.buttonImg = unsaved;
//            }
//        }

//        EditorGUILayout.EndFadeGroup();

//        #endregion

//        #region Lantern

//        lantern.button = EditorGUILayout.DropdownButton(new GUIContent(lantern.buttonName, EditorGUIUtility.FindTexture(lantern.buttonImg), lantern.buttonTootip), FocusType.Keyboard);

//        if (lantern.button)
//            lantern.buttonClicked = !lantern.buttonClicked;

//        lantern.showPart.target = lantern.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(lantern.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            EditorGUILayout.PrefixLabel("Color");
//            m_Color = EditorGUILayout.ColorField(m_Color);
//            EditorGUILayout.PrefixLabel("Text");
//            m_String = EditorGUILayout.TextField(m_String);
//            EditorGUILayout.PrefixLabel("Number");
//            m_Number = EditorGUILayout.IntSlider(m_Number, 0, 10);
//            EditorGUI.indentLevel--;

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                lantern.buttonImg = unsaved;
//            }

//        }

//        EditorGUILayout.EndFadeGroup();


//        #endregion

//        #region FX

//        fx.button = EditorGUILayout.DropdownButton(new GUIContent(fx.buttonName, EditorGUIUtility.FindTexture(fx.buttonImg), fx.buttonTootip), FocusType.Keyboard);

//        if (fx.button)
//            fx.buttonClicked = !fx.buttonClicked;

//        fx.showPart.target = fx.buttonClicked;

//        if (EditorGUILayout.BeginFadeGroup(fx.showPart.faded))
//        {
//            EditorGUI.BeginChangeCheck();

//            EditorGUI.indentLevel++;
//            EditorGUILayout.PrefixLabel("Color");
//            m_Color = EditorGUILayout.ColorField(m_Color);
//            EditorGUILayout.PrefixLabel("Text");
//            m_String = EditorGUILayout.TextField(m_String);
//            EditorGUILayout.PrefixLabel("Number");
//            m_Number = EditorGUILayout.IntSlider(m_Number, 0, 10);
//            EditorGUI.indentLevel--;

//            if (EditorGUI.EndChangeCheck())
//            {
//                hasUnsavedChanges = true;
//                fx.buttonImg = unsaved;
//            }

//        }

//        EditorGUILayout.EndFadeGroup();

//        #endregion

//        EditorGUILayout.EndScrollView();

//        if (hasUnsavedChanges)
//        {
//            GUI.backgroundColor = Color.red;
//        }

//        if (GUILayout.Button("Save"))
//            SaveChanges();

        
//    }

//    public override void SaveChanges()
//    {
//        gameManagerGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Charlelie/Prefabs/GameManager.prefab", typeof(GameObject));
//        playerGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Charlelie/Prefabs/Player.prefab", typeof(GameObject));
//        iaGo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Loick/IA.prefab", typeof(GameObject));

//        gameManager = gameManagerGo.GetComponent<GameManager>();
//        playerController = playerGo.GetComponent<PlayerController>();
//        controller = playerGo.GetComponent<Controller>();
//        attack = playerGo.GetComponent<Attack>();

//        aiController = iaGo.GetComponent<AIController>();
//        navAgent = iaGo.GetComponent<NavMeshAgent>();

//        int pNbr = 0;

//        switch (game.variables["NbrOfPlayers"])
//        {
//                case NbrOfPlayers.ONE:
//                    pNbr = 1;
//                    break;

//                case NbrOfPlayers.TWO:
//                    pNbr = 2;
//                    break;

//                case NbrOfPlayers.THREE:
//                    pNbr = 3;
//                    break;

//                case NbrOfPlayers.FOUR:
//                    pNbr = 4;
//                    break;

//                default:
//                    pNbr = 2;
//                    break;
//        }

//        FindObjectOfType<GameManager>().OnValuesChanged(pNbr, (int)game.variables["IAPerPlayer"]);
//        gameManager.OnValuesChanged(pNbr, (int)game.variables["IAPerPlayer"]);

//        controller.OnValuesChanged((bool)player.variables["ShowGizmos"], (float)player.variables["Speed"], (float)player.variables["Acceleration"], (float)player.variables["Decceleration"],
//                        (float)player.variables["KillCooldown"], (float)player.variables["KillCooldownOnAI"],
//                        (float)player.variables["BoxLength"]);

//        aiController.OnValuesChanged((bool)ia.variables["ShowGizmos"], (float)ia.variables["Speed"], (Vector2)ia.variables["MoveRange"], (Vector2)ia.variables["MoveTime"]);

//        //for (int i = 0; i < tpArr.Count - 1; i++)
//        //{
//        //    tpArr[i].buttonImg = ok;
//        //}

//        foreach (TweakPart tp in tpArr)
//            tp.buttonImg = ok;

//        GUI.backgroundColor = oldColor;

//        Debug.Log($"{this} saved successfully!!!");
//        base.SaveChanges();
//    }

//}
