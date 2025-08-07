using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlowKit.Prefabs
{
    public class ContentTabController : MonoBehaviour
    {
        [SerializeField] private CharacterDisplayManager characterDisplayManager;
        [SerializeField] private GameObject QuestPanel;
        [SerializeField] private GameObject GalleryPanel;
        [SerializeField] private GameObject TipsPanel;
        private readonly List<Button> _contentButtons = new List<Button>();
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
            SetOnClickEvents();
        }

        // ----------------------------------------------------- PRIVATE INIT METHODS -----------------------------------------------------

        private void InitContentButtons()
        {
            foreach (Transform childTransform in transform)
            {
                if (childTransform.gameObject.TryGetComponent<Button>(out Button btn))
                {
                    _contentButtons.Add(btn);
                }
            }
        }

        private void SetOnClickEvents()
        {
            foreach (Button button in _contentButtons)
            {
                if (button.name == "QuestLog")
                {
                    button.onClick.AddListener(() => OpenContentTab(ContentTab.QuestLog));
                }
                else if (button.name == "Gallery")
                {
                    button.onClick.AddListener(() => OpenContentTab(ContentTab.Gallery));
                }
                else if (button.name == "Tips")
                {
                    button.onClick.AddListener(() => OpenContentTab(ContentTab.Tips));
                }
            }
        }

        // ----------------------------------------------------- BUTTON EVENT HANDLER -----------------------------------------------------

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

        private void HandleQuestLogContent()
        {
            CharacterData characterData = characterDisplayManager._characterDataMap[characterDisplayManager.SelectedCharacterIndex];
            var textArr = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>();
            if (textArr.Length == 0) { return; }

            string tempTitle = "";
            string tempDescription = "";

            foreach (var quest in characterData.questDictionary)
            {
                if (quest.Value.isActive && !quest.Value.isCompleted)
                {
                    tempTitle = quest.Value.title;
                    tempDescription = quest.Value.description;
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

        private void HandleGalleryContent()
        {

            // Populate gallery content grid with unlocked & locked images
            // Images will handle OnClicks based on locked status
        }

        private void HandleTipsContent()
        {
            
            // Populate tips content with character-specific tips or general tips
            // If general tips are decided, remore this method and implement the statically
        }
    }
}
