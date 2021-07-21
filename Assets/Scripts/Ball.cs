using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
   // Cache
   LineRenderer _lineRenderer;
   Vector3 _startingPoistion;

   private void Start()
   {
      _lineRenderer = GetComponent<LineRenderer>();

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
   
}
