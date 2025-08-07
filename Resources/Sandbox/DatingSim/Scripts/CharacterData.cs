using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit.Prefabs
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
        public Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();

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
                if (RelationshipLevel != levelsArr[i])
                {
                    continue;
                }
                MaxRelationshipStage = stageCounterArr[i];
                return;
            }
        }

        // ----------------------------------------------------- QUEST METHODS -----------------------------------------------------

        public void AddQuest(Quest quest)
        {
            if (questDictionary.ContainsKey(quest.title))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{quest.title}' already exists for character '{Name}'. It will not be added again.");
                #endif
                return;
            }
            questDictionary.Add(quest.title, quest);
        }

        public void CompleteQuest(string questTitle)
        {
            if (!questDictionary.ContainsKey(questTitle))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{questTitle}' does not exist. It will not be completed");
                #endif
                return;
            }
            questDictionary[questTitle].isCompleted = true;
            questDictionary[questTitle].isActive = false;
        }

        public void ActivateQuest(string questTitle)
        {
            if (!questDictionary.ContainsKey(questTitle))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Quest '{questTitle}' does not exist. It will not be activated");
                #endif
                return;
            }
            questDictionary[questTitle].isActive = true;
        }

        // ----------------------------------------------------- ADVANCE VALUE METHODS -----------------------------------------------------

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

            while (RelationshipStage > MaxRelationshipStage)
            {
                AdvanceLevel(1);
                RelationshipStage -= MaxRelationshipStage;
            }

            RelationshipStage += stages;
            #if UNITY_EDITOR
            Debug.Log($"Advanced relationship stage for {Name} by {stages}. New stage: {RelationshipStage}");
            #endif
        }

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
            if (RelationshipLevel > MaxRelationshipLevel)
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

        public void ToggleFavourite()
        {
            IsFavourite = !IsFavourite;
        }

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
        public string title;
        public string description;
        public bool isCompleted;
        public bool isActive;
        public Quest(string title, string description, bool isActive)
        {
            this.title = title;
            this.description = description;
            this.isActive = isActive;
            isCompleted = false;
        }
    }
}
