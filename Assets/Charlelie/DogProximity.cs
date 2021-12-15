using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogProximity : MonoBehaviour
{
    GameObject dog;
    GameManager gm;
    UIManager manager;
    float range;
    float currProgress = 0;
    float incrementValue;
    float valueToWin;
    Slider slider;

    void Start()
    {
        gm = GameManager.GetInstance();
        dog = GameObject.FindGameObjectWithTag("SecondGoal");
        range = dog.GetComponent<AIController>().localMaxMoveRange;
        manager = FindObjectOfType<UIManager>();
        incrementValue = /*(manager as Test)*/manager.incrementValue;
        valueToWin = /*(manager as Test)*/manager.valueToWin;
        slider = manager.GetPlayerSlider(gameObject.GetComponent<PlayerController>().contNbr);
        slider.value = 0;
        slider.maxValue = valueToWin;
    }


    void Update()
    {
        if ((Vector2.Distance(transform.position, dog.transform.position) <= range) && !gm.isPause)
        {
            //Debug.Log("Increment");
            currProgress += /*Time.deltaTime*/incrementValue;
            slider.value = currProgress;
            if (currProgress >= valueToWin)
                Win();
        }
    }

    void Win()
    {
        GameManager.GetInstance().WinWithSecondObjective(GetComponent<PlayerController>().teamNb);
    }
}
