using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CatHandler : MonoBehaviour
{
    // Configs
    [SerializeField] GameObject catPrefab;
    [SerializeField] Rigidbody2D pivotPoint;
    [SerializeField] float detachBallDelay = 1f;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float maxDragDistance = 2f;
    // [SerializeField] float destroyDelay;

    // states
    bool isDragging;

    // Cache
    Rigidbody2D _currentBallRb;
    SpringJoint2D _currentBallSj;
    GameObject _catInstance;
    Camera _camera;

    private void Start()
    {
        RespawnCat();
        
        _camera = Camera.main;
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
                LauchCat();
            }
            
            // Hides the AimIndicator when we're not touching the screen
            _catInstance.GetComponent<LineRenderer>().enabled = false;
            isDragging = false;
            return;
        }
        
        isDragging = true;
        
        // Shows the AimIndicator when touching the screen
        _catInstance.GetComponent<LineRenderer>().enabled = true;
        
        // Reads the touch position on the screen
        var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        
        // Converts touchPosition from screenSpace coordinates to worldSpace coordinates. 
        Vector2 worldPosition = _camera.ScreenToWorldPoint(touchPosition);
        
        // if distance between our touchPosition and the hook position is greater than maxDragDistance
        if (Vector3.Distance(worldPosition, pivotPoint.position) > maxDragDistance)
        {
            // then prevent the player from dragging any further
                // ".normalized" is to make the "(current Ball position - pivot point position)" equal to 1.
                    // (current hook position) + (current Ball position - pivot point position).normalized * (max allowed drag distance)

                    _currentBallRb.position = pivotPoint.position + (worldPosition - pivotPoint.position).normalized * maxDragDistance;
        }
        else
        {
            // Changes ball position in relation to where we touch the screen
            _currentBallRb.position = worldPosition;
        }
        
        // Sets Rb to kinematic when touching the ball
        _currentBallRb.isKinematic = true;
    }
    
    
    private void LauchCat()
    {
        // sets the _currentBallRb rb back to Dynamic
        _currentBallRb.isKinematic = false;
            
        // prevents the ball from snapping back to the touch location once we launch the ball
        _currentBallRb = null;
        
        // detaches the ball after a delay
        Invoke(nameof(DetachCat), detachBallDelay);
        
        // StartCoroutine(DestroyCat());
    }

    private void DetachCat()
    {
        // deactivate the currentBallSpringJoint to launch the ball once we're not touching the screen
        _currentBallSj.enabled = false;
        _currentBallSj = null;
        
        // spawns the ball after it launches; after a delay
        Invoke(nameof(RespawnCat), spawnDelay);
        
    }

    private void RespawnCat()
    {
        _catInstance = Instantiate(catPrefab, pivotPoint.position, Quaternion.identity);


        // sets the rb to be equal to that of the instantiated ball's rb.
        _currentBallRb = _catInstance.GetComponent<Rigidbody2D>();
        _currentBallSj = _catInstance.GetComponent<SpringJoint2D>();
        
        // this line will attach the ball to the pivot
        _currentBallSj.connectedBody = pivotPoint;
        
    }
    

    // destroys the instantiated ball after a delay
    // IEnumerator DestroyCat()
    // {
    //     yield return new WaitForSeconds(destroyDelay);
    //     Destroy(_ballInstance);
    //     
    // }
    
 
}
