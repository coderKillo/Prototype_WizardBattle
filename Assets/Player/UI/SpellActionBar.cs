using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class SpellActionBar : MonoBehaviour
{
    [SerializeField] private SpellButton[] mainSpellButtons;
    [SerializeField] private SpellButton[] sideSpellButtons;
    [SerializeField] private GameObject spellButtonPrefab;

    private GameObject mainSpellBar;
    private GameObject sideSpellBar;

    void Update()
    {
        mainSpellBar = gameObject.transform.GetChild(0).gameObject;
        sideSpellBar = gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < mainSpellButtons.Length; i++)
        {
            if (i >= mainSpellBar.transform.childCount)
            {
                GameObject.Instantiate(spellButtonPrefab, Vector3.zero, Quaternion.identity, mainSpellBar.transform);
            }

            mainSpellBar.transform.GetChild(i).Find("Hotkey").GetComponent<TextMeshProUGUI>().text = mainSpellButtons[i].hotkey;
            mainSpellBar.transform.GetChild(i).Find("Icon").GetComponent<Image>().sprite = mainSpellButtons[i].icon;
        }

        for (int i = 0; i < sideSpellButtons.Length; i++)
        {
            if (i >= sideSpellBar.transform.childCount)
            {
                GameObject.Instantiate(spellButtonPrefab, Vector3.zero, Quaternion.identity, sideSpellBar.transform);
            }

            sideSpellBar.transform.GetChild(i).Find("Hotkey").GetComponent<TextMeshProUGUI>().text = sideSpellButtons[i].hotkey;
            sideSpellBar.transform.GetChild(i).Find("Icon").GetComponent<Image>().sprite = sideSpellButtons[i].icon;
        }
    }
}
