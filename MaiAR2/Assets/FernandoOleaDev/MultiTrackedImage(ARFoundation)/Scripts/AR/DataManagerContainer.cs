using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static class DataManagerContainer
{
    public static AROcclusionManager aROcclusionManager;

    public static void ActiveOcclusion(bool value) {
        aROcclusionManager.enabled = value;
    }
}
