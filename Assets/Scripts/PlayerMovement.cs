using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // determine if moving
        if (moveInput == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
        } else
        {
            animator.SetBool("isWalking", true);
        }

        // determine direction


    }
}
