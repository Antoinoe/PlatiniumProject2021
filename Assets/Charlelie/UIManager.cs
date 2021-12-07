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
        UpdateCooldownOnUI();
    }

    public Slider GetPlayerSlider(int playerNbr)
    {
        return uiSliders[playerNbr];
    }
    public Image getPlayerImage(int playerNbr)
    {
        return uiImages[playerNbr];
    }

    void UpdateCooldownOnUI()
    {
        for(int i = 0; i< GameManager.GetInstance().playerNbrs; i++)
        {
            uiImages[i].transform.GetChild(0).GetComponent<Image>().fillAmount += Mathf.Clamp(Time.deltaTime,0,1);
            
        }
    }

    public void EmptyBar(int index)
    {
        uiImages[index].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
    }
}



