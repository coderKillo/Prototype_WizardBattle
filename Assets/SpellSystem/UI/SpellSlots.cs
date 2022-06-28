using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Slot : int
{
    PRIMARY,
    SECONDARY,
    MAX
}

public class SpellSlots : MonoBehaviour
{
    [SerializeField] private SpellButton[] spellButtons = new SpellButton[(int)Slot.MAX];

    static private SpellSlots instance = null;
    static public SpellSlots Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public SpellButton GetButton(Slot button)
    {
        return spellButtons[(int)button];
    }

    public void SetSlotColor(Color color, int index)
    {
        if (index >= transform.childCount) return;

        var child = transform.GetChild(index).gameObject;

        child.SetActive(true);
        child.GetComponent<Image>().color = color;
    }
}
