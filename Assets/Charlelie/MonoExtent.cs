using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoExtent
{
    public delegate void OnButtonClickDelegate(int val);
    public static OnButtonClickDelegate buttonClickDelegate;
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void OnValuesChanged(this MonoBehaviour mono)
    {
        buttonClickDelegate(5);
    }

}