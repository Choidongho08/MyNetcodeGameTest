using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Bullet clientBullet;
    [SerializeField] private Bullet serverBullet;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform gun;

    private CapsuleCollider2D collder;

    private void Awake()
    {
        collder = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
            gun.right = GetMouseDirestion();
        }
    }

    private void Fire()
    {
        Bullet bullet = Instantiate(clientBullet, firePos.position, gun.rotation);
        bullet.Init(collder);
        if (IsOwner)
        {
            FireServerRpc();
        }
    }
    [ServerRpc]
    private void FireServerRpc()
    {
        Bullet bullet = Instantiate(serverBullet, firePos.position, gun.rotation);
        bullet.Init(collder);
        FireClientRpc();
    }
    [ClientRpc]
    private void FireClientRpc()
    {
        if(!IsOwner)
            Fire();
    }

    private Vector2 GetMouseDirestion()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        direction.Normalize();
        return direction;
    }
}
