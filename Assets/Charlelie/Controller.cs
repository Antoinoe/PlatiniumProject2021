using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Rewired;
public class Controller : MonoBehaviour
{
    int playerNum;
    public float speed = 4;
    public Rewired.Player player;
    SpriteRenderer sR;
    Animator anim;
    Vector2 _movementVec;
    InputBehavior ib;
    bool isPhyGUIShown = false;
    PlayerController playerController;
    Attack attack;
    bool showGizmos = true;
    Vector3 previous;
    Vector3 velocity;

    public bool ShowGizmos
    {
        get { return showGizmos; }
        set { showGizmos = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector2 MovementVector
    {
        get { return _movementVec; }
    }

    public float Acceleration
    {
        get { return ib.digitalAxisSensitivity; }
        set { ib.digitalAxisSensitivity = value; }
    }

    public float Decceleration
    {
        get { return ib.digitalAxisGravity; }
        set { ib.digitalAxisGravity = value; }
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        attack = GetComponent<Attack>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        sR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SetNum(int val)
    {
        playerNum = val;
        player = ReInput.players.GetPlayer(playerNum);
        ib = player.controllers.maps.GetInputBehavior(0);
        Decceleration = ib.digitalAxisGravity;
        Acceleration = ib.digitalAxisSensitivity;
        anim.SetFloat("playerNbr", playerNum);
    }

    void Update()
    {
        if (player.GetButtonDown("Attack"))
        {
            //Debug.Log("Attack");
            GetComponent<Attack>().OnAttack();
        }
    }


    
    private void FixedUpdate()
    {
        Move();

        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;

        if ((velocity.x != 0 || velocity.y != 0)/* && !anim.GetBool("isWalking")*/)
        {
            anim.SetBool("isWalking", true);
            if (velocity.x < 0)
                sR.flipX = true;
            else
                sR.flipX = false;
        }
        else /*if ((velocity.x == 0 || velocity.y == 0) && anim.GetBool("isWalking"))*/
        {
            anim.SetBool("isWalking", false);
            sR.flipX = false;
        }
    }



    private void Move()
    {
        if (player != null)
        {
            _movementVec = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
            transform.Translate(_movementVec * speed * Time.fixedDeltaTime);
        } 
    }

    public void OnValuesChanged
    (bool _showGizmo, float _speed, 
    float _accel, float _deccel,
    float _kCool, float _kCoolAI,
    Vector2 _boxLength)
    {
        ShowGizmos = _showGizmo;
        Speed = _speed;
        Acceleration = _accel;
        Decceleration = _deccel;
        GetComponent<PlayerController>().KillCooldown = _kCool;
        GetComponent<PlayerController>().KillIAaddCooldown = _kCoolAI;
        GetComponent<Attack>().BoxLength = _boxLength;
    }


    private void OnGUI()
    {

        if (!isPhyGUIShown)
            if (GUI.Button(new Rect(Screen.width - 140, Screen.height - 100, 140, 100), "Controller physics"))
                isPhyGUIShown = true;

        if (isPhyGUIShown)
        {
            GUI.Box(new Rect(Screen.width - 430, Screen.height - 100, 430, 100), "Controller physics");

            GUI.Label(new Rect(Screen.width - 400, Screen.height - 80, 250, 20), "Speed");
            speed = GUI.HorizontalSlider(new Rect(Screen.width - 280, Screen.height - 80, 250, 20), speed, 0.0F, 20.0F);
            GUI.Label(new Rect(Screen.width - 20, Screen.height - 85, 250, 20), speed.ToString());

            GUI.Label(new Rect(Screen.width - 400, Screen.height - 60, 250, 20), "Slowing Rate");
            Decceleration = GUI.HorizontalSlider(new Rect(Screen.width - 280, Screen.height - 60, 250, 20), Decceleration, 0.0F, 10.0F);
            GUI.Label(new Rect(Screen.width - 20, Screen.height - 65, 250, 20), Decceleration.ToString());

            GUI.Label(new Rect(Screen.width - 400, Screen.height - 40, 250, 20), "Speeding Rate");
            Acceleration = GUI.HorizontalSlider(new Rect(Screen.width - 280, Screen.height - 40, 250, 20), Acceleration, 0.0F, 10.0F);
            GUI.Label(new Rect(Screen.width - 20, Screen.height - 45, 250, 20), Acceleration.ToString());

            if (GUI.Button(new Rect(Screen.width - 80, Screen.height - 20, 80, 20), "Close X"))
                isPhyGUIShown = false;
        }       
    }
}
