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

    private Camera mainCamera;

    [Header("PlayerMarketMovement")]
    private Vector2 startTouchPosition;
    private bool isTouching;

    private void Awake()
    {
        playerRB ??= GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        playerAnim.runtimeAnimatorController = PlayerStats.playerStatsInstance.GetPlayerRuntimeController();
    }

    public float horizontalValue;

    private void Update()
    {
        if (PlayerStats.playerStatsInstance.playerMarketState != PlayerStats.PlayerMarketState.Walk)
            return;

        if (GamePlatformChecker.gamePlatformInstance.deviceType == GamePlatformChecker.DeviceType.Mobile)
        {
            HandleMobileInput();
        }
        else if (GamePlatformChecker.gamePlatformInstance.deviceType == GamePlatformChecker.DeviceType.WebGL)
        {
            HandlePCInput();
        }
    }

    private void HandlePCInput()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (horizontalValue < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalValue > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            playerRB.velocity = Vector2.zero;
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount <= 0)
        {
            horizontalValue = 0;
            playerRB.velocity = Vector2.zero;
            return;
        }

        Touch touch = Input.touches[0];
        if (mainCamera.ScreenToWorldPoint(touch.position).x < transform.position.x)
        {
            horizontalValue = -1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            horizontalValue = 1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        Move(horizontalValue);
    }
    //move the player
    private void Move(float horizontalValue)
    {
        if (horizontalValue == 0 || PlayerStats.playerStatsInstance.playerMarketState == PlayerStats.PlayerMarketState.InMenu)
        {
            playerAnim.SetBool("isMoving", false);
            return;
        }

        playerRB.AddForce(new Vector2(horizontalValue * playerSpeed, 0), ForceMode2D.Force);
        playerAnim.SetBool("isMoving", true);

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
            PlayerStats.playerStatsInstance.shopMenuInRange = PlayerStats.ShopMenuInRange.Ingredient;
        }

        if (collision.gameObject.GetComponent<MarketBooth>())
        {
            PlayerStats.playerStatsInstance.shopMenuInRange = PlayerStats.ShopMenuInRange.Ingredient;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MarketBooth>())
        {
            //collision.gameObject.GetComponent<MarketBooth>().ExitShop();
        }

        if (collision.gameObject.GetComponent<ApplianceBooth>())
        {
            //collision.gameObject.GetComponent<ApplianceBooth>().ExitShop();
        }
    }

}
