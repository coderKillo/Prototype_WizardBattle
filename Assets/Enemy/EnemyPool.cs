using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int size = 4;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private GameObject enemyPrefab;

    private GameObject[] pool;
    private float timePassed = 0f;

    void Start()
    {
        pool = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            pool[i] = GameObject.Instantiate(enemyPrefab, GetPosition(i), Quaternion.identity, transform);
            pool[i].SetActive(false);
        }
    }

    private Vector3 GetPosition(int i)
    {
        var position = transform.position;
        position.x += i;
        return position;
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= spawnTime)
        {
            timePassed = 0f;
            SpawnEnemy();
        }
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
