using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class Selector : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] items;
    private bool haveToWait = true;
    [SerializeField] public float swapDuration;
    [SerializeField] [Range(0.5f,1.5f)] private float mapScaleMin, mapScaleMax;
    private bool canSwitch = true;
    private float XdistToGo;
    [HideInInspector]public int it;
    public Menu thisMenu;
    public GameObject NavTuto;
    void Start()
    {
        Array.Resize(ref items, transform.childCount);
        for(int i = 0; i< transform.childCount; i++)
        {
            items[i] = transform.GetChild(i).gameObject;
        }
        XdistToGo = items[1].GetComponent<RectTransform>().localPosition.x - items[0].GetComponent<RectTransform>().localPosition.x;
        //reset the scale of the maps in the map menu
        if (thisMenu == Menu.MAP)
            ScaleItems();
    }

    // Update is called once per frame
    void Update()
    {
        //inputs
        if (MenuManager.Instance.actualMenuOn == thisMenu)
        {
            if (Input.GetKey(KeyCode.D) && canSwitch) //right
            {
                print("next");
                StartCoroutine(Change(false));
            }
            if (Input.GetKey(KeyCode.Q) && canSwitch) //left
            {
                print("prev");
                StartCoroutine(Change(true));
            }
        }
    }

    IEnumerator Change(bool input)
    {
        haveToWait = true;
        canSwitch = false;
        print("int ienum");
        #region move items
        if (input)
        {
            if (it > 0)
            {
                it--;
                transform.DOLocalMoveX(transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            if (it < items.Length - 1)
            {
                it++;
                transform.DOLocalMoveX(transform.localPosition.x - XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        #endregion

        if (thisMenu == Menu.MAP)
            ScaleItems();
        if (thisMenu == Menu.TUTO)
            UpdateNav();

        if(haveToWait)
            yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        canSwitch = true;
    }

    void ScaleItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (it == i)
                items[i].GetComponent<RectTransform>().DOScale(new Vector2(1.3f, 1.3f), swapDuration);
            else
                items[i].GetComponent<RectTransform>().DOScale(new Vector2(1f, 1f), swapDuration);
        }
    }

    void UpdateNav()
    {
        NavTuto.GetComponent<DisplayNavTuto>().UpdateStateOfSquares();
    }

    public string SelectMap()
    {
       return items[it].name;
    }
}
