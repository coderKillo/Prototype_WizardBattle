using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomModel : MonoBehaviour
{
    [SerializeField] private Transform modelPackage;

    void Start()
    {
        var randomModelIndex = Random.Range(1, modelPackage.childCount - 1);
        for (int i = 0; i < modelPackage.childCount; i++)
        {
            var active = i == randomModelIndex;
            modelPackage.GetChild(i).gameObject.SetActive(active);
        }
    }
}
