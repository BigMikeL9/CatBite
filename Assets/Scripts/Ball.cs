using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ball : MonoBehaviour
{
   // Config
   [SerializeField] int enemyValue = 100;
   
   
   // Cache
   LineRenderer _lineRenderer;
   Vector3 _startingPoistion;
   CircleCollider2D _circleCollider2D;
   GameSession _gameSession;

   private void Start()
   {
      _lineRenderer = GetComponent<LineRenderer>();
      _circleCollider2D = GetComponent<CircleCollider2D>();

      _gameSession = FindObjectOfType<GameSession>();

      _startingPoistion = transform.position;
   }

   private void Update()
   {
      AimIndicator();
   }
   
   private void AimIndicator()
   {
      // Line renderer starts from the ball starting position
      _lineRenderer.SetPosition(1, _startingPoistion);
      
      // Line renderer extends to the updated ball position
      _lineRenderer.SetPosition(0, transform.position);
   }

   // if ball touches enemy 
      // destroy enemy and ball
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.CompareTag("Enemy") )
      {
         // _gameSession.ScoreUpdate(enemyValue);
         Destroy(other.gameObject);
         Destroy(gameObject);
      }
   }
}
