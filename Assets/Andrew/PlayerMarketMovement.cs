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


        //if (GamePlatformChecker.gamePlatformInstance.deviceType == GamePlatformChecker.DeviceType.WebGL)
        //{
            HandlePCInput();
        //}
    }

    private void HandlePCInput()
    {
        Debug.Log("PC Input");
        horizontalValue = Input.GetAxis("Horizontal");
    }

    private void HandleMobileInput()
    {

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
        //if (playerRB.velocity.magnitude > maxVelocity)
        //{
        //    playerRB.velocity = playerRB.velocity.normalized * maxVelocity;
        //}
    }

}
