using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 10f;
    private CharacterController myCC;
    public float momentumDamping = 5f;
    public AudioClips sfx;

    public int maxHealth = 3;
    public int health;

    public Animator vignette;
    public Animator camAnim;
    private bool isWalking;
    public Animator heartsAnim;

    public bool isDead;

    [Header("Orbs")]
    public bool hasRed = false;
    public bool hasOrange = false;
    public bool hasYellow = false;
    public bool hasGreen = false;
    public bool hasBlue = false;
    public bool hasPurple = false;
    public bool hasPink = false;


    private Vector3 inputVector;
    private Vector3 movementVector;
    private readonly float myGravity = -10f;

    void Start()
    {
        isDead = false;
        health = maxHealth;
        myCC = GetComponent<CharacterController>();
    }


    void Update()
    {
        GetInput();
        MovePlayer();
        Die();
        heartsAnim.SetInteger("Health", health);
        vignette.SetInteger("Health", health);

        camAnim.SetBool("isWalking", isWalking);
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;
        }
        else
        {
            inputVector = Vector3.MoveTowards(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
        }

        movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Die()
    {
        if (health <= 0)
        {
            isDead = true;
            SceneManager.LoadScene("Game Over");
        }
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            myCC.Move(2 * Time.deltaTime * movementVector);
        }
        else
        {
            myCC.Move(movementVector * Time.deltaTime);
        }
    }
}