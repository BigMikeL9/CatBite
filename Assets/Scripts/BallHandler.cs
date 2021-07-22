using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class BallHandler : MonoBehaviour
{
    // Configs
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivotPoint;
    [SerializeField] float detachBallDelay = 1f;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float destroyDelay;

    // states
    bool isDragging;
    
    // Cache
    Rigidbody2D _currentBallRb;
    SpringJoint2D _currentBallSj;
    GameObject _ballInstance;
    
    
    private void Start()
    {
        RespawnBall();
    }

    void Update()
    {
        TouchControl();
    }


    private void TouchControl()
    {
        // prevents us form getting errors if we touch the screen while the ball is launched, due to it being null
        if (_currentBallRb == null) { return; }
        
        // If we're not touching the screen, then don't continue with the rest of the code below
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LauchBall();
            }
            
            // Hides the AimIndicator when we're not touching the screen
            _ballInstance.GetComponent<LineRenderer>().enabled = false;
            isDragging = false;
            return;
        }
        
        isDragging = true;
        
        // Shows the AimIndicator when touching the screen
        _ballInstance.GetComponent<LineRenderer>().enabled = true;
        
        // Reads the touch position on the screen
        var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        
        // Converts touchPosition from screenSpace coordinates to worldSpace coordinates. 
        var worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        // Changes ball position in relation to where we touch the screen
        _currentBallRb.position = worldPosition;
        
        // Sets Rb to kinematic when touching the ball
        _currentBallRb.isKinematic = true;
    }
    
    
    private void LauchBall()
    {
        // sets the _currentBallRb rb back to Dynamic
        _currentBallRb.isKinematic = false;
            
        // prevents the ball from snapping back to the touch location once we launch the ball
        _currentBallRb = null;
        
        // detaches the ball after a delay
        Invoke(nameof(DetachBall), detachBallDelay);
        
        StartCoroutine(DestroyBall());
    }

    private void DetachBall()
    {
        // deactivate the currentBallSpringJoint to launch the ball once we're not touching the screen
        _currentBallSj.enabled = false;
        _currentBallSj = null;
        
        // spawns the ball after it launches; after a delay
        Invoke(nameof(RespawnBall), spawnDelay);
        
    }

    private void RespawnBall()
    {
        if (_ballInstance == null)
        {
            _ballInstance = Instantiate(ballPrefab, pivotPoint.position, Quaternion.identity);


            // sets the rb to be equal to that of the instantiated ball's rb.
            _currentBallRb = _ballInstance.GetComponent<Rigidbody2D>();
            _currentBallSj = _ballInstance.GetComponent<SpringJoint2D>();
        
            // this line will attach the ball to the pivot
            _currentBallSj.connectedBody = pivotPoint;
        }
        
    }

    // destroys the instantiated ball after a delay
    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(_ballInstance);
        
    }
    
 
}
