using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit
{
    public class ResourceData : MonoBehaviour
    {
        public static ResourceData Instance;

        [Header("Resources.Load paths | must be a Sprite")]
        public UI ui;
        [Header("Resources.Load paths | must be a Sprite")]
        public Gallery gallery;
        [Header("Method calls for Interface events")]
        public MethodCalls methodCalls;

        [System.Serializable]
        public class GalleryItemEvent : UnityEvent<GalleryItem> {}
        [System.Serializable]
        public class HandleTipsEvent : UnityEvent<CharacterData> {}

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [System.Serializable]
        public class UI
        {
            public string SetAsFavourite = "";
            public string NotSetAsFavourite = "";
        }

        [System.Serializable]
        public class Gallery
        {
            public string LockedItem = "";
        }

        [System.Serializable]
        public class MethodCalls
        {
            [Header("Method Call for Gallery Item Selection | \nmust take in (FlowKit.GalleryItem)")]
            [Tooltip("Assign methods to call when a gallery item is selected,\n" +
                     "Handle this as desired, such as showing a popup or enlarging the image.")]
            public GalleryItemEvent OnGalleryItemSelected;

            [Header("Method Call for Handling Tips Content | \nmust take in (FlowKit.CharacterData)")]
            [Tooltip("Assign methods to call when the tips menu is selected,\n" +
                     "Either show static tips or dynamic tips based on the selected character.")]
            public HandleTipsEvent HandleTips;
        }
    }
}
