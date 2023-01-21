using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine.SceneManagement;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

public class AutoConfigurateProject : MonoBehaviour {
#if UNITY_EDITOR
    public void InstallARFoundation() {
        UnityEditor.PackageManager.Client.Add("com.unity.xr.arfoundation@4.1.7");
    }

    public void InstallARCore() {
        UnityEditor.PackageManager.Client.Add("com.unity.xr.arcore@4.1.7");
    }
    
    public void InstallARKit() {
        UnityEditor.PackageManager.Client.Add("com.unity.xr.arkit@4.1.7");
    }
#endif
}

#if UNITY_EDITOR
    [CustomEditor(typeof(AutoConfigurateProject))]
    [CanEditMultipleObjects]
    public class AutoConfigurateProjectEditor : Editor {

        void OnEnable() {

        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            //DrawDefaultInspector();

            AutoConfigurateProject myTarget = (AutoConfigurateProject)target;

            if (GUILayout.Button("Install ARFoundation")) {
                myTarget.InstallARFoundation();
            }
            
            if (GUILayout.Button("Install ARCore")) {
                myTarget.InstallARCore();
            }
            
            if (GUILayout.Button("Install ARKit")) {
                myTarget.InstallARKit();
            }
        }
    }
#endif