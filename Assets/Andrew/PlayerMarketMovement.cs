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

    [SerializeField] private MarketBooth marketBooth;

    [SerializeField] private MarketBooth applianceBooth;

    [SerializeField] private Animator fade;

    [Header("PlayerMarketMovement")]
    private Vector2 startTouchPosition;
    private bool isTouching;

    [SerializeField] private bool isInBoothRange = false;

    private void Awake()
    {
        playerRB ??= GetComponent<Rigidbody2D>();
    }
    public float horizontalValue;

    private void Update()
    {
        if (PlayerStats.playerStatsInstance.playerMarketState != PlayerStats.PlayerMarketState.Walk)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            fade.Play("FadeToBlack");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            fade.Play("FadeToClear");
        }
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

        if (Input.GetKeyDown(KeyCode.E) && isInBoothRange)
        {
            EnableStore();
        }
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

    private void EnableStore()
    {
        Interact();
    }

    private void Interact()
    {
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
        switch (PlayerStats.playerStatsInstance.shopMenuInRange)
        {
            case PlayerStats.ShopMenuInRange.Ingredient:
                marketBooth.EnableShop();
                break;

            case PlayerStats.ShopMenuInRange.Appliance:
                applianceBooth.EnableShop();
                break;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ingredients"))
        {
            isInBoothRange = true;
            PlayerStats.playerStatsInstance.shopMenuInRange = PlayerStats.ShopMenuInRange.Ingredient;
        }

        if (collision.gameObject.CompareTag("Appliances"))
        {
            isInBoothRange = true;
            PlayerStats.playerStatsInstance.shopMenuInRange = PlayerStats.ShopMenuInRange.Appliance;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ingredients") || collision.gameObject.CompareTag("Appliances"))
        {
            isInBoothRange = false;
            PlayerStats.playerStatsInstance.shopMenuInRange = PlayerStats.ShopMenuInRange.None;
        }
    }

}
