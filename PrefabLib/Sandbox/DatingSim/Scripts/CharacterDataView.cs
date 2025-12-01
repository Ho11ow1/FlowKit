using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlowKit
{
    public class CharacterDataView : MonoBehaviour
    {
        private Transform contentParent;
        [SerializeField] private ContentTabController contentTabController;

        [Header("UI References")]
        [SerializeField] private GameObject characterButtonPrefab;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI quoteText;
        [SerializeField] private TextMeshProUGUI relationshipLevelText;
        [SerializeField] private TextMeshProUGUI relationshipStageText;
        [SerializeField] private Slider relationshipStageSlider;
        [SerializeField] private Image characterPortrait;
        [SerializeField] private Button isFavouriteToggle;

        public static Dictionary<string, CharacterData> CharacterMap { get; private set; } = new Dictionary<string, CharacterData>();
        public static string SelectedCharacterName;

        void Start()
        {
            SelectedCharacterName = CharacterMap.Keys.FirstOrDefault<string>();
            contentParent = gameObject.transform;

            InitCharacterMap();
            InitCharacterGallery();
            InitCharacterStageLevels();

            RebuildScrollViewContent();
            OnCharacterSelected(SelectedCharacterName);
            isFavouriteToggle.onClick.AddListener(() => OnFavouriteTogglePressed());
        }

        // ----------------------------------------------------- PRIVATE EXAMPLE INIT METHODS -----------------------------------------------------

        /* --------------------------------------------------------------------
         *                   THESE METHODS ARE EXAMPLES.
         * 
         * The logic defined within these methods is and example and only serves
         * as a way to show how to use the CharacterData class.
         * 
         * --------------------------------------------------------------------
         */

        private void InitCharacterMap()
        {
            //CharacterMap.Add("Willow", new CharacterData("Willow", "Well, well, well...", Resources.Load<Sprite>(ResourceData.Portraits.Willow1)));
        }
        
        private void InitCharacterGallery()
        {
            //CharacterMap["Willow"].AddGalleryItem(new GalleryItem("Willow1", Resources.Load<Sprite>(ResourceData.Gallery.Willow1)));
        }

        private void InitCharacterStageLevels()
        {
            //CharacterMap["Willow"].SetMaxStageBasedOnLevel(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 4, 5, 6, 7, 8, 9 });
        }

        // ----------------------------------------------------- CHARACTER BUTTON HANDLERS -----------------------------------------------------

        private void RebuildScrollViewContent()
        {
            // Clear content for rebuilding
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var character in CharacterMap.OrderByDescending(character => character.Value.IsFavourite))
            {
                GameObject characterButton = Instantiate(characterButtonPrefab, contentParent);

                _ = characterButton.TryGetComponent<Button>(out Button btn);
                Image[] imageArr = characterButton.GetComponentsInChildren<Image>();

                imageArr[2].gameObject.SetActive(character.Value.IsFavourite);

                if (character.Value.Sprite != null)
                {
                    imageArr[1].sprite = character.Value.Sprite;
                }

                btn.onClick.AddListener(() => OnCharacterSelected(character.Key));
            }
        }

        private void OnCharacterSelected(string characterName)
        {
            SelectedCharacterName = characterName;
            var characterData = CharacterMap[SelectedCharacterName];
            contentTabController.OpenContentTab(ContentTabController.ContentTab.QuestLog);
            // Top values
            // Left
            nameText.text = characterData.Name;
            quoteText.text = characterData.Quote;
            // Right
            relationshipLevelText.text = characterData.RelationshipLevel.ToString();
            relationshipStageText.text = characterData.RelationshipStage.ToString();
            // Lower
            relationshipStageSlider.maxValue = characterData.MaxRelationshipStage;
            relationshipStageSlider.value = characterData.RelationshipStage;
            // Right side
            if (characterData.Sprite != null)
            {
                characterPortrait.sprite = characterData.Sprite;
            }
            ToggleFavouriteSprite();
        }

        // ----------------------------------------------------- FAVOURITE TOGGLE HANDLERS -----------------------------------------------------

        private void OnFavouriteTogglePressed()
        {
            CharacterMap[SelectedCharacterName].ToggleFavourite();
            RebuildScrollViewContent();
            ToggleFavouriteSprite();
        }

        private void ToggleFavouriteSprite()
        {
            isFavouriteToggle.GetComponentInChildren<Image>().sprite = CharacterMap[SelectedCharacterName].IsFavourite ?
                Resources.Load<Sprite>(ResourceData.Instance.ui.SetAsFavourite)
                :
                Resources.Load<Sprite>(ResourceData.Instance.ui.NotSetAsFavourite);
        }
    }
}
