using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float upwardsSpeed;
    [SerializeField] private float disappearTimer;
    [SerializeField] private float disappearSpeed;

    private TextMeshPro textMesh;

    static public DamagePopup Create(Vector3 location, int damage)
    {
        Debug.Log("create " + damage);
        var popup = GameObject.Instantiate(GameAsset.Instance.damagePopupPrefab, location, Quaternion.identity);
        var damagePopup = popup.GetComponent<DamagePopup>();

        damagePopup.Setup(damage);

        return damagePopup;
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    void Setup(int damage)
    {
        textMesh.text = damage.ToString();
    }

    void Update()
    {
        Debug.Log("update");
        transform.position += new Vector3(0, upwardsSpeed, 0) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            textMesh.alpha -= disappearSpeed * Time.deltaTime;
            if (textMesh.alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
