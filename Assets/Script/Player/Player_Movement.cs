using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;

    #region KEYBINDINGS
    KeyCode dashInput = KeyCode.Space;
    KeyCode runInput = KeyCode.LeftShift;
    #endregion

    [SerializeField] Transform sprite;
    [SerializeField] Transform face;
    [SerializeField] Transform hitboxes;

    public bool isMoving;

    #region SPEEDS
    [Header("SPEEDS")]
    float moveSpd;
    [SerializeField] float walkSpd = 5f;
    [SerializeField] float runSpd = 9f;
    #endregion

    #region DASH
    [Header("DASH")]
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] float dashStamina = 40f;
    [SerializeField] float dashDistance = 5;
    [SerializeField] float dashForce = 100;
    bool justDash = false;
    bool dashing = false;
    #endregion

    bool noMovement = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameController.Instance.enablePlayerInput)
        {
            PlayerInput();
        }
        else
        {
            movement = Vector2.zero;
        }

        isMoving = movement != Vector2.zero;


        PlayerUI.Instance.dashUI.color = new(1, 1, 1, Player_Health.Instance.stamina < dashStamina ? .5f : 1);


        if (movement.x > 0)
        {
            hitboxes.eulerAngles = new(0, 0, 0);
            sprite.localScale = new(1, 1, 1);

        }
        else if (movement.x < 0)
        {
            hitboxes.eulerAngles = new(0, 180, 0);
            sprite.localScale = new(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Handle all player input regarding movement (axis, run, dash)
    void PlayerInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement != Vector2.zero)
            face.localPosition = movement;

        if (Input.GetKeyDown(dashInput) && !justDash)
        {
            if (Player_Health.Instance.SpendStamina(dashStamina))
                dashing = true;
        }

        // Change moveSpd depending on if running or not
        moveSpd = Input.GetKey(runInput) ? runSpd : walkSpd;
    }

    void HandleMovement()
    {
        if (!noMovement)
        {
            if (dashing)
            {
                StartCoroutine(StartDashing(transform.position));
            }
            else
            {
                rb.AddForce(100f * moveSpd * Time.deltaTime * movement, ForceMode2D.Impulse);

                // Speed Control, make sure player doesn't accelerate beyond move speed
                if (rb.velocity.magnitude > moveSpd)
                {
                    rb.velocity = rb.velocity.normalized * moveSpd;
                }

                // Stops player if no input given
                if (movement == Vector2.zero)
                    rb.velocity = Vector2.zero;
            }
        }
    }

    // Dashing until certain distance
    IEnumerator StartDashing(Vector2 startPos)
    {
        dashParticle.Play();
        justDash = true;
        noMovement = true;
        Vector2 targetDir = (face.position - transform.position).normalized;
        float startTime = Time.time;

        while (Vector2.Distance(startPos, transform.position) < dashDistance && Time.time < startTime + 1)
        {
            rb.AddForce(dashForce * Time.deltaTime * targetDir, ForceMode2D.Impulse);
            PlayerUI.Instance.dashUI.fillAmount = Vector2.Distance(startPos, transform.position) / dashDistance;
            yield return null;
        }
        PlayerUI.Instance.dashUI.fillAmount = 1;
        rb.velocity = Vector2.zero;
        noMovement = false;
        justDash = false;
        dashing = false;
    }
}
