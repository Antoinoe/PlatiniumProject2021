using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GardenManager : MonoBehaviour
{
    public float incrementValue;
    public float valueToWin;
    public Slider[] uiSliders;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i < GameManager.GetInstance().playerNbrs)
            {
                Debug.Log(GameManager.GetInstance().playersOnBoard[i].name);
                uiSliders[i].gameObject.SetActive(true);
                GameManager.GetInstance().playersOnBoard[i].AddComponent<DogProximity>();
            }
            else
                uiSliders[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slider GetPlayerSlider(int playerNbr)
    {
        return uiSliders[playerNbr];
    }
}
