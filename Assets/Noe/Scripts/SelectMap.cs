using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMap : MonoBehaviour
{
    public List<GameObject> maps;
    GameObject MapSelector;

    void Start()
    {
        MapSelector.transform.Find("MapSelector");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
