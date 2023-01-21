using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Serialization;
using Lyceum.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;


namespace Lyceum.AR {

    public class ARMultiTrackedImageManager : MonoBehaviour {

		#region Vars

		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("m_MultiReferenceLibrary")]
		[Tooltip("The library of images which will be detected and/or tracked in the physical environment.")]
		private MultiXRReferenceImageLibrary m_SerializedMultiLibrary;

		[Header("Spawn Control")]
		[SerializeField] private float distanceToDespawn = 1;

		//Dictionaries
		private Dictionary<string, MultiXRReferenceImage> imagesReferencesDictionary = new Dictionary<string, MultiXRReferenceImage>();
		private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

		//Components
		private ARTrackedImageManager aRTrackedImageManager;


		#endregion

		#region Unity Methods

		private void Start() {
			InitializeVars();
			InitializeEvents();
			GenerateImageReferenceDictionary();

			Debug.Log("Finish Start");
		}

        private void Update() {

        }

        #endregion

        #region Initialize Methods

		private void InitializeVars() {
			aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
			DataManagerContainer.aROcclusionManager = FindObjectOfType<AROcclusionManager>();
        }

        private void InitializeEvents() {
			aRTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

		}

		private void GenerateImageReferenceDictionary() {
			if(m_SerializedMultiLibrary != null) {
				m_SerializedMultiLibrary.m_MultiImages.ForEach(imageReference => imagesReferencesDictionary.Add(imageReference.xRReferenceImage.name, imageReference));
			}
        }

		#endregion

		#region Tracked Image Events Methods

		private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
			Debug.Log("OnTrackedImagesChanged");
			eventArgs.added.ForEach(imageAdded => OnImageAdded(imageAdded));
			eventArgs.updated.ForEach(imageUpdated => OnImageUpdated(imageUpdated));
			eventArgs.removed.ForEach(imageRemoved => OnImageRemoved(imageRemoved));
		}

		private void OnImageAdded(ARTrackedImage aRTrackedImage) {
			Debug.Log("OnImageAdded");
			MultiXRReferenceImage multiXRReferenceImage = imagesReferencesDictionary[aRTrackedImage.referenceImage.name];
			if(multiXRReferenceImage == null) {
				return;
            }
			SpawnARObject(multiXRReferenceImage, aRTrackedImage);
            if (multiXRReferenceImage.exclusiveInScene) {
				DeactivateOtherAnimations(aRTrackedImage);
            }
		}

		private void OnImageUpdated(ARTrackedImage aRTrackedImage) {
			Debug.Log("OnImageUpdated");
			switch (aRTrackedImage.trackingState) {
				case UnityEngine.XR.ARSubsystems.TrackingState.Tracking:
					OnImageUpdatedTracking(aRTrackedImage);
					break;
				case UnityEngine.XR.ARSubsystems.TrackingState.Limited:
					OnImageUpdatedLimited(aRTrackedImage);
					break;
				case UnityEngine.XR.ARSubsystems.TrackingState.None:
					OnImageUpdatedNone(aRTrackedImage);
					break;
			}
		}

		private void OnImageUpdatedTracking(ARTrackedImage aRTrackedImage) {
			MultiXRReferenceImage multiXRReferenceImage = imagesReferencesDictionary[aRTrackedImage.referenceImage.name];
			if (spawnedObjects.ContainsKey(aRTrackedImage.referenceImage.name) && multiXRReferenceImage.exclusiveInScene) {
				DeactivateOtherAnimations(aRTrackedImage);
			}
			spawnedObjects[aRTrackedImage.referenceImage.name].SetActive(true);
			DataManagerContainer.ActiveOcclusion(multiXRReferenceImage.activeOcclusion);
		}

		private void OnImageUpdatedLimited(ARTrackedImage aRTrackedImage) {
			MultiXRReferenceImage multiXRReferenceImage = imagesReferencesDictionary[aRTrackedImage.referenceImage.name];
		}

		private void OnImageUpdatedNone(ARTrackedImage aRTrackedImage) {
			MultiXRReferenceImage multiXRReferenceImage = imagesReferencesDictionary[aRTrackedImage.referenceImage.name];			
		}

		private void OnImageRemoved(ARTrackedImage aRTrackedImage) {
			Debug.Log("OnImageRemoved");
			MultiXRReferenceImage multiXRReferenceImage = imagesReferencesDictionary[aRTrackedImage.referenceImage.name];
			if (spawnedObjects.ContainsKey(aRTrackedImage.referenceImage.name)) {
				spawnedObjects.Remove(aRTrackedImage.referenceImage.name);
			}
			DestroyARObject(multiXRReferenceImage);
		}

		#endregion

		#region Manage ARObjects

		private void SpawnARObject(MultiXRReferenceImage multiXRReferenceImage, ARTrackedImage aRTrackedImage) {
			Transform imageParent = aRTrackedImage.transform;
			GameObject ARObject = Instantiate(multiXRReferenceImage.prefabToSpawn, imageParent);
			ARObject.transform.CopyTransform(imageParent);
			ARObject.transform.localScale = ARObject.transform.localScale * multiXRReferenceImage.scaleMultiplier;
			multiXRReferenceImage.inScene = true;
			multiXRReferenceImage.objectInScene = ARObject;
			spawnedObjects.Add(aRTrackedImage.referenceImage.name, ARObject);
			DataManagerContainer.ActiveOcclusion(multiXRReferenceImage.activeOcclusion);
			multiXRReferenceImage.objectInScene.GetComponent<ARTrackedObjectController>().SetDebugActive(multiXRReferenceImage.debugActive);
		}

		private void DeactivateOtherAnimations(ARTrackedImage currentARTrackedImage) {
			spawnedObjects.ToList().ForEach(spawnedAnimation => {
				if(spawnedAnimation.Key != currentARTrackedImage.referenceImage.name) {
					spawnedAnimation.Value.SetActive(false);
                }
			});
        }

		private IEnumerator DestroyARObjectInSeconds(MultiXRReferenceImage multiXRReferenceImage, float seconds) {
			yield return new WaitForSeconds(seconds);
			DestroyARObject(multiXRReferenceImage);
        }

		private void DestroyARObject(MultiXRReferenceImage multiXRReferenceImage) {
			if (multiXRReferenceImage.objectInScene != null) {
				DestroyImmediate(multiXRReferenceImage.objectInScene);
			}
			multiXRReferenceImage.inScene = false;
		}

        #endregion

        #region Editor Methods
