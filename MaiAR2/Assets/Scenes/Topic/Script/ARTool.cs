using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;
using System;



public class ARTool : MonoBehaviour
{
    public ARTrackedImageManager m_TrackedImageManager;
    public bool istracking;
    Coroutine coroutine;

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        TrackFound.showlog(eventArgs.ToString());
        istracking = true;
        StopAllCoroutines();
        coroutine = StartCoroutine(CountDown(1f, () => {
            istracking = false;
            TrackFound.showlog("none");
            coroutine = null;
            foreach (var item in FindObjectsOfType<AR_Show_Prefab>())
            {
                item.gameObject.SetActive(false);
            }
        }));
    }

    private void Update()
    {
       
    }

    IEnumerator CountDown(float time,Action callback)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(0.1f);
            time -= 0.1f;
        }
        callback.Invoke();

    }

    public void testCountDown()
    {
        TrackFound.showlog("start");
        StartCoroutine(CountDown(3f, () => { TrackFound.showlog("End"); }));
    }

}
