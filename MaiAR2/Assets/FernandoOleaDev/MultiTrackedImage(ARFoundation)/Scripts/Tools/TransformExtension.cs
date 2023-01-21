using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyceum.Tools {
    public static class TransformExtension {

        public static void CopyTransform(this Transform transform, Transform other, bool isLocal = false) {
            if (isLocal) {
                transform.localPosition = other.localPosition;
                transform.localRotation = other.localRotation;
            } else {
                transform.position = other.position;
                transform.rotation = other.rotation;
            }
            transform.localScale = other.localScale;

        }

    }
}