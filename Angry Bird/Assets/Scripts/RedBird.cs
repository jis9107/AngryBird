using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedBird : MonoBehaviour
{
    [SerializeField] private GameObject bombEffect;
    public enum BirdType
    {
        red,
        black
    }
    
    public BirdType birdType;
    
    private Rigidbody2D rigid;
    
    void start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // StartCoroutine(Deceleration());
        // Debug.Log("감속시작");
    }

    void Update()
    {
        if (birdType == BirdType.black && Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
    }

 
    
    
    void Explode()
    {
        float explosionRadius = 1.2f;
        float explosionForce = 15f;
        float upWardForce = 5f;
        
        // Ray를 이용해 주변에 폭발 반경에 닿는 콜라이더를 구한다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 폭발 중심으로부터 방향과 거리 계산
                Vector2 direction = rb.transform.position - transform.position;
                float distance = direction.magnitude;
                
                // 거리에 따른 힘 감소
                float forceFalloff = 1 - (distance / explosionRadius);
                Vector2 force = direction.normalized * (explosionForce * forceFalloff);
                force += Vector2.up * upWardForce;
                
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        Instantiate(bombEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.instance.selectBird = true;
        GameManager.instance.birdCount--;
        GameManager.instance.CheckCount();
        
    }
}
