using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] Vector2 deathFling = new Vector2(5f, 10f);
    [SerializeField] int deathTime = 1;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawn;

    Vector2 moveInput;
    
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    GameSession gameSession;
    float gravityScaleAtStart;
    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>(); 
        gameSession = FindObjectOfType<GameSession>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }
    void Update()
    {
        if(!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive){ return; }
        myAnimator.SetTrigger("Attacking");
        Instantiate(projectile, projectileSpawn.position, transform.rotation);
    }

    void OnPause(InputValue value)
    {
        if(Time.timeScale == 0)
        {
            gameSession.ResumeGame();
        }
        else
        {
            gameSession.PauseGame();
        }
    }
        void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);


    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;
        
        bool playerIsClimbing = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerIsClimbing);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath()
    {
        isAlive = false;
        myAnimator.SetTrigger("Dying");
        myRigidbody.velocity = deathFling;

        yield return new WaitForSecondsRealtime(deathTime);

        gameSession.ProcessPlayerDeath();
    }

}
