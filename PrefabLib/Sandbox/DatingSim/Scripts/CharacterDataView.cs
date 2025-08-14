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

        [SerializeField] private GameObject characterButtonPrefab;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI quoteText;
        [SerializeField] private TextMeshProUGUI relationshipLevelText;
        [SerializeField] private TextMeshProUGUI relationshipStageText;
        [SerializeField] private Slider relationshipStageSlider;
        [SerializeField] private Image characterPortrait;
        [SerializeField] private Button isFavouriteToggle;

        public static Dictionary<string, CharacterData> CharacterMap { get; private set; } = new Dictionary<string, CharacterData>();
        public static string SelectedCharacterName = "Willow";

        void Awake()
        {
            contentParent = gameObject.transform;

            InitCharacterMap();
            InitCharacterGallery();
            InitCharacterStageLevels();

            RebuildScrollViewContent();
            OnCharacterSelected(SelectedCharacterName);
            isFavouriteToggle.onClick.AddListener(() => OnFavouriteTogglePressed());
        }

        // ----------------------------------------------------- PRIVATE OPTIONAL INIT METHODS -----------------------------------------------------

        /* --------------------------------------------------------------------
         *                        THESE METHODS ARE OPTIONAL.
         * 
         * The logic defined within these methods is public so you can also do it in
         * your own files if you so choose.
         * 
         * Do not forget to set the SelectedCharacterName to a valid character or else it will throw an error.
         * 
         * (These examples use a utility class for a simple and single point of modification,
         * This class can be found under FlowKit/Common/ResourcePaths.cs)
         * 
         * --------------------------------------------------------------------
         */

        private void InitCharacterMap()
        {
            //CharacterMap.Add("Willow", new CharacterData("Willow", "Well, well, well...", Resources.Load<Sprite>(Common.ResourcePaths.Portraits.Willow1)));
        }
        
        private void InitCharacterGallery()
        {
            //CharacterMap["Willow"].AddGalleryItem(new GalleryItem("Willow1", Resources.Load<Sprite>(Common.ResourcePaths.Gallery.Willow1)));
        }

        private void InitCharacterStageLevels()
        {
            //CharacterMap["Willow"].SetMaxStageBasedOnLevel(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 4, 5, 6, 7, 8, 8 });
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

        // Commented color example for debugging purposes
        private void ToggleFavouriteSprite()
        {
            isFavouriteToggle.GetComponentInChildren<Image>().sprite = CharacterMap[SelectedCharacterName].IsFavourite ?
                Resources.Load<Sprite>(Common.ResourcePaths.UI.FavouriteTrue)
                :
                Resources.Load<Sprite>(Common.ResourcePaths.UI.FavouriteFalse);
            //isFavouriteToggle.GetComponentInChildren<Image>().color = CharacterMap[SelectedCharacterName].IsFavourite ?
            //    Color.white
            //    :
            //    Color.black;
        }

    }
}
