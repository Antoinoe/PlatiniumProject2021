using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class DisplayNavTuto : MonoBehaviour
{
    public Color colorOn, colorOff;
    public GameObject selector;
    [SerializeField] private Image[] squares;
    void Start()
    {
        InitializeNav();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStateOfSquares()
    {
        for(int i = 0; i < squares.Length; i++)
        {
            if (i <= selector.GetComponent<Selector>().it)
            {
                //ON
                squares[i].transform.GetChild(0).GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), selector.GetComponent<Selector>().swapDuration);
            }
            else
            {
                //OF
                squares[i].transform.GetChild(0).GetComponent<RectTransform>().DOScale(new Vector3(0,0,0), selector.GetComponent<Selector>().swapDuration);
            }
        }
    }

    public void InitializeNav()
    {
        Array.Resize(ref squares, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            squares[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
            squares[i].name = "Off";
            squares[i].color = colorOff;
            squares[i].transform.GetChild(0).name = "On";
            squares[i].transform.GetChild(0).GetComponent<Image>().color = colorOn;
            if(i+1 == 1)
                squares[i].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            else
                squares[i].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        }
    }
}
