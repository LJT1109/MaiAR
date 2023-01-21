using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lyceum.AR{
    /// <summary>
    /// A reference image library is a collection of images to search for in
    /// the physical environment when image tracking is enabled.
    /// </summary>
    /// <remarks>
    /// Image libraries are immutable at runtime. To create and manipulate
    /// an image library via Editor scripts, see the extension methods in
    /// <see cref="XRReferenceImageLibraryExtensions"/>.
    /// If you need to mutate the library at runtime, see <see cref="MutableRuntimeReferenceImageLibrary"/>.
    /// </remarks>
    [CreateAssetMenu(fileName = "MultiReferenceImageLibrary", menuName = "XR/Multi Reference Image Library", order = 1002)]
    public class MultiXRReferenceImageLibrary : ScriptableObject {

        public List<MultiXRReferenceImage> m_MultiImages = new List<MultiXRReferenceImage>();
    }
}