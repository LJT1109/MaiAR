using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using System;

namespace Lyceum.AR{
	[Serializable]
	public class MultiXRReferenceImage {

		[Tooltip("Image reference from ARTrackedImageManager.referenceLibrary")]
		public XRReferenceImage xRReferenceImage;
		[Tooltip("Prefab to spawn when image is tracked")]
		public GameObject prefabToSpawn;
		[Tooltip("Image reference being tracked in scene")]
		[HideInInspector]
		public bool inScene;
		[Tooltip("Object spawned in scene")]
		[HideInInspector]
		public GameObject objectInScene;
		[Tooltip("Multiplier to apply to a object scale when spawn")]
		public float scaleMultiplier = 1;
		[Tooltip("Active or Deactive Occlusion when spawn")]
		public bool activeOcclusion;
		[Tooltip("If exclusive deactive all others objects in scene.")]
		public bool exclusiveInScene;
		[Tooltip("If active, image reference show in scene")]
		public bool debugActive;
	}
}