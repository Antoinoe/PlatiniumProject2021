using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Rewired;
public class Controller : MonoBehaviour
{


    //[DllImport("DeviceEnumeration", EntryPoint = "main")]
    //public static extern int main();

    EPlayerNum playerNum;
    int pNum = 0;
    string verticalAxis, horizontalAxis;
    float speed = 4;
    public Player player;
    void Start()
    {
        //int test = main();
        //Debug.Log(test);
        player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetPlayerNum(EPlayerNum num)
    {
        playerNum = num;
        switch (num)
        {
            case EPlayerNum.Player1:
                pNum = 1;
                verticalAxis = "Vertical1";
                horizontalAxis = "Horizontal1";
                break;

            case EPlayerNum.Player2:
                pNum = 2;
                verticalAxis = "Vertical2";
                horizontalAxis = "Horizontal2";
                break;

            case EPlayerNum.Player3:
                pNum = 3;
                verticalAxis = "Vertical3";
                horizontalAxis = "Horizontal3";
                break;

            case EPlayerNum.Player4:
                pNum = 4;
                verticalAxis = "Vertical4";
                horizontalAxis = "Horizontal4";
                break;
        }

        
    }

    private void Move()
    {
        Vector2 vec = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
        transform.Translate(vec * speed * Time.deltaTime);
    }
}
