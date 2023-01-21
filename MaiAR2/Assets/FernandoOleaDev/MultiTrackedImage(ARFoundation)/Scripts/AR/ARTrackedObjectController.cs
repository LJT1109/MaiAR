using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyceum.AR {
    public class ARTrackedObjectController : MonoBehaviour {

        [SerializeField] private GameObject imageReferenceDebug;


        public void SetDebugActive(bool value) {
            imageReferenceDebug.SetActive(value);
        }
    }
}
