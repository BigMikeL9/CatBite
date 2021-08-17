using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    // Configs
    [Header("Chicken Configs")]
    [SerializeField] float minChickenSpeed;
    [SerializeField] float maxChickenSpeed;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int hitJumpForce = 5;
    [SerializeField] int chickenHealth = 2;
    [SerializeField] float destroyDelay= 0.3f;
    [SerializeField] int chickenKillValue = 15;
    // [SerializeField] Vector2 deathKickForce = new Vector2(25f, 25f);

    [Header("Chicken Boundaries")]
    [SerializeField] GameObject leftBoxCollider;
    [SerializeField] GameObject rightBoxCollider;

    [Header("Chicken SFX")] 
    [SerializeField] AudioClip idleSFX;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0, 1)] float chickenSFXVolume;
    
    
    // States
    bool isGoingLeft;
    bool isGoingRight = true;
    bool isAlive = true;
    
    // Cache
    Rigidbody2D _rigidbody2D;
    PolygonCollider2D _bodyCollider;
    CircleCollider2D _jumpDetectionCollider;
    Cat _cat;
    Camera _camera;
    AudioSource _audioSource;
    GameManager _gameManager;
    float _enemySpeed;

    
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodyCollider = GetComponent<PolygonCollider2D>();
        _jumpDetectionCollider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        _cat = FindObjectOfType<Cat>();
        _gameManager = FindObjectOfType<GameManager>();
        
        _camera = Camera.main;
        
        _enemySpeed = Random.Range(minChickenSpeed, maxChickenSpeed);
        // _audioSource.PlayOneShot(idleSFX, chickenSFXVolume);
    }

    private void FixedUpdate()
    {
        if (!isAlive) { return; }
        
        EnemyMove();
        JumpOverObstacle();
    }

    private void Update()
    {
        if (!isAlive) { return; }
        
        FlipSprite();
        Die();
    }
    
    private void EnemyMove()
    {
        _rigidbody2D.velocity = new Vector2(_enemySpeed, _rigidbody2D.velocity.y);
    }
    
    
    private void JumpOverObstacle()
    {
        bool isTouchingObstacle = _jumpDetectionCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle"));
        if (!isTouchingObstacle) { return; }
        
        _rigidbody2D.AddForce(Vector2.up * jumpForce);
    }
    
    
    // Control the direction the chicken sprite is facing
    private void FlipSprite()
    {
        bool isEnemyMoving = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (isEnemyMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(-_rigidbody2D.velocity.x), 1f);
        }
    }
    
    
    // Cat Damage to Chicken
    public void TakeDamage(int damageAmount)
    {
        chickenHealth -= damageAmount;
        _enemySpeed = -_enemySpeed;
        
        // chicken jump on hit
        _rigidbody2D.velocity += new Vector2(0f, hitJumpForce);
        AudioSource.PlayClipAtPoint(hitSFX, _camera.transform.position, chickenSFXVolume);
    }


    private void Die()
    {
        if (chickenHealth <= 0)
        {
            isAlive = false;
            _gameManager.ScoreUpdate(chickenKillValue);
            // var deathKickDirection = new Vector2(Mathf.Sign(-_rigidbody2D.velocity.x), Mathf.Sign(- _rigidbody2D.velocity.y));
            // _rigidbody2D.velocity = deathKickDirection * deathKickForce; 
            // transform.Rotate(0, 0, Random.Range(45, 90));
            Destroy(gameObject, destroyDelay);
        }
    }
    
    // Detects which collider the chicken hit, inorder to go in the opposite direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // Chicken Movement Boundaries
        if (otherObject == leftBoxCollider)
        {
            isGoingLeft = false;
            isGoingRight = true;
            _enemySpeed = Mathf.Abs(_enemySpeed); // Controls the chicken move direction
        }
        else if (otherObject == rightBoxCollider)
        {
            isGoingLeft = true;
            isGoingRight = false;
            _enemySpeed = -_enemySpeed; // Controls the chicken move direction
        }
    }
}
