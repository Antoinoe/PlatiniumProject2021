using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour
{
    private GameObject[] buttons = new GameObject[4];
    private EventSystem sys; 
    private GameObject selectedItem;
    private Vector2 plusScale = new Vector2(295,20);
    private Vector2 actualScale;
    private float duration = 0.25f;
    void Start()
    {
        for (int i = 0; i<buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
        }
        sys = EventSystem.current;
        //print(sys);
        ResetSelectedObject();
        actualScale = buttons[1].GetComponent<RectTransform>().sizeDelta;
    }
    private void Update()
    {
        if (sys.currentSelectedGameObject != selectedItem)
            UpdateNav();
        //print(selectedItem);
    }

    public void ResetSelectedObject()
    {
        sys.SetSelectedGameObject(buttons[0]);
        selectedItem = sys.currentSelectedGameObject;
    }

    private void UpdateNav()
    {
        if(MenuManager.Instance.actualMenuOn == Menu.MAIN)
        {
            selectedItem = sys.currentSelectedGameObject;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (selectedItem == buttons[i])
                {
                    selectedItem.GetComponent<RectTransform>().DOSizeDelta(actualScale + plusScale, duration);
                }
                else
                {
                    buttons[i].GetComponent<RectTransform>().DOSizeDelta(actualScale, duration);
                }
            }
        }

    }
}
