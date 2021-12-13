using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.EventSystems;

public static class p
{
    public static int[] it = { 0, 0, 0, 0 };
}

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
    public int[] pIt = { 0, 0, 0, 0 };

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
                {
                    FindObjectOfType<AudioManager>().Play("UIUp");
                    rArrowAnim.SetBool("isActivate", true);
                    if (canSwitch)
                        StartCoroutine(Change(true,false,0));
                }
                    
                if (nav == 0)
                    rArrowAnim.SetBool("isActivate", false);
                if (nav < 0)
                {
                    FindObjectOfType<AudioManager>().Play("UIDown");
                    lArrowAnim.SetBool("isActivate", true);
                    if (canSwitch)
                        StartCoroutine(Change(false, false, 0));
                }
                    
                if (nav == 0)
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
                {
                    FindObjectOfType<AudioManager>().Play("UIUp");
                    StartCoroutine(Change(false, false, 0));
                }
                if (nav < 0 && canSwitch) //left
                {
                    FindObjectOfType<AudioManager>().Play("UIDown");
                    StartCoroutine(Change(true, false, 0));
                }
            }



        }

        //string str = "";
        //for (int i = 0; i < Data.pSprite.Length; i++)
        //{
        //    str += " " + Data.pSprite[i] + " ";
        //}
        //Debug.Log(str);
    }


    //C'est du big caca je sais dsl mais je m'en bas les c*** � molette
    void ChangePlayer(int pId, bool input)
    {
        if (MenuManager.Instance.selectedChara[pId]) return;
        switch (pId)
        {
            case 0:
                if (!MenuManager.Instance.playersWait[0])
                    StartCoroutine(ChangePlayer1(input));
                break;

            case 1:
                if (!MenuManager.Instance.playersWait[1])
                    StartCoroutine(ChangePlayer2(input));
                break;

            case 2:
                if (!MenuManager.Instance.playersWait[2])
                    StartCoroutine(ChangePlayer3(input));
                break;

            case 3:
                if (!MenuManager.Instance.playersWait[3])
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
            //Debug.Log(it + "  " + items.Length);
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
            //Debug.Log(it + "  " + items.Length);
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
        MenuManager.Instance.playersWait[0] = true;
        #region move items
        if (input)
        {
            //Debug.Log(p.it[0] + "  " + items.Length);
            if (p.it[0] > 0)
            {
                FindObjectOfType<AudioManager>().Play("UIUp");
                p.it[0]--;
                root.transform.GetChild(0).GetChild(0).DOLocalMoveX(root.transform.GetChild(0).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
                Data.pSprite[0] = pIt[0];
                Data.pUiGame[0] = pIt[0];
            }
            else
                haveToWait = false;
        }
        else
        {
            //Debug.Log(pIt[0] + "  " + items.Length);
            if (p.it[0] < items.Length - 1)
            {
                FindObjectOfType<AudioManager>().Play("UIDown");
                p.it[0]++;
                root.transform.GetChild(0).GetChild(0).DOLocalMoveX(root.transform.GetChild(0).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
                Data.pSprite[0] = pIt[0];
                Data.pUiGame[0] = pIt[0];
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        MenuManager.Instance.playersWait[0] = false;
    }

    IEnumerator ChangePlayer2(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        MenuManager.Instance.playersWait[1] = true;
        #region move items
        if (input)
        {
            //Debug.Log(pIt[1] + "  " + items.Length);
            if (p.it[1] > 0)
            {
                FindObjectOfType<AudioManager>().Play("UIUp");
                p.it[1]--;
                root.transform.GetChild(1).GetChild(0).DOLocalMoveX(root.transform.GetChild(1).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
                Data.pSprite[1] = pIt[1];
                Data.pUiGame[1] = pIt[1];
            }
            else
                haveToWait = false;
        }
        else
        {
            //Debug.Log(pIt[1] + "  " + items.Length);
            if (p.it[1] < items.Length - 1)
            {
                FindObjectOfType<AudioManager>().Play("UIDown");
                p.it[1]++;
                root.transform.GetChild(1).GetChild(0).DOLocalMoveX(root.transform.GetChild(1).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
                Data.pSprite[1] = pIt[1];
                Data.pUiGame[1] = pIt[1];
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        MenuManager.Instance.playersWait[1] = false;
    }

    IEnumerator ChangePlayer3(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        MenuManager.Instance.playersWait[2] = true;
        #region move items
        if (input)
        {
            //Debug.Log(pIt[2] + "  " + items.Length);
            if (p.it[2] > 0)
            {
                FindObjectOfType<AudioManager>().Play("UIUp");
                p.it[2]--;
                root.transform.GetChild(2).GetChild(0).DOLocalMoveX(root.transform.GetChild(2).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
                Data.pSprite[2] = pIt[2];
                Data.pUiGame[2] = pIt[2];
            }
            else
                haveToWait = false;
        }
        else
        {
            //Debug.Log(pIt[2] + "  " + items.Length);
            if (p.it[2] < items.Length - 1)
            {
                FindObjectOfType<AudioManager>().Play("UIDown");
                p.it[2]++;
                root.transform.GetChild(2).GetChild(0).DOLocalMoveX(root.transform.GetChild(2).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
                Data.pSprite[2] = pIt[2];
                Data.pUiGame[2] = pIt[2];
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        MenuManager.Instance.playersWait[2] = false;
    }

    IEnumerator ChangePlayer4(bool input)
    {
        GameObject root = GameObject.Find("PlayersGrid");
        MenuManager.Instance.playersWait[3] = true;
        #region move items
        if (input)
        {
            //Debug.Log(pIt[3] + "  " + items.Length);
            if (p.it[3] > 0)
            {
                FindObjectOfType<AudioManager>().Play("UIUp");
                p.it[3]--;
                root.transform.GetChild(3).GetChild(0).DOLocalMoveX(root.transform.GetChild(3).GetChild(0).transform.localPosition.x + XdistToGo, swapDuration);
                Data.pSprite[3] = pIt[3];
                Data.pUiGame[3] = pIt[3];
            }
            else
                haveToWait = false;
        }
        else
        {
            //Debug.Log(pIt[3] + "  " + items.Length);
            if (p.it[3] < items.Length - 1)
            {
                FindObjectOfType<AudioManager>().Play("UIDown");
                p.it[3]++;
                root.transform.GetChild(3).GetChild(0).DOLocalMoveX(root.transform.GetChild(3).GetChild(0).transform.localPosition.x - XdistToGo, swapDuration);
                Data.pSprite[3] = pIt[3];
                Data.pUiGame[3] = pIt[3];
            }
            else
                haveToWait = false;
        }
        #endregion

        yield return new WaitForSeconds(swapDuration + (swapDuration * 0.05f)); //wait end of anim before switch again   
        MenuManager.Instance.playersWait[3] = false;
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
