using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cat : MonoBehaviour
{
   // Config
   [Header("Cat Configs")]
   [SerializeField] float speed = 5f;
   [SerializeField] float jumpForce = 10f;
   [SerializeField] float minDistanceToMoveToward = 5f;
   [SerializeField] int enemyValue = 100;
   

   // States
   bool _isMoving;
   bool _isTouchingChicken;

   // Cache
   LineRenderer _lineRenderer;
   Vector3 _startingPoistion;
   Transform _nearestEnemy;
   Enemy[] _enemies;
   Rigidbody2D _rigidbody2D;
   PolygonCollider2D _bodyCollider;
   CircleCollider2D _jumpDetectionCollider;
   Animator _animator;
   float distance;
   GameSession _gameSession;

   private void Start()
   {
      _lineRenderer = GetComponent<LineRenderer>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _bodyCollider = GetComponent<PolygonCollider2D>();
      _jumpDetectionCollider = GetComponent<CircleCollider2D>();
      _animator = GetComponent<Animator>();

      _gameSession = FindObjectOfType<GameSession>();
      _enemies = FindObjectsOfType<Enemy>();

      _startingPoistion = transform.position;
   }

   private void Update()
   {
      AimIndicator();
      FindClosestEnemy();
      MoveTowardNearestEnemy();
      CatJump();
      FlipSprite();
   }
   
   private void AimIndicator()
   {
      // Line renderer starts from the ball starting position
      _lineRenderer.SetPosition(1, _startingPoistion);
      
      // Line renderer extends to the updated ball position
      _lineRenderer.SetPosition(0, transform.position);
   }
    
         // Tomorrow: *********************
   // Each cat find only one enemy to kill --> after that there is a null reference error because that enemy is no
   // longer there. Solution: - Make the cat idle after it kills one enemy and then disappear after a delay.
   private void FindClosestEnemy()
   {
      float minDistanceToClosestEnemy = minDistanceToMoveToward;
      _nearestEnemy = null;

      foreach (var enemy in _enemies)
      {
         // if there is an enemy in scene
         if (enemy != null)
         {
            // gets the distance between cat and enemy
            distance = Vector3.Distance(transform.position, enemy.transform.position);
            // if the distance between them is smaller than ... (minDistanceToClosestEnemy)
            if (distance < minDistanceToClosestEnemy)
            {
               // then set the nearest enemy to the closest one
               _nearestEnemy = enemy.transform;
               // the new distance is the closest one
               minDistanceToClosestEnemy = distance;
            }
         } 
         else
         {
            return;
         }
         
      }
   }
   
   private void MoveTowardNearestEnemy()
   {
      _isTouchingChicken = _bodyCollider.IsTouchingLayers(LayerMask.GetMask("Chicken"));
      
      // Attacking Animation
      _animator.SetBool("isAttacking", _isTouchingChicken);
      
      // if cat and chicken are touching --> then dont continue with the rest of the code
      if(_isTouchingChicken) { return; }
      
      if (_nearestEnemy != null)
      {
         // Move cat toward chicken when "minDistanceToMoveToward" is reached
         float moveTowardSpeed = speed * Time.deltaTime;
         transform.position = Vector3.MoveTowards(transform.position, _nearestEnemy.transform.position, moveTowardSpeed);
         _isMoving = true;

         // Debug.Log("Closest enemy is: " + _nearestEnemy + ". Distance is: " + minDistanceToClosestEnemy);
         Debug.DrawLine(transform.position, _nearestEnemy.position, Color.red);
      }
      else
      {
         _isMoving = false;
      }
      // Running Animation
      _animator.SetBool("isRunning", _isMoving);
   }

   
   // This method is called by an Animation Event
   public void Attack(int damage)
   {
      // if cat IS touching obstacle --> then dont continue with the rest of the code
      // if cat is NOT touching chicken --> then dont continue with rest of the code
      if (!_isTouchingChicken) { return; }

      Enemy enemy = _nearestEnemy.GetComponent<Enemy>();
      enemy.TakeDamage(damage);
   }

   private void CatJump()
   {
      bool isTouchingObstacle = _jumpDetectionCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle"));
      if (!isTouchingObstacle) { return; }
        
      _rigidbody2D.AddForce(Vector2.up * jumpForce);
   }
   
   
   private void FlipSprite()
   {
      if (_isMoving && _nearestEnemy != null)
      {
         // gets the position of the cat in local space, relative to the closest enemy
         Vector3 relativePoint = transform.InverseTransformPoint(_nearestEnemy.transform.position);
         
         // if relativePoint is negative --> then that means that enemy is behind cat
         if (relativePoint.x < 0)
         {
            // thus flip the sprite
            transform.localScale = new Vector3(Mathf.Sign(relativePoint.x), 1f);
         }
      }
   }

}
