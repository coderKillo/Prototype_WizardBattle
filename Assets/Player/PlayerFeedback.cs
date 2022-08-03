using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] private MMFeedbacks hitFeedback;

    void Start()
    {
        PlayerHealth.OnHit += OnHit;

    }

    void OnHit()
    {
        hitFeedback.PlayFeedbacks();
    }
}
