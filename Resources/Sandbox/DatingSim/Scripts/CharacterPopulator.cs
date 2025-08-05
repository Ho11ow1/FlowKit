using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlowKit.Prefabs
{
    public class CharacterPopulator : MonoBehaviour
    {
        [SerializeField] private GameObject characterButtonPrefab;
        private Transform contentParent;

        void Awake()
        {
            contentParent = gameObject.transform;
        }

        void Start()
        {
            PopulateScrollViewContent();
        }

        private void PopulateScrollViewContent()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject newButton = Instantiate(characterButtonPrefab, contentParent);

                // Add click functionality
                Button button = newButton.GetComponent<Button>();
                int characterIndex = i; // Capture for closure
                button.onClick.AddListener(() => OnCharacterSelected(characterIndex));
            }
        }

        void OnCharacterSelected(int index)
        {
            Debug.Log("Selected Character: " + index);
            // This is where you'll update the middle and right panels later
        }
    }
}
