using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using VirTest.Event;

namespace VirTest.UI
{
    public class CameraUI : MonoBehaviour
    {
        [SerializeField] private Button switchButton;
        [SerializeField] private Button modeButton;
        [SerializeField] private TMP_Text modeLabel;

        private void Awake()
        {
            switchButton.onClick.AddListener(ClickSwitch);
            modeButton.onClick.AddListener(ClickMode);

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

        public void SetModeLabel(string text)
        {
            modeLabel.text = text;
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<ModeChange>(OnModeChange);
        }
    }
}
