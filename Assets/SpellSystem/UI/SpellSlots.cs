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
    }

    public SpellButton GetSlot(Slot button)
    {
        return spellButtons[(int)button];
    }

    private void Update()
    {
        var spells = SpellManager.Instance.Slots;

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (i < spells.Length)
            {
                child.SetActive(true);
                child.GetComponent<Image>().color = spells[i].wandGlowColor;
            }
            else
            {
                child.SetActive(false);
            }
        }
    }
}