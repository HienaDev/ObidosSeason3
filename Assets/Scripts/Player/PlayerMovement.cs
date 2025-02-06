using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode slap = KeyCode.Space;


    [SerializeField] private float movSped = 5f;
    private Vector2 velocity = Vector2.zero;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;

        if (Input.GetKey(up))
        {
            velocity.y = 1;
        }

        if (Input.GetKey(down))
        {
            velocity.y = -1;
        }

        if (Input.GetKey(right))
        {
            velocity.x = 1;
        }

        if (Input.GetKey(left))
        {
            velocity.x = -1;
        }

        rb.linearVelocity = velocity.normalized * movSped;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        bool walking = rb.linearVelocity.magnitude > 0.1f;
        _animator.SetBool("walk", walking);

        if (Input.GetKeyDown(slap))
        {
            _animator.SetTrigger("slap");
        }

        if (walking)
        {

            if (rb.linearVelocityX > 0)
                _animator.transform.right = Vector2.right;
            else
                _animator.transform.right = Vector2.left;
        }

        // Debug.Log(rb.linearVelocity.magnitude);
    }
}
