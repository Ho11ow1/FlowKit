using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlowKit
{
    public class ContentTabController : MonoBehaviour
    {
        [SerializeField] private GameObject galleryItemPrefab;
        [SerializeField] private GameObject QuestPanel;
        [SerializeField] private GameObject GalleryPanel;
        [SerializeField] private GameObject TipsPanel;
        public ContentTab CurrentTab { get; private set; } = ContentTab.QuestLog;

        public enum ContentTab
        {
            QuestLog,
            Gallery,
            Tips
        }

        void Awake()
        {
            InitContentButtons();
        }

        // ----------------------------------------------------- PRIVATE INIT METHODS -----------------------------------------------------

        private void InitContentButtons()
        {
            foreach (Transform childTransform in transform)
            {
                if (childTransform.gameObject.TryGetComponent<Button>(out Button btn))
                {
                    if (btn.gameObject.name == "QuestLog")
                    {
                        btn.onClick.AddListener(() => OpenContentTab(ContentTab.QuestLog));
                    }
                    else if (btn.gameObject.name == "Gallery")
                    {
                        btn.onClick.AddListener(() => OpenContentTab(ContentTab.Gallery));
                    }
                    else if (btn.gameObject.name == "Tips")
                    {
                        btn.onClick.AddListener(() => OpenContentTab(ContentTab.Tips));
                    }
                }
            }
        }

        // ----------------------------------------------------- TAB BUTTON HANDLER -----------------------------------------------------

        public void OpenContentTab(ContentTab tab)
        {
            if (tab == ContentTab.QuestLog)
            {
                CurrentTab = ContentTab.QuestLog;

                QuestPanel.SetActive(true);
                GalleryPanel.SetActive(false);
                TipsPanel.SetActive(false);

                HandleQuestLogContent();
            }
            else if (tab == ContentTab.Gallery)
            {
                CurrentTab = ContentTab.Gallery;

                QuestPanel.SetActive(false);
                GalleryPanel.SetActive(true);
                TipsPanel.SetActive(false);

                HandleGalleryContent();
            }
            else if (tab == ContentTab.Tips)
            {
                CurrentTab = ContentTab.Tips;

                QuestPanel.SetActive(false);
                GalleryPanel.SetActive(false);
                TipsPanel.SetActive(true);

                HandleTipsContent();
            }
        }

        // ----------------------------------------------------- CONTENT TAB HANDLERS -----------------------------------------------------

        private void HandleQuestLogContent()
        {
            CharacterData characterData = CharacterDataView.CharacterMap[CharacterDataView.SelectedCharacterName];
            var textArr = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>();
            if (textArr.Length == 0) { return; }

            string tempTitle = "";
            string tempDescription = "";

            foreach (var quest in characterData.QuestMap)
            {
                if (quest.Value.IsActive && !quest.Value.IsCompleted)
                {
                    tempTitle = quest.Value.Title;
                    tempDescription = quest.Value.Description;
                    break;
                }
            }

            foreach (var textComponent in textArr)
            {
                if (textComponent.gameObject.name == "Title")
                {
                    textComponent.text = tempTitle;
                }
                else if (textComponent.gameObject.name == "Description")
                {
                    textComponent.text = tempDescription;
                }
            }
        }

        /* --------------------------------------------------------------------
         * 
         * Modify the Resources.Load path to match your locked gallery image.
         * Or alternatively use the ResourcePaths.cs static classes
         * which can be found under FlowKit/Common/ResourcePaths.cs
         * 
         * --------------------------------------------------------------------
         */
        private void HandleGalleryContent()
        {
            CharacterData characterData = CharacterDataView.CharacterMap[CharacterDataView.SelectedCharacterName];
            var galleryContentGrid = GalleryPanel.GetComponentInChildren<GridLayoutGroup>();

            // Reset view for each character selection
            foreach (Transform childTransform in (RectTransform)galleryContentGrid.gameObject.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (var galleryItem in characterData.GalleryMap)
            {
                GameObject galleryItemObject = Instantiate(galleryItemPrefab, (RectTransform)galleryContentGrid.gameObject.transform);

                if (!galleryItem.Value.IsUnlocked)
                {
                    galleryItemObject.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(Common.ResourcePaths.Gallery.Locked);
                }
                else if (galleryItem.Value.IsUnlocked && galleryItem.Value.Sprite != null)
                {
                    galleryItemObject.GetComponentInChildren<Image>().sprite = galleryItem.Value.Sprite;
                }

                galleryItemObject.GetComponent<Button>().onClick.AddListener(() => OnGalleryItemSelected(galleryItem.Value));
            }
        }

        /* --------------------------------------------------------------------
         * 
         * Handle the gallery image selection logic in this function
         * whether that be showing a popup, enlarging the image, etc...
         * 
         * All games are different so this is left up you to handle.
         * 
         * --------------------------------------------------------------------
         */
        private void OnGalleryItemSelected(GalleryItem galleryItem)
        {
            
        }

        /* --------------------------------------------------------------------
         * 
         * Handle the Tips content logic in this function
         * 
         * As all games handle tips differently whether it be static for all items,
         * or dynamic based on the selected character.
         * 
         * This is left up to you to handle.
         * 
         * --------------------------------------------------------------------
         */
        private void HandleTipsContent()
        {
            
        }
    }
}
