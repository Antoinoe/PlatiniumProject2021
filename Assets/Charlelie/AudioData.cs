using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum VolumeType
{
    GENERAL = 0,
    MUSIC = 1,
    FX = 2
}

public static class AudioStatic
{
    public static float general;
    public static float music;
    public static float effect;
}

public class AudioData : MonoBehaviour
{
    public void ChangeValue(int audioType)
    {
        switch ((VolumeType)audioType)
        {
            case VolumeType.GENERAL:
                AudioStatic.general = GameObject.Find("S_General").transform.GetChild(1).GetComponent<Slider>().value;
                break;

            case VolumeType.MUSIC:
                AudioStatic.music = GameObject.Find("S_Music").transform.GetChild(1).GetComponent<Slider>().value;
                break;

            case VolumeType.FX:
                AudioStatic.effect = GameObject.Find("S_Effect").transform.GetChild(1).GetComponent<Slider>().value;
                break;
        }
    }
}
