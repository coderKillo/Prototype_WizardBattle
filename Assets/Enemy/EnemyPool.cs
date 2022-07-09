using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int size = 4;
    [SerializeField] private GameObject enemyPrefab;

    private GameObject[] pool;

    void Awake()
    {
        pool = new GameObject[size];
    }

    public void StartSpawn()
    {
        for (int i = 0; i < size; i++)
        {
            pool[i] = GameObject.Instantiate(enemyPrefab, GetPosition(i), Quaternion.identity, transform);
            pool[i].transform.position = GetPosition(i);
        }
    }

    private Vector3 GetPosition(int i)
    {
        var position = transform.position;
        position.x += i;
        return position;
    }

    private void SpawnEnemy()
    {
        var i = FindNextDisabledEnemy();
        if (i >= pool.Length) return;

        pool[i].SetActive(true);
        pool[i].transform.position = GetPosition(i);
    }

    private int FindNextDisabledEnemy()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeSelf)
            {
                return i;
            }
        }

        return pool.Length;
    }
}
