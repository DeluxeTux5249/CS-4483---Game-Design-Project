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
         // Spawn player at door if moving between scenes
    if(PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY"))
    {
        float x = PlayerPrefs.GetFloat("SpawnX");
        float y = PlayerPrefs.GetFloat("SpawnY");
        transform.position = new Vector3(x, y, transform.position.z);

        PlayerPrefs.DeleteKey("SpawnX");
        PlayerPrefs.DeleteKey("SpawnY");
    }
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

            // determine direction
            gameObject.GetComponent<SpriteRenderer>().flipX = moveInput.x < 0;
        }


    }
}
