using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour {
    
    [SerializeField] private int frameRate = 30;
    void Start()
    {
        Application.targetFrameRate = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
