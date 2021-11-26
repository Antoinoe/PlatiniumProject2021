using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private GameObject[] playerUIList;

    private GameObject reloadObject;
    private GameObject secondaryObject;

    private Image reloadSlider;
    private Image secondarySlider;

    [Header("Slider Values")]
    [Range(0, 1)]
    public float reloadValue;
    [Range(0, 1)]
    public float secondaryValue;

    void Start()
    {
        int nbOC = transform.childCount;
        Array.Resize(ref playerUIList, nbOC);
    }

}
