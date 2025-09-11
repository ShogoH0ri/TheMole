using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 30f;
    public float spawnTime = 3f;
    private Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Enemy"))
        {
            hitTransform.GetComponent<Enemy>().TakeDamage(10);
        }
            Destroy(gameObject, spawnTime);
    }
}