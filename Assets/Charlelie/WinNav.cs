using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using DG.Tweening;

public class WinNav : MonoBehaviour
{
    EventSystem eventSystem;
    Button replay, menu;
    bool good = false;
    public Sprite[] heads;
    public Image winnerHead;
    int winner;
    bool isReplay = true;
    private Vector2 minusScale;
    private Vector2 actualScale;
    private float duration = 0.25f;
    bool navLock = false;
    public Color selectColor;
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
        actualScale = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta;
        minusScale = new Vector2(actualScale.x - 250, actualScale.y);
        winnerHead.sprite = heads[winner];
        eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
        eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight().GetComponent<RectTransform>().DOSizeDelta(minusScale, duration);
        eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
    }


    void Update()
    {
        Debug.Log(eventSystem.currentSelectedGameObject.GetComponent<Button>().name);
        if (ReInput.players.GetPlayer(0).GetAxisRaw("MoveHorizontal") == 0) navLock = false;
        if (!good || navLock) return;
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown("Attack"))
                eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveHorizontal") > 0)
            {
                eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(minusScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnRight().gameObject);
                eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
                navLock = true;
            }
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveHorizontal") < 0)
            {
                eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(minusScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnLeft().gameObject);
                eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
                navLock = true;
            }                
        }
    }
}
