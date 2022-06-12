using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spark")]
public class SparkSpell : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float damage = 10f;

    public override void CastSpell(SpellManager manager)
    {
    }
}