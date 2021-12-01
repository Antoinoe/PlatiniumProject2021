using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogProximity : MonoBehaviour
{
    GameObject dog;
    GardenManager manager;
    float range;
    float currProgress = 0;
    float incrementValue;
    float valueToWin;
    Slider slider;

    void Start()
    {
        dog = GameObject.FindGameObjectWithTag("SecondGoal");
        range = dog.GetComponent<AIController>().localMaxMoveRange;
        manager = FindObjectOfType<GardenManager>();
        incrementValue = manager.incrementValue;
        valueToWin = manager.valueToWin;
        slider = manager.GetPlayerSlider(gameObject.GetComponent<PlayerController>().playerNb);
        slider.value = 0;
        slider.maxValue = valueToWin;
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, dog.transform.position) <= range)
        {
            Debug.Log("Increment");
            currProgress += /*Time.deltaTime*/incrementValue;
            slider.value = currProgress;
            if (currProgress >= valueToWin)
                Win();
        }
    }

    void Win()
    {

    }
}
