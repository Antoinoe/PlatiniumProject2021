using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarborManager : MonoBehaviour
{
    public GameObject[] lanterns = new GameObject[4];
    int[] lanternNbr = new int[4] { 10, 11, 12, 13};

    public float timeToGetLantern;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 4; i++)
        {
            lanterns[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }


    public void SetLanternColor(int playerNbr, GameObject lantern, int nbr)
    {
        switch (playerNbr)
        {
            case 0:
                lantern.GetComponent<SpriteRenderer>().color = Color.red;
                lanternNbr[nbr] = 1;
                break;

            case 1:
                lantern.GetComponent<SpriteRenderer>().color = Color.blue;
                lanternNbr[nbr] = 2;
                break;

            case 2:
                lantern.GetComponent<SpriteRenderer>().color = Color.green;
                lanternNbr[nbr] = 3;
                break;

            case 3:
                lantern.GetComponent<SpriteRenderer>().color = Color.yellow;
                lanternNbr[nbr] = 4;
                break;

        }

        if (CheckWin()) Win(lanternNbr[0]);
    }

    bool CheckWin()
    {
        return (lanternNbr[0] == lanternNbr[1] && lanternNbr[1] == lanternNbr[2] && lanternNbr[2] == lanternNbr[3]);
    }

    void Win(int playerNbr)
    {
        Debug.Log(playerNbr + " win!");
    }

}
