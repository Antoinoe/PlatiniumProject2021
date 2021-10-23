using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Rewired;
public class Controller : MonoBehaviour
{
    int playerNum;
    float speed = 4;
    public Rewired.Player player;
    void Start()
    {
        
    }

    public void SetNum(int val)
    {
        playerNum = val;
        player = ReInput.players.GetPlayer(playerNum);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        Vector2 vec = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
        transform.Translate(vec * speed * Time.deltaTime);
    }
}
