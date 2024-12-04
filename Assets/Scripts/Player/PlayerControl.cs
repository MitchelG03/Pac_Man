using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]


public class PlayerControl : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    private Rigidbody2D playerRigidbody2D;
    private Animator playerAnimator;
    private string lastTriger = "TriggerRight"; // Assumes starting facing right

    public Vector2 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the Spawn Point
        transform.position = spawnPoint;

        //Connecting the Animator and RigidBpdy to this scrip
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checking if collided with Power Pill
        if (other.CompareTag("PowerPill"))
        {
            //Making the Ghosts run away
            var enemies = FindObjectsOfType<EnemyScript>();
            foreach (var enemy in enemies)
            {
                enemy.PLayerFlee();
            }
        }
        //Checking which Prtal has been entered and then spittimg the Player out at the other side
        else if (other.CompareTag("PortalRight"))
        {
            this.transform.position = new Vector2(-11.5f,0);
        }
        else if (other.CompareTag("PortalLeft"))
        {
            this.transform.position = new Vector2(11.5f,0);
        }
    }


    void FixedUpdate()
    {
        //Letting the Player control the Avatar
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        this.MovePlayer(moveHorizontal, moveVertical);
        this.AnimatePlayer(moveHorizontal, moveVertical);
    }

    private void MovePlayer(float moveHorizontal, float moveVertical)
    {
        Vector2 movment = new Vector2(moveHorizontal, moveVertical);
        playerRigidbody2D.MovePosition(playerRigidbody2D.position + movment * playerSpeed * Time.deltaTime);
    }

    private void AnimatePlayer(float moveHorizontal, float moveVertical)
    {
        //Setting the Animations
        string newTrigger = DetermineDirection(moveHorizontal, moveVertical);
        if (!string.IsNullOrEmpty(newTrigger) && lastTriger != newTrigger)
        {
            playerAnimator.SetTrigger(newTrigger);
            lastTriger = newTrigger;
        }
    }

    private string DetermineDirection(float moveHorizontal, float moveVertical) 
    {
        //Setting the string which impacts the Animation
        if (moveHorizontal > 0) return "TriggerRight";
        if (moveHorizontal < 0) return "TriggerLeft";
        if (moveVertical > 0) return "TriggerUp";
        if (moveVertical < 0) return "TriggerDown";
        return null;
    }

}
