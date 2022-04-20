using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using VirTest.Event;

namespace VirTest.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Button switchButton;
        [SerializeField] private Button modeButton;
        [SerializeField] private Button itemButton;
        [SerializeField] private TMP_Text modeLabel;

        private void Awake()
        {
            switchButton.onClick.AddListener(ClickSwitch);
            modeButton.onClick.AddListener(ClickMode);
            itemButton.onClick.AddListener(ClickItem);

            EventManager.AddListener<ModeChange>(OnModeChange);
        }

        private void OnModeChange(ModeChange evt)
        {
            SetModeLabel(evt.modeName);
        }

        private void ClickSwitch()
        {
            EventManager.TriggerEvent(new SwitchClick());
        }

        private void ClickMode()
        {
            EventManager.TriggerEvent(new ModeClick());
        }
        
        private void ClickItem()
        {
            EventManager.TriggerEvent(new ItemClick());
        }

        public void SetModeLabel(string text)
        {
            modeLabel.text = text;
        }

        private void OnDestroy()
        {
            switchButton.onClick.RemoveAllListeners();
            modeButton.onClick.RemoveAllListeners();
            itemButton.onClick.RemoveAllListeners();

            EventManager.RemoveListener<ModeChange>(OnModeChange);
        }
    }
}
