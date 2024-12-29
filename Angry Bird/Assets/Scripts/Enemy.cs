using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float currentHealth;
    [SerializeField] private GameObject deathEffect;
    
    private float minDamage = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        float damage = other.relativeVelocity.magnitude;

        if (damage < minDamage)
            return;
        
        TakeDamage(damage);
        
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
            Destroy();
        else
        {
            StartCoroutine(HitSprite());
        }
    }

    void Destroy()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        GameManager.instance.monsterCount--;
        Destroy(gameObject);
    }

    IEnumerator HitSprite()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    
}
