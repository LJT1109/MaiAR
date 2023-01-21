using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TrackFound : MonoBehaviour
{

    public Text Count,logt;
    public static Text log;

    private void Start()
    {
        log = logt;
    }
    private void Update()
    {
        
        Count.text = FindObjectsOfType<PresentPrefab>().Length.ToString()+"\n";


    }

    public static void showlog(string txt)
    {
        log.text = txt;
    }
}
