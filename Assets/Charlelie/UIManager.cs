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
    public GameObject[] uiFireObject;
    public GameObject[] fireEffect;
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
                uiImages[i].gameObject.SetActive(false);
                uiFireObject[i].gameObject.SetActive(false);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCooldownOnUI();
    }

    public Slider GetPlayerSlider(int playerNbr)
    {
        return uiSliders[playerNbr];
    }
    public Image getPlayerImage(int playerNbr)
    {
        return uiImages[playerNbr];
    }

    public GameObject GetPlayerFireEffect( int playerNbr)
    {
        return fireEffect[playerNbr];
    }
    public GameObject GetPlayerEffectObject(int playerNbr)
    {
        return uiFireObject[playerNbr];
    }

    public void SetFlammes(int playerNbr, int nbteam)
    {
        GameObject flamme = Instantiate(fireEffect[playerNbr], uiFireObject[nbteam].transform.position, Quaternion.identity);
        flamme.transform.parent = uiFireObject[nbteam].transform;
    }

    public void ActivateFireObject(int playerNbr)
    {
        uiFireObject[playerNbr].SetActive(true);
    }

    public void DesactivateFireObject(int playerNbr)
    {
        uiFireObject[playerNbr].SetActive(false);
    }

    public void UpdateCooldownOnUI(int playerNbr, float val)
    {
        uiImages[playerNbr].transform.GetChild(0).GetComponent<Image>().fillAmount = val;
    }

    public void EmptyBar(int index)
    {
        uiImages[index].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
    }
}



