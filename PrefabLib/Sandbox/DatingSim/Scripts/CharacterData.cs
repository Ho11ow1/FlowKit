using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit
{
    [System.Serializable]
    public class CharacterData
    {
        public string Name { get; private set; }
        public string Quote { get; private set; }
        public Sprite Sprite { get; private set; }
        public int RelationshipLevel { get; private set; } = 1;
        public int MaxRelationshipLevel { get; private set; } = 9;
        public int RelationshipStage { get; private set; } = 0;
        public int MaxRelationshipStage { get; private set; } = 4;
        public bool IsFavourite { get; private set; } = false;
        private int[] levelsArr = null;
        private int[] stageCounterArr = null;
        public Dictionary<string, Quest> QuestMap { get; private set; } = new Dictionary<string, Quest>();
        public Dictionary<string, GalleryItem> GalleryMap { get; private set; } = new Dictionary<string, GalleryItem>();

        private static event UnityAction OnLevelChange;

        public CharacterData(string name, string quote, Sprite sprite, int relationshipLevel, int relationshipStage)
        {
            Name = name;
            Quote = quote;
            Sprite = sprite;
            RelationshipLevel = relationshipLevel;
            RelationshipStage = relationshipStage;
            OnLevelChange += UpdateMaxStageCounter;
        }

        public CharacterData(string name, string quote, Sprite sprite)
        {
            Name = name;
            Quote = quote;
            Sprite = sprite;
            OnLevelChange += UpdateMaxStageCounter;
        }

        private void UpdateMaxStageCounter()
        {
            if (levelsArr == null || stageCounterArr == null)
            {
                return;
            }

            for (int i = 0; i < levelsArr.Length; i++)
            {
                if (RelationshipLevel == levelsArr[i])
                {
                    MaxRelationshipStage = stageCounterArr[i];
                    return;
                }
            }

            MaxRelationshipStage = 4; // Back to default if no match is found
        }

        // ----------------------------------------------------- QUEST METHODS -----------------------------------------------------

        /// <summary>
        /// Adds a quest to the quest map.
        /// </summary>
        /// <param name="quest">Specifies the quest to add</param>
        public void AddQuest(Quest quest)
        {
            if (QuestMap.ContainsKey(quest.Title))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{quest.Title}' already exists for character '{Name}'. It will not be added again.");
                #endif
                return;
            }
            quest.AssignOwner(this);
            QuestMap.Add(quest.Title, quest);
        }

        /// <summary>
        /// Completes a quest identified by it's title
        /// </summary>
        /// <param name="questTitle">Specifies the title of the quest to complete</param>
        public void CompleteQuest(string questTitle)
        {
            if (!QuestMap.ContainsKey(questTitle))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{questTitle}' does not exist for character {Name}. It will not be completed");
                #endif
                return;
            }
            QuestMap[questTitle].IsCompleted = true;
            QuestMap[questTitle].IsActive = false;
        }

        /// <summary>
        /// Activates a quest identified by it's title.
        /// </summary>
        /// <param name="questTitle">Specifies the title of the quest to activate</param>
        public void ActivateQuest(string questTitle)
        {
            if (!QuestMap.ContainsKey(questTitle))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{questTitle}' does not exist for character {Name}. It will not be activated");
                #endif
                return;
            }
            QuestMap[questTitle].IsActive = true;
        }

        // ----------------------------------------------------- GALLERY METHODS -----------------------------------------------------

        /// <summary>
        /// Adds a gallery item to the gallery.
        /// </summary>
        /// <param name="item">Specifies the gallery item to add</param>
        public void AddGalleryItem(GalleryItem item)
        {
            if (GalleryMap.ContainsKey(item.Name))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Gallery Item '{item.Name}' already exists for character '{Name}'. It will not be added again.");
                #endif
                return;
            }
            item.AssignOwner(this);
            GalleryMap.Add(item.Name, item);
        }

        /// <summary>
        /// Unlocks a gallery item identified by it's name.
        /// </summary>
        /// <param name="itemName">Specifies the gallery item to be unlocked</param>
        public void UnlockGalleryItem(string itemName)
        {
            if (!GalleryMap.ContainsKey(itemName))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Gallery Item '{itemName}' does not exist for character {Name}. It will not be unlocked");
                #endif
                return;
            }
            GalleryMap[itemName].IsUnlocked = true;
        }

        // ----------------------------------------------------- ADVANCE VALUE METHODS -----------------------------------------------------

        /// <summary>
        /// Advances the relationship stage by a specified number of stages.
        /// <list type="bullet">"
        ///     <item>
        ///         <description>If relationship stage exceeds the max stage, advances level</description>
        ///     </item>
        /// </summary>
        /// <param name="stages">Specifies the amount of stage to advance by</param>
        public void AdvanceStage(int stages)
        {
            if (stages <= 0)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Cannot advance relationship stage for {Name} by {stages}. Points must be greater than zero.");
                #endif
                return;
            }

            RelationshipStage += stages;

            while (RelationshipStage >= MaxRelationshipStage)
            {
                int oldMaxStage = MaxRelationshipStage;
                AdvanceLevel(1);
                RelationshipStage -= oldMaxStage;
            }

            #if UNITY_EDITOR
            Debug.Log($"Advanced relationship stage for {Name} by {stages}. New stage: {RelationshipStage}");
            #endif
        }

        /// <summary>
        /// Advances the relationship level by a specified number of levels.
        /// </summary>
        /// <param name="levels">Specifies the amount of levels to advance by</param>
        public void AdvanceLevel(int levels)
        {
            if (levels <= 0)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Cannot advance relationship level for {Name} by {levels}. levels must be greater than zero.");
                #endif
                return;
            }

            RelationshipLevel += levels;
            OnLevelChange?.Invoke();
            if (RelationshipLevel >= MaxRelationshipLevel)
            {
                RelationshipLevel = MaxRelationshipLevel;
                #if UNITY_EDITOR
                Debug.Log($"Relationship level for {Name} has reached maximum level: {MaxRelationshipLevel}");
                #endif
            }

            #if UNITY_EDITOR
            Debug.Log($"Advanced relationship level for {Name} by {levels}. New level: {RelationshipLevel}");
            #endif
        }

        // ----------------------------------------------------- PERSONAL VALUE METHODS -----------------------------------------------------

        /// <summary>
        /// Toggles the IsFavourite flag.
        /// </summary>
        public void ToggleFavourite()
        {
            IsFavourite = !IsFavourite;
        }

        /// <summary>
        /// Specifies the maximum relationship stage based on the provided levels and stages.
        /// </summary>
        /// <param name="levels">Specifies the levels at which stage amount should change</param>
        /// <param name="stages">Specifies the amount of stages equivelant to the level</param>
        public void SetMaxStageBasedOnLevel(int[] levels, int[] stages)
        {
            if (levels == null || stages == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Found null array, aborting");
                #endif
                return;
            }
            if (levels.Length != stages.Length)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Provided arrays are not the same length, aborting");
                #endif
                return;
            }

            levelsArr = levels;
            stageCounterArr = stages;

            UpdateMaxStageCounter();
        }
    }

    [System.Serializable]
    public class Quest
    {
        public CharacterData Owner { get; private set; } = null;
        public string Title;
        public string Description;
        public bool IsActive;
        public bool IsCompleted = false;

        public Quest(string title, string description, bool isActive)
        {
            Title = title;
            Description = description;
            IsActive = isActive;
        }

        internal void AssignOwner(CharacterData owner)
        {
            Owner = owner;
        }
    }

    [System.Serializable]
    public class GalleryItem
    {
        public CharacterData Owner { get; private set; } = null;
        public string Name;
        public Sprite Sprite;
        public bool IsUnlocked = false;

        public GalleryItem(string name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;
        }

        internal void AssignOwner(CharacterData owner)
        {
            Owner = owner;
        }
    }
}
