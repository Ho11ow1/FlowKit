using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlowKit.Prefabs
{
    public class InterfaceToggle : MonoBehaviour
    {
        [SerializeField] private GameObject DatingInterface;

        void Awake()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => ToggleInterface());
        }

        private void ToggleInterface()
        {
            DatingInterface.SetActive(!DatingInterface.activeInHierarchy);
        }
    }
}
