using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class SpellActionBar : MonoBehaviour
{
    [SerializeField] private String[] spellButtons;
    // TODO: read spell buttons from Player Input Manager
    [SerializeField] private GameObject spellButtonPrefab;
    [SerializeField] private SpellManager manager;

    private GameObject mainSpellBar;
    private GameObject sideSpellBar;

    void Update()
    {
        mainSpellBar = gameObject.transform.GetChild(0).gameObject;
        sideSpellBar = gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < manager.SlotLength(); i++)
        {
            var bar = i < 2 ? sideSpellBar.transform : mainSpellBar.transform;

            if (i >= bar.childCount)
            {
                GameObject.Instantiate(spellButtonPrefab, Vector3.zero, Quaternion.identity, bar);
            }

            bar.GetChild(i).Find("Hotkey").GetComponent<TextMeshProUGUI>().text = spellButtons[i];
            bar.GetChild(i).Find("Icon").GetComponent<Image>().sprite = manager.Slot(i).icon;
        }
    }
}
