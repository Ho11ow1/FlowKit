using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlowKit.Prefabs
{
    public class CharacterDisplayManager : MonoBehaviour
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

        public readonly List<CharacterData> _characterDataMap = new List<CharacterData>();
        public int SelectedCharacterIndex { get; private set; } = 0;

        void Awake()
        {
            contentParent = gameObject.transform;

            PopulateCharacterMap();
            PopulateScrollViewContent();
            OnCharacterSelected(SelectedCharacterIndex); // Set first character as default
        }

        // ----------------------------------------------------- PRIVATE INIT METHODS -----------------------------------------------------

        private void PopulateCharacterMap()
        {
            _characterDataMap.Add(new CharacterData("Caroline", "Well, well, well...", Resources.Load<Sprite>("Sprites/Test"), 2, 2));
            _characterDataMap.Add(new CharacterData("Penelope", "Why are you here?", Resources.Load<Sprite>("Sprites/Test"), 1, 3));
            _characterDataMap.Add(new CharacterData("Emily", "Hey!", Resources.Load<Sprite>("Sprites/Test"), 2, 3));
            _characterDataMap.Add(new CharacterData("Airi", "Come here!", Resources.Load<Sprite>("Sprites/Test"), 1, 2));
        }


        private void PopulateScrollViewContent()
        {
            foreach (var character in _characterDataMap)
            {
                GameObject characterButton = Instantiate(characterButtonPrefab, contentParent);

                if (character.Sprite != null)
                {
                    characterButton.GetComponent<Button>().image.sprite = character.Sprite;
                }

                characterButton.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(_characterDataMap.IndexOf(character)));
            }
        }

        // ----------------------------------------------------- BUTTON EVENT HANDLER -----------------------------------------------------

        private void OnCharacterSelected(int index)
        {
            SelectedCharacterIndex = index;
            var characterData = _characterDataMap[SelectedCharacterIndex];
            contentTabController.OpenContentTab(ContentTabController.ContentTab.QuestLog);
            // Top values
            // Left
            nameText.text = characterData.Name;
            quoteText.text = characterData.Quote;
            // Right
            relationshipLevelText.text = characterData.RelationshipLevel.ToString();
            relationshipStageText.text = characterData.RelationshipStage.ToString();
            // Lower
            HandleRelationshipStageSlider(characterData);
            // Right side
            if (characterData.Sprite != null)
            {
                characterPortrait.sprite = characterData.Sprite;
            }
        }

        private void HandleRelationshipStageSlider(CharacterData characterData)
        {
            relationshipStageSlider.maxValue = characterData.MaxRelationshipStage;
            relationshipStageSlider.value = characterData.RelationshipStage;
        }
    }
}
