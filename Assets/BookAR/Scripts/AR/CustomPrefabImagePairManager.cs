using System;
using System.Collections.Generic;
using Scenes.BookAR.Scripts;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;


namespace BookAR.Scripts.AR
{
    /// <summary>
    /// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
    /// and overlays some prefabs on top of the detected image.
    /// </summary>
    public class CustomPrefabImagePairManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Used to associate an `XRReferenceImage` with a Prefab by using the `XRReferenceImage`'s guid as a unique identifier for a particular reference image.
        /// </summary>
        [Serializable]
        struct NamedPrefab
        {
            // System.Guid isn't serializable, so we store the Guid as a string. At runtime, this is converted back to a System.Guid
            public string imageGuid;
            public GameObject imagePrefab;
            

            public NamedPrefab(Guid guid, GameObject prefab)
            {
                imageGuid = guid.ToString();
                imagePrefab = prefab;
            }
        }
        

        public bool ExtendedTrackableImageMode = true;
        [SerializeField]
        [HideInInspector]
        List<NamedPrefab> m_PrefabsList = new List<NamedPrefab>();

        public Dictionary<Guid, GameObject> m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
        Dictionary<Guid, GameObject> m_Instantiated = new Dictionary<Guid, GameObject>();

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        /// <summary>
        /// Get the <c>XRReferenceImageLibrary</c>
        /// </summary>
        public XRReferenceImageLibrary imageLibrary
        {
            get => m_ImageLibrary;
            set => m_ImageLibrary = value;
        }

        public void OnBeforeSerialize()
        {
            m_PrefabsList.Clear();
            foreach (var kvp in m_PrefabsDictionary)
            {
                m_PrefabsList.Add(new NamedPrefab(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
            foreach (var entry in m_PrefabsList)
            {
                m_PrefabsDictionary.Add(Guid.Parse(entry.imageGuid), entry.imagePrefab);
            }
        }
        


        public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
            => m_PrefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;



#if UNITY_EDITOR
        /// <summary>
        /// This customizes the inspector component and updates the prefab list when
        /// the reference image library is changed.
        /// </summary>
        [CustomEditor(typeof(CustomPrefabImagePairManager))]
        class PrefabImagePairManagerInspector : Editor
        {
            List<XRReferenceImage> m_ReferenceImages = new List<XRReferenceImage>();
            bool m_IsExpanded = true;

            bool HasLibraryChanged(XRReferenceImageLibrary library)
            {
                if (library == null)
                    return m_ReferenceImages.Count == 0;

                if (m_ReferenceImages.Count != library.count)
                    return true;

                for (int i = 0; i < library.count; i++)
                {
                    if (m_ReferenceImages[i] != library[i])
                        return true;
                }

                return false;
            }

            public override void OnInspectorGUI()
            {
                //customized inspector
                var behaviour = serializedObject.targetObject as CustomPrefabImagePairManager;

                serializedObject.Update();
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
                }

                var libraryProperty = serializedObject.FindProperty(nameof(m_ImageLibrary));
                EditorGUILayout.PropertyField(libraryProperty);
                var library = libraryProperty.objectReferenceValue as XRReferenceImageLibrary;

                //check library changes
                if (HasLibraryChanged(library))
                {
                    if (library)
                    {
                        var tempDictionary = new Dictionary<Guid, GameObject>();
                        foreach (var referenceImage in library)
                        {
                            tempDictionary.Add(referenceImage.guid, behaviour.GetPrefabForReferenceImage(referenceImage));
                        }
                        behaviour.m_PrefabsDictionary = tempDictionary;
                    }
                }

                // update current
                m_ReferenceImages.Clear();
                if (library)
                {
                    foreach (var referenceImage in library)
                    {
                        m_ReferenceImages.Add(referenceImage);
                    }
                }

                //show prefab list
                m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, "Prefab List");
                if (m_IsExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUI.BeginChangeCheck();

                        var tempDictionary = new Dictionary<Guid, GameObject>();
                        foreach (var image in library)
                        {
                            var prefab = (GameObject) EditorGUILayout.ObjectField(image.name, behaviour.m_PrefabsDictionary[image.guid], typeof(GameObject), false);
                            tempDictionary.Add(image.guid, prefab);
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Update Prefab");
                            behaviour.m_PrefabsDictionary = tempDictionary;
                            EditorUtility.SetDirty(target);
                        }
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}
