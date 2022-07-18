using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int size = 4;
    [SerializeField] private float spawnRadius = 25f;
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
            pool[i] = GameObject.Instantiate(enemyPrefab, GetPosition(), Quaternion.identity, transform);
            pool[i].transform.position = GetPosition();
        }
    }

    private Vector3 GetPosition()
    {
        var pos = transform.position;
        pos.x += Random.Range(-spawnRadius, spawnRadius);
        pos.z += Random.Range(-spawnRadius, spawnRadius);
        return pos;
    }

    private void SpawnEnemy()
    {
        var i = FindNextDisabledEnemy();
        if (i >= pool.Length) return;

        pool[i].SetActive(true);
        pool[i].transform.position = GetPosition();
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
