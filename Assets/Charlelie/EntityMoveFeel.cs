using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveFeel : MonoBehaviour
{
    bool isMoving = false;
    float moveIndex = 0;
    float moveSpeed = 25;
    int moveWay = 1;
    Vector3 idleIndex = Vector2.one;
    float idleSpeed = 1;
    int idleWay = 1;


    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    void Update()
    {
        if (isMoving)
        {
            moveIndex += (Time.deltaTime * moveSpeed) * moveWay;
        }
        else
        {
            idleIndex.x += (Time.deltaTime * idleSpeed) * idleWay;
            idleIndex.y -= ((Time.deltaTime * idleSpeed)) * idleWay;
        }
            
        //DEBUG
        if (moveIndex <= -10 || moveIndex >= 10) moveWay *= -1;
        if (idleIndex.x <= 0.75f || idleIndex.x >= 1.25f) idleWay *= -1;
        if (idleIndex.x > 1.25f) idleIndex.x = 1.25f;
        if (idleIndex.x < 0.75f) idleIndex.x = 0.75f;
    }

    private void FixedUpdate()
    {
        if (GetComponent<Controller>())
            isMoving = GetComponent<Controller>().MovementVector != Vector2.zero;
        Vector3 rot = transform.localRotation.eulerAngles;
        if (isMoving)
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, moveIndex);
            transform.localScale = Vector3.one;
        } 
        else
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, 0);
            transform.localScale = idleIndex;
        }
    }
}
