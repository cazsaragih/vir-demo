using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VirTest.UI
{
    public class PlatformLabel : MonoBehaviour
    {
        private TMP_Text label;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            string platform = "";
#if UNITY_STANDALONE
            platform = "Standalone build";
            label.color = Color.blue;
#elif UNITY_ANDROID
            platform = "Android build";
            label.color = Color.green;
#elif UNITY_IOS
            platform = "iOS build";
            label.color = Color.red;
#endif
            label.text = platform;
        }

    }
}