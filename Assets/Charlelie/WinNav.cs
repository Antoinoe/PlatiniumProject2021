using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

public class WinNav : MonoBehaviour
{
    EventSystem eventSystem;
    Button replay, menu;
    bool good = false;
    public Sprite[] heads;
    public Image winnerHead;
    int winner;
    bool isReplay = true;
    void Start()
    {
        
    }

    public void Init(int _winner)
    {
        winner = _winner;
        float timer = 1.0f;
        while (timer > 0)
            timer -= Time.deltaTime;
        good = true;
        replay = transform.GetChild(0).GetComponent<Button>();
        menu = transform.GetChild(1).GetComponent<Button>();
        replay.Select();
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(replay.gameObject);
        winnerHead.sprite = heads[winner];
    }


    void Update()
    {
        if (!good) return;
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            if (ReInput.players.GetPlayer(i).GetButtonDown("Attack"))
                eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveHorizontal") > 0)
                if (isReplay)
                {
                    isReplay = !isReplay;
                    eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight().gameObject);
                }
                else
                {
                    isReplay = !isReplay;
                    eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnLeft().gameObject);
                }
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveHorizontal") < 0)
                if (isReplay)
                {
                    isReplay = !isReplay;
                    eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight().gameObject);
                }
                else
                {
                    isReplay = !isReplay;
                    eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnLeft().gameObject);
                }
        }
    }
}
