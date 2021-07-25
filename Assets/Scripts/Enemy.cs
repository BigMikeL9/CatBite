using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Configs
    [SerializeField] float enemySpeed = 3f;
    [SerializeField] GameObject leftBoxCollider;
    [SerializeField] GameObject rightBoxCollider;
    
    // States
    bool isGoingLeft;
    bool isGoingRight = true;
    
    // Cache
    Rigidbody2D _rigidbody2D;
    PolygonCollider2D _polygonCollider2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        EnemyMove();
        FlipSprite();
    }
    
    private void EnemyMove()
    {
        var isTouchingLayers = _polygonCollider2D.IsTouchingLayers(LayerMask.GetMask("Cat"));
        // if cat and chicken are touching --> then dont continue with the rest of the code
        if (isTouchingLayers) { return; } // ****************** FIX THISSS TOMORRROWW ***************
        _rigidbody2D.velocity = new Vector2(enemySpeed, _rigidbody2D.velocity.y);
    }

    // Detects which collider the chicken hit, inorder to go in the opposite direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == leftBoxCollider)
        {
            isGoingLeft = false;
            isGoingRight = true;
            enemySpeed = Mathf.Abs(enemySpeed); // Controls the chicken move direction
        }
        else if (other.gameObject == rightBoxCollider)
        {
            isGoingLeft = true;
            isGoingRight = false;
            enemySpeed = -enemySpeed; // Controls the chicken move direction
        }
    }

    // Control the direction the chicken sprite is facing
    private void FlipSprite()
    {
        transform.localScale = new Vector3(Mathf.Sign(-_rigidbody2D.velocity.x), 1f);
    }
}
