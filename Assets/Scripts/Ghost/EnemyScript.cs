using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform target;

    private Transform playerTransform;
    NavMeshAgent agent;
    private Behavior currentBehavior;
    private float fleeDistance = 2.0f;
    private float fleeTime = 5.0f;
    private SpriteRenderer myColor;

    public GameObject ghost;
    
    private SceneManagement sceneManagement;

    // Start is called before the first frame update
    void Start()
    {
        sceneManagement = FindAnyObjectByType<SceneManagement>();

        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        myColor = GetComponent<SpriteRenderer>();

        myColor.color = Color.white;
        //Setting the Behavior to follow so that the Ghosts start hunting
        currentBehavior = Behavior.Follow;

        //Closing/ Opening the Gates in case a Ghost was killed by the Player
        StartCoroutine(sceneManagement.OpeningTheGates());
    }

    //Checking collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch(currentBehavior)
            {
                case Behavior.Follow:
                    sceneManagement.GettingHit();
                    break;
                case Behavior.Flee:
                    sceneManagement.KillingGhosts();
                    GhostDeed();
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Checking which Behavior is active and making the Ghosts do the stuff accordingly 
        switch(currentBehavior)
        {
            case Behavior.Follow:
                PlayerFollow();
                break;
            case Behavior.Flee:
                PlayerFleeing();
                break;
        }
    }

    //Following (hunting) the Player 
    public void PlayerFollow()
    {
        agent.SetDestination(playerTransform.position);
    }

    //Running away from the Player
    public void PlayerFleeing()
    {
        // Determining where the Ghosts are gonna go (Spoiler: Away!)
        Vector3 fleeDirection = transform.position - playerTransform.position;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * fleeDistance;

        // Checking if there is a Wall (the NavMesh) in my back
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // There is a wall at my back, so now i go in the opposite direction?
            agent.SetDestination(transform.position);
        }
    }

    //This is the Method called upon by the Player script. As it happened, i was not abel to copy your script in time from the screen and thus it did not work
    //After a while of questioning my sanity as well as the functionality of my Laptop, i realized is was missing a whole Method. I then proceeded to get it to work like this
    public void PLayerFlee()
    {
        currentBehavior = Behavior.Flee;
        // Starting a Core Routine so that Behavior = Follow again once the fleeTime is over
        StartCoroutine(RevertToFollowerAfterDelay(this.fleeTime));

        //Changing Ghostst Color to signify they are now running from the PLayer
        myColor.color = new Color(0.5f, 0.7f, 1f);
    }


    // I am Fully aware that Death is written like, well, Death and not Deed, but it was late and a friend of mine commendet Death was written Deed and now it shall stay that way
    public void GhostDeed()
    {
        //Originally the Ghosts weren't supposed to be destroyed, but to teleport back onto the spawn point. For some reason, that worked most of the time but sometimes they would 
        //randomly teleport all over the map upon Death and i could not fix it.
        //A friend of mine, the same one who told me how to write Death, said: Many ways lead to Rome. So i found another way, now it works perfectly

        //Spawning a new Ghost
        Instantiate(ghost, new Vector2(0,0), Quaternion.identity);
        //Destroying the killed Ghost
        Destroy(this.gameObject);
    }

    // We wrote this during class an i am 90% certain i did not change anything, what should i be commenting on then??
    //Also i am getting sassy, i should probably not considering this is a project which i will get graded for. Welp, i have yet to find my common sense

    private IEnumerator RevertToFollowerAfterDelay(float delay)
    {
        //Delaying the script for the inputted amount of time
        yield return new WaitForSeconds(delay);
        //Returning the Behavior to follow
        currentBehavior = Behavior.Follow;
        //Returning the color to normal
        myColor.color = Color.white;
    }
}
