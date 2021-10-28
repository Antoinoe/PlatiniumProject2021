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
    float slowingRate = 0.0f;
    float speedingRate = 0.0f;
    Vector2 _movementVec;

    bool isPhyGUIShown = false;

    public Vector2 MovementVector
    {
        get { return _movementVec; }
    }

    void Start()
    {
        
    }

    public void SetNum(int val)
    {
        playerNum = val;
        player = ReInput.players.GetPlayer(playerNum);
        InputBehavior ib = player.controllers.maps.GetInputBehavior(0);
        slowingRate = ib.digitalAxisGravity;
        speedingRate = ib.digitalAxisSensitivity;
    }

    void Update()
    {
        if (player.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
            GetComponent<Attack>().OnAttack();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        _movementVec = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
        transform.Translate(_movementVec * speed * Time.fixedDeltaTime);
    }


    private void OnGUI()
    {
        Debug.Log(isPhyGUIShown);
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
            slowingRate = GUI.HorizontalSlider(new Rect(Screen.width - 280, Screen.height - 60, 250, 20), slowingRate, 0.0F, 10.0F);
            GUI.Label(new Rect(Screen.width - 20, Screen.height - 65, 250, 20), slowingRate.ToString());

            GUI.Label(new Rect(Screen.width - 400, Screen.height - 40, 250, 20), "Speeding Rate");
            speedingRate = GUI.HorizontalSlider(new Rect(Screen.width - 280, Screen.height - 40, 250, 20), speedingRate, 0.0F, 10.0F);
            GUI.Label(new Rect(Screen.width - 20, Screen.height - 45, 250, 20), speedingRate.ToString());

            if (GUI.Button(new Rect(Screen.width - 80, Screen.height - 20, 80, 20), "Close X"))
                isPhyGUIShown = false;
        }       
    }
}
