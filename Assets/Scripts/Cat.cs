using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cat : MonoBehaviour
{
   // Config
   [SerializeField] int enemyValue = 100;
   [SerializeField] float minDistanceToMoveToward = 5f;
   [SerializeField] float speed = 5f;

   // States
   bool isMoving;


   // Cache
   LineRenderer _lineRenderer;
   Vector3 _startingPoistion;
   GameSession _gameSession;
   Transform _nearestEnemy;
   Enemy[] _enemies;
   Rigidbody2D _rigidbody2D;

   private void Start()
   {
      _lineRenderer = GetComponent<LineRenderer>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      
      _gameSession = FindObjectOfType<GameSession>();
      _enemies = FindObjectsOfType<Enemy>();

      _startingPoistion = transform.position;
      
   }

   private void Update()
   {
      AimIndicator();
      FindClosestEnemy();
      FlipSprite();
   }
   
   private void AimIndicator()
   {
      // Line renderer starts from the ball starting position
      _lineRenderer.SetPosition(1, _startingPoistion);
      
      // Line renderer extends to the updated ball position
      _lineRenderer.SetPosition(0, transform.position);
   }

   // if cat touches enemy 
      // destroy enemy and cat
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.CompareTag("Enemy") )
      {
         // _gameSession.ScoreUpdate(enemyValue);
         Destroy(other.gameObject,2);
         Destroy(gameObject,3 );
      }
   }
   
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
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
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

      if (_nearestEnemy != null)
      {
         // Move cat toward chicken when "minDistanceToMoveToward" is reached
         float moveTowardSpeed = speed * Time.deltaTime;
         transform.position = Vector3.MoveTowards(transform.position, _nearestEnemy.transform.position, moveTowardSpeed);
         isMoving = true;

         // Debug.Log("Closest enemy is: " + _nearestEnemy + ". Distance is: " + minDistanceToClosestEnemy);
         // Debug.DrawLine(transform.position, _nearestEnemy.position, Color.red);
      }
      
   }

   private void FlipSprite()
   {
      if (isMoving)
      {
         transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
      }
   }

}
