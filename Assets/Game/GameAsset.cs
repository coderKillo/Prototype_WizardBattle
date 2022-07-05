using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    static private GameAsset instance;
    static public GameAsset Instance
    {
        get
        {
            if (instance == null) instance = Instantiate(Resources.Load<GameAsset>("GameAsset")).GetComponent<GameAsset>();
            return instance;
        }
    }

    public GameObject damagePopupPrefab;
}
