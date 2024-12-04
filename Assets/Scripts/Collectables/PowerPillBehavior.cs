using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class PowerPillBehavior : MonoBehaviour
{
    public AudioClip collectbleSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
