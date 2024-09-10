using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDistroyByTime : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.ApplyDamage(10);
        }
    }
}
