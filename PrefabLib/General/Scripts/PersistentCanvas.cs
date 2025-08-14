using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowKit
{
    public class PersistentCanvas : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
