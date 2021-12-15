using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using DG.Tweening;


public class PauseNav : MonoBehaviour
{
    EventSystem eventSystem;
    Button resume;
    bool good = false;
    bool isReplay = true;
    private Vector2 minusScale;
    private Vector2 actualScale;
    private float duration = 0.25f;
    bool navLock = false;
    public Color selectColor;


    void Start()
    {
        float timer = 1.0f;
        while (timer > 0)
            timer -= Time.deltaTime;
        good = true;
        resume = transform.GetChild(0).GetComponent<Button>();
        resume.Select();
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(resume.gameObject);
        actualScale = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().sizeDelta;
        //minusScale = new Vector2(actualScale.x - 250, actualScale.y);
        //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
        eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
    }

    void Update()
    {
        Debug.Log(eventSystem.currentSelectedGameObject.GetComponent<Button>().name);
        if (ReInput.players.GetPlayer(0).GetAxisRaw("MoveVertical") == 0) navLock = false;
        if (!good || navLock) return;
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown("Attack") && eventSystem.currentSelectedGameObject)
                eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveVertical") > 0)
            {
                //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(minusScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnUp().gameObject);
                //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
                navLock = true;
            }
            else if (ReInput.players.GetPlayer(i).GetAxisRaw("MoveVertical") < 0)
            {
                //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(minusScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject.GetComponent<Button>().FindSelectableOnDown().gameObject);
                //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
                eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().color = selectColor;
                navLock = true;
            }
        }
    }
}

