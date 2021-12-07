using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public float incrementValue;
    public float valueToWin;
    public Slider[] uiSliders;
    public Image[] uiImages;
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
                //Debug.Log(GameManager.GetInstance().playersOnBoard[i].name);
                uiSliders[i].gameObject.SetActive(true);
                GameManager.GetInstance().playersOnBoard[i].AddComponent<DogProximity>();
            }
            else
            {
                uiSliders[i].gameObject.SetActive(false);
                uiImages[i].gameObject.SetActive(true);
            }
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
    public Image getPlayerImage(int playerNbr)
    {
        return uiImages[playerNbr];
    }
}



