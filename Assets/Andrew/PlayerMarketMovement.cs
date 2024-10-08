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

    [SerializeField] private ApplianceBooth applianceBooth;

    [SerializeField] private Animator fade;

    private enum PlayerMarketState
    {
        Walk,
        InMenu,
    }

    private enum ShopMenuInRange
    {
        None = 0,
        Ingredient,
        Appliance,
    }

    private PlayerMarketState playerMarketState;
    private ShopMenuInRange shopMenuInRange = 0;

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
        if (playerMarketState != PlayerMarketState.Walk)
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

        if (Input.GetKeyDown(KeyCode.E))
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
        StartCoroutine(Interact());
    }

    private IEnumerator Interact()
    {

        switch (shopMenuInRange)
        {
            case ShopMenuInRange.Ingredient:
                fade.Play("FadeToBlack");
                yield return new WaitForSeconds(1);
                marketBooth.EnableShop();
                fade.Play("FadeToClear");
                yield return new WaitForSeconds(1);
                break;

            case ShopMenuInRange.Appliance:
                fade.Play("FadeToBlack");
                yield return new WaitForSeconds(1);
                applianceBooth.EnableShop();
                fade.Play("FadeToClear");
                yield return new WaitForSeconds(1);
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
        if (collision.gameObject.GetComponent<MarketBooth>())
        {
            shopMenuInRange = ShopMenuInRange.Ingredient;
        }

        if (collision.gameObject.GetComponent<MarketBooth>())
        {
            shopMenuInRange = ShopMenuInRange.Ingredient;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MarketBooth>())
        {
            collision.gameObject.GetComponent<MarketBooth>().ExitShop();
        }

        if (collision.gameObject.GetComponent<ApplianceBooth>())
        {
            collision.gameObject.GetComponent<ApplianceBooth>().ExitShop();
        }
    }

}
