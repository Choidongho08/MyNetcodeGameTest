using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
}
