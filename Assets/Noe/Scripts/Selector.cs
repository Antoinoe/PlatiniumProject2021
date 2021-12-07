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
    [SerializeField] [Range(0.5f, 1.5f)] private float mapScaleMin, mapScaleMax;
    private bool canSwitch = true;
    private float XdistToGo;
    [HideInInspector] public int it;
    public Menu thisMenu;
    public GameObject NavTuto;
    public Rewired.Player player;
    public EventSystem eventSys;
    bool navLock = false;
    bool[] playersWait = { false, false, false, false };
    int[] pIt = { 0, 0, 0, 0 };

    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        Array.Resize(ref items, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
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
            if (thisMenu == Menu.MAP)
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
            else if (thisMenu == Menu.CHARACTER)
            {
                for (int i = 0; i < ReInput.controllers.joystickCount; i++)
                {
                    float nave = ReInput.players.GetPlayer(i).GetAxisRaw("MoveHorizontal");
                    if (nave > 0 && canSwitch) //right
                        ChangePlayer(i, false);
                    if (nave < 0 && canSwitch) //left
                        ChangePlayer(i, true);
                }

            }
            else
            {
                if (nav > 0 && canSwitch) //right
                    StartCoroutine(Change(false, false, 0));
                if (nav < 0 && canSwitch) //left
                    StartCoroutine(Change(true, false, 0));
            }



        }
    }


    //C'est du big caca je sais dsl mais je m'en bas les c*** à molette
    void ChangePlayer(int pId, bool input)
    {
        switch (pId)
        {
            case 0:
                if (!playersWait[0])
                    StartCoroutine(ChangePlayer1(input));
                break;

            case 1:
                if (!playersWait[1])
                    StartCoroutine(ChangePlayer2(input));
                break;

            case 2:
                if (!playersWait[2])
                    StartCoroutine(ChangePlayer3(input));
                break;

            case 3:
                if (!playersWait[3])
                    StartCoroutine(ChangePlayer4(input));
                break;
        }
    }

    IEnumerator Change(bool input, bool isPlayer, int playerId)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        haveToWait = true;
        canSwitch = false;
        #region move items
        if (input)
        {
            Debug.Log(it + "  " + items.Length);
            if (it > 0)
            {
                it--;
                if (isPlayer)
                    root.transform.GetChild(playerId).GetChild(0).DOLocalMoveX(root.transform.GetChild(playerId).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
                else
                    transform.DOLocalMoveX(transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            Debug.Log(it + "  " + items.Length);
            if (it < items.Length - 1)
            {
                it++;
                if (isPlayer)
                    root.transform.GetChild(playerId).GetChild(0).DOLocalMoveX(root.transform.GetChild(playerId).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
                else
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

        if (haveToWait)
            yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        canSwitch = true;
    }

    IEnumerator ChangePlayer1(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        playersWait[0] = true;
        #region move items
        if (input)
        {
            Debug.Log(pIt[0] + "  " + items.Length);
            if (pIt[0] > 0)
            {
                pIt[0]--;
                root.transform.GetChild(0).GetChild(0).DOLocalMoveX(root.transform.GetChild(0).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            Debug.Log(pIt[0] + "  " + items.Length);
            if (pIt[0] < items.Length - 1)
            {
                pIt[0]++;
                root.transform.GetChild(0).GetChild(0).DOLocalMoveX(root.transform.GetChild(0).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        playersWait[0] = false;
    }

    IEnumerator ChangePlayer2(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        playersWait[1] = true;
        #region move items
        if (input)
        {
            Debug.Log(pIt[1] + "  " + items.Length);
            if (pIt[1] > 0)
            {
                pIt[1]--;
                root.transform.GetChild(1).GetChild(0).DOLocalMoveX(root.transform.GetChild(1).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            Debug.Log(pIt[1] + "  " + items.Length);
            if (pIt[1] < items.Length - 1)
            {
                pIt[1]++;
                root.transform.GetChild(1).GetChild(0).DOLocalMoveX(root.transform.GetChild(1).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        playersWait[1] = false;
    }

    IEnumerator ChangePlayer3(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        playersWait[2] = true;
        #region move items
        if (input)
        {
            Debug.Log(pIt[2] + "  " + items.Length);
            if (pIt[2] > 0)
            {
                pIt[2]--;
                root.transform.GetChild(2).GetChild(0).DOLocalMoveX(root.transform.GetChild(2).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            Debug.Log(pIt[2] + "  " + items.Length);
            if (pIt[2] < items.Length - 1)
            {
                pIt[2]++;
                root.transform.GetChild(2).GetChild(0).DOLocalMoveX(root.transform.GetChild(2).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        playersWait[2] = false;
    }

    IEnumerator ChangePlayer4(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        playersWait[3] = true;
        #region move items
        if (input)
        {
            Debug.Log(pIt[3] + "  " + items.Length);
            if (pIt[3] > 0)
            {
                pIt[3]--;
                root.transform.GetChild(3).GetChild(0).DOLocalMoveX(root.transform.GetChild(3).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        else
        {
            Debug.Log(pIt[3] + "  " + items.Length);
            if (pIt[3] < items.Length - 1)
            {
                pIt[3]++;
                root.transform.GetChild(3).GetChild(0).DOLocalMoveX(root.transform.GetChild(3).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        playersWait[3] = false;
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