#if UNITY_EDITOR

        [ContextMenu("printCountReferenceLibrary")]
        public void printCount()
		{
			for (int i = 0; i < aRTrackedImageManager.referenceLibrary.count; i++)
			{
                InitializeVars();
                var m_material = new Material(Shader.Find("Standard"));
			    m_material.SetTexture(Shader.PropertyToID("_MainTex"), aRTrackedImageManager.referenceLibrary[i].texture as Texture) ;
                AssetDatabase.CreateAsset(m_material,"Assets/FernandoOleaDev/MultiTrackedImage(ARFoundation)/Resources/Materials/" + aRTrackedImageManager.referenceLibrary[i].name + ".mat");
            }
            
        }

        [ContextMenu("CreatPrefab")]
        public void CreatPrefab()
        {
            for (int i = 0; i < aRTrackedImageManager.referenceLibrary.count; i++)
            {
                InitializeVars();
                var m_material = new Material(Shader.Find("Standard"));
                m_material.SetTexture(Shader.PropertyToID("_MainTex"), aRTrackedImageManager.referenceLibrary[i].texture as Texture);
                AssetDatabase.CreateAsset(m_material, "Assets/FernandoOleaDev/MultiTrackedImage(ARFoundation)/Resources/Materials/" + aRTrackedImageManager.referenceLibrary[i].name + ".mat");
            }

        }

        [ContextMenu("GenerateMultiReferenceLibrary")]
		public void GenerateMultiReferenceLibrary() {
			if(aRTrackedImageManager.referenceLibrary == null) {
				Debug.LogError("SerializedLibrary not found");
				return;
            }

			if(m_SerializedMultiLibrary != null) {
				UpdateMultiReferenceLibrary();
				return;
            }

			GenerateNewMultiReferenceLibrary();
		}

		[ContextMenu("GenerateNewMultiReferenceLibrary")]
		public void GenerateNewMultiReferenceLibrary() {
			InitializeVars();
			m_SerializedMultiLibrary = ScriptableObject.CreateInstance<MultiXRReferenceImageLibrary>();
			AssetDatabase.CreateAsset(m_SerializedMultiLibrary, "Assets/SerializedMultiLibrary.asset");

			for (int i = 0; i < aRTrackedImageManager.referenceLibrary.count; i++) {
				AddMultiXRReferenceImage(i);
			}
		}

		private void AddMultiXRReferenceImage(int index) {
			MultiXRReferenceImage multiXRReferenceImage = new MultiXRReferenceImage();
			multiXRReferenceImage.xRReferenceImage = aRTrackedImageManager.referenceLibrary[index];
			m_SerializedMultiLibrary.m_MultiImages.Add(multiXRReferenceImage);
		}

		[ContextMenu("UpdateMultiReferenceLibrary")]
		public void UpdateMultiReferenceLibrary() {
			InitializeVars();
			for (int i = 0; i < aRTrackedImageManager.referenceLibrary.count; i++) {
				if (i < m_SerializedMultiLibrary.m_MultiImages.Count) {
					m_SerializedMultiLibrary.m_MultiImages[i].xRReferenceImage = aRTrackedImageManager.referenceLibrary[i];
                } else {
					AddMultiXRReferenceImage(i);
				}
			}
		}
#endif

		#endregion
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ARMultiTrackedImageManager))]
	[CanEditMultipleObjects]
	public class ARMultiTrackedImageManagerEditor : Editor {

		void OnEnable() {
			
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			
			ARMultiTrackedImageManager myTarget = (ARMultiTrackedImageManager)target;

            if (GUILayout.Button("Generate New MultiReferenceLibrary")) {
                myTarget.GenerateNewMultiReferenceLibrary();
            }

			if (GUILayout.Button("Update MultiReferenceLibrary")) {
				myTarget.UpdateMultiReferenceLibrary();
			}

            if (GUILayout.Button("Generate Material"))
            {
				myTarget.printCount();
            }
        }
	}
#endif
}