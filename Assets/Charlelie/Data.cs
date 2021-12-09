using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    static public int playerNbr;
    static public int[] pSprite;
    static public int[] pUiGame;
    static public bool isDebug = true;

    static public void SetSprites(int val)
    {
        pSprite = new int[val];
        pUiGame = new int[val];
    }
}
