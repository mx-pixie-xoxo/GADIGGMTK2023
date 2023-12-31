using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.5f; // Maximum speed
    [SerializeField] private float hspeed; // Horizontal Speed
    [SerializeField] private float vspeed; // Vertical Speed
    [SerializeField] private float acc = 30f; //Acceleration
    [SerializeField] private float dec = 26f; //Deceleration
    [SerializeField] private Rigidbody2D rb;
    private bool isAbleToMove = true;
    public float cookiePowerTimeMax = 10.0f;
    public float cookiePowerTimeLeft = 0.0f;
    public AudioSource audioSource;

    /**
        bool isWalking = false; 
        bool isSwinging = false; 
        bool isHurt = false;
        bool isRolling = false;
*/



    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        hspeed = 0f;
        vspeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //walking animation is currently being based off of  axis 
        float moveInputHori = Input.GetAxisRaw("Horizontal");
        float moveInputVert = Input.GetAxisRaw("Vertical");

        if ((moveInputHori == 0) && (moveInputVert == 0))
        {
            animator.SetBool("isWalking", false);
        }
        else 
        {
            animator.SetBool("isWalking", true);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isAbleToMove)
        {
            //fix jankiness
            if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(hspeed))
            {
                hspeed = rb.velocity.x;
            }
            if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(vspeed))
                vspeed = rb.velocity.y;


            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;

            up = Input.GetKey(KeyCode.W);
            down = Input.GetKey(KeyCode.S);
            left = Input.GetKey(KeyCode.A);
            right = Input.GetKey(KeyCode.D);

            //Acceleration
            if (up)
            {
                vspeed += acc * Time.deltaTime;
            }
            
            if (down)
            {
                vspeed -= acc * Time.deltaTime;
            }
            if (left)
            {
                hspeed -= acc * Time.deltaTime;
            }
            if (right)
            {
                hspeed += acc * Time.deltaTime;
            }
            
            //Deceleration
            if (!up && !down)
            {
                if (vspeed > 0f)
                {
                    vspeed -= dec * Time.deltaTime;
                    if (vspeed < 0f)
                    {
                        vspeed = 0f;
                    }
                }
                if (vspeed < 0f)
                {
                    vspeed += dec * Time.deltaTime;
                    if (vspeed > 0f)
                    {
                        vspeed = 0f;
                    }
                }
            }
            if (!left && !right)
            {
                if (hspeed > 0f)
                {
                    hspeed -= dec * Time.deltaTime;
                    if (hspeed < 0f)
                    {
                        hspeed = 0f;
                    }
                }
                if (hspeed < 0f)
                {
                    hspeed += dec * Time.deltaTime;
                    if (hspeed > 0f)
                    {
                        hspeed = 0f;
                    }
                }
            }

            //Limit Speed
            hspeed = Mathf.Clamp(hspeed, -moveSpeed, moveSpeed);
            vspeed = Mathf.Clamp(vspeed, -moveSpeed, moveSpeed);

        
            //Apply Movement
            Vector2 moveVec = new Vector3(hspeed, vspeed);
            rb.velocity = moveVec;

        }
        else
        {
            rb.velocity = new Vector3(0, 0);
        }
    }

    void OnTriggerEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "GoldenCookie")
        {
            cookiePowerTimeLeft = cookiePowerTimeMax;
            audioSource.Play();
            Destroy(other.gameObject);
        }
    }
}
