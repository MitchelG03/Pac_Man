using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class CookieBehavior : MonoBehaviour
{
    public AudioClip collectbleSound;

    public int cookieValue = 1;
    private SceneManagement sceneManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //Finding the SceneManagement to call upon the IncreaseScore function
        this.sceneManager = FindObjectOfType<SceneManagement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            // Increase the score when eating a cookie
            this.sceneManager?.IncreaseScore(cookieValue);

            // Play the collectibles sounds
            GetComponent<AudioSource>().PlayOneShot(collectbleSound);

            //Hide the cookie by disabling the renderer
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            //Destroy the cookie AFTER the sound has played
            GameObject.Destroy(gameObject, collectbleSound.length);
        }
    }
}
