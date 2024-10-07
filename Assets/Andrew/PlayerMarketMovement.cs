using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script by Andrew
/// </summary>
public class PlayerMarketMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private float playerSpeed;

    [SerializeField] private float maxVelocity;

    [Header("PlayerMarketMovement")]
    private Vector2 startTouchPosition;
    private bool isTouching;

    private void Awake()
    {
        playerRB ??= GetComponent<Rigidbody2D>();
        //playerAnim ??= GetComponent<Animator>();
    }
    public float horizontalValue;

    private void Update()
    {

        if (GamePlatformChecker.gamePlatformInstance.deviceType == GamePlatformChecker.DeviceType.Mobile)
        {
            HandleMobileInput();
        }


        if (GamePlatformChecker.gamePlatformInstance.deviceType == GamePlatformChecker.DeviceType.WebGL)
        {
            HandlePCInput();
        }
    }

    private void HandlePCInput()
    {
        Debug.Log("PC Input");
        horizontalValue = Input.GetAxis("Horizontal");
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.touches[0]; // Get the first touch

        if (touch.phase == TouchPhase.Began)
        {
            //Record the starting touch position
            startTouchPosition = touch.position;
            isTouching = true;
        }
        else if (touch.phase == TouchPhase.Moved && isTouching)
        {
            //Calculate the horizontal movement direction
            float horizontalDelta = touch.position.x - startTouchPosition.x;
            //Move the object left or right based on horizontalDelta
            transform.Translate(horizontalDelta * playerSpeed * Time.deltaTime, 0, 0);
            //Update the starting touch position for continuous movement
            startTouchPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            //Reset the flag when the touch ends
            isTouching = false;
        }

    }
    private void FixedUpdate()
    {
        Move(horizontalValue);
    }

    //move the player
    private void Move(float horizontalValue)
    {
        playerRB.AddForce(new Vector2(horizontalValue * playerSpeed, 0), ForceMode2D.Force);

        //clamp the velocity of the player
        if (playerRB.velocity.magnitude > maxVelocity)
        {
            playerRB.velocity = playerRB.velocity.normalized * maxVelocity;
        }
    }

}
