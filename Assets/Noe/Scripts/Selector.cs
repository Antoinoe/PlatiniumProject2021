using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Selector : MonoBehaviour
{
    /*[HideInInspector]*/
    private GameObject[] items;
    
    private float tweenDuration = 0.25f;
    private bool canSwap = true;
    Vector3 distToGo;
    /*[SerializeField]*/ private float XdistToGo;
    [SerializeField] private int it = 0;
    [SerializeField] public Menu thisMenu;
    void Start()
    {
        Array.Resize(ref items, transform.childCount);
        for(int i = 0; i< transform.childCount; i++)
        {
            items[i] = transform.GetChild(i).gameObject;
            //print(gameObject.name + items[i].GetComponent<RectTransform>().localPosition.x);
        }
        XdistToGo = items[1].GetComponent<RectTransform>().localPosition.x - items[0].GetComponent<RectTransform>().localPosition.x;

        ScaleItems();
        //print(gameObject.name + XdistToGo);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwap && MenuManager.Instance.actualMenuOn == thisMenu)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                print("next");
                StartCoroutine(Change(false));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("prev");
                StartCoroutine(Change(true));
            }
        }
    }

    IEnumerator Change(bool input)
    {
        canSwap = false;
        print("int ienum");
        if (input)
        {
            if(it > 0)
            {
                it--;
                transform.DOLocalMoveX(transform.localPosition.x + XdistToGo, tweenDuration);
            } 
        }
        else
        {
            if (it < items.Length - 1)
            {
                it++;
                transform.DOLocalMoveX(transform.localPosition.x - XdistToGo, tweenDuration);
            }
                
        }
        
        if(thisMenu == Menu.MAP)
            ScaleItems();
        yield return new WaitForSeconds(tweenDuration);
        canSwap = true;
    }

    public void ScaleItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (it == i)
                items[i].GetComponent<RectTransform>().DOScale(new Vector2(1.3f, 1.3f), tweenDuration);
            else
                items[i].GetComponent<RectTransform>().DOScale(new Vector2(1f, 1f), tweenDuration);
        }
    }
}
