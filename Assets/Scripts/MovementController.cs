using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    private Rigidbody2D rb;
    private CollisionController coll;
    //Player Movement Status
    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;

    //Bolleans
    private bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<CollisionController>();
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        MoveController();
        JumpController();
    }

    void JumpController(){
        if (Input.GetButtonDown("Jump"))
        {
            if (coll.onGround){
                //GetComponent<BetterJumping>().enabled = true;
                Jump(Vector2.up);  
            }
                

        }
    }

    void MoveController(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }
}