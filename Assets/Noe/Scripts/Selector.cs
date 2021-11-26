using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] items;
    private bool haveToWait = true;
    public Animator lArrowAnim, rArrowAnim;
    [SerializeField] public float swapDuration;
    [SerializeField] [Range(0.5f,1.5f)] private float mapScaleMin, mapScaleMax;
    private bool canSwitch = true;
    private float XdistToGo;
    [HideInInspector]public int it;
    public Menu thisMenu;
    public GameObject NavTuto;
    public Rewired.Player player;
    public EventSystem eventSys;
    bool navLock = false;

    void Start()
    {
        player = ReInput.players.GetPlayer(0);
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
        float nav = player.GetAxisRaw("MoveHorizontal");
        if (MenuManager.Instance.actualMenuOn == thisMenu)
        {
            if (nav > 0 && canSwitch) //right
                StartCoroutine(Change(false));
            if (nav < 0 && canSwitch) //left
                StartCoroutine(Change(true));

            if(thisMenu == Menu.MAP)
            {
                if (nav > 0)
                    rArrowAnim.SetBool("isActivate", true);
                if (nav > 0)
                    rArrowAnim.SetBool("isActivate", false);
                if (nav < 0)
                    lArrowAnim.SetBool("isActivate", true);
                if (nav < 0)
                    lArrowAnim.SetBool("isActivate", false);
            }
            


        }
    }

    IEnumerator Change(bool input)
    {
        haveToWait = true;
        canSwitch = false;
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
                items[i].GetComponent<RectTransform>().DOScale(mapScaleMax, swapDuration);
            else
                items[i].GetComponent<RectTransform>().DOScale(mapScaleMin, swapDuration);
        } 
    }

    void UpdateNav()
    {
        NavTuto.GetComponent<DisplayNavTuto>().UpdateStateOfSquares();
    }

    public string SelectMap()
    {
       return items[it].GetComponent<MapName>().mapName;
    }
}
