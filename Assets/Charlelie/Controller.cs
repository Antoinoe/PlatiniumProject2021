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
        Vector2 vec = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
        transform.Translate(vec * speed * Time.fixedDeltaTime);
    }

    private void OnGUI()
    {
        
    }
}
