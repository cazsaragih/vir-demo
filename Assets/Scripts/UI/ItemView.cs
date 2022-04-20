using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemView : MonoBehaviour
{
    public string Index { get; private set; }

    [SerializeField] private TMP_Text label;

    public void SetIndex(string text)
    {
        Index = text;
        label.text = text;
    }

    public void ResetIndex()
    {
        Index = "";
        label.text = "";
    }
}
