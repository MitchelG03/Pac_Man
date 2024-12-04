using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]

public class SceneManagement : MonoBehaviour
{
    private Label scoreTextLabel;
    private Label countdownLabel;
    private Label levelLabel;
    private int countdown;
    public int level;

    private int score;
    private int cookiesToNextLevel;
    private int cookiesCollected;

    public GameObject gameOverPanel;
    public GameObject gameWonPanel;

    private VisualElement heart1;
    private VisualElement heart2;
    private VisualElement heart3;
    private int amountOfHearts;

    private GameObject navMeshOpen;
    private GameObject navMeshClose;

    public AudioClip gettingHurt;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the Win/Loose Panels on False so that they don't pop up when the Game is started. Are there other ways to do this? Absolutly, i just prefer this
        this.gameOverPanel.SetActive(false);
        this.gameWonPanel.SetActive(false);

        this.cookiesToNextLevel = FindObjectsOfType<CookieBehavior>().Length;
        var root = GameObject.Find("ScoreUI").GetComponent<UIDocument>().rootVisualElement;
        this.scoreTextLabel = root.Q<Label>("scoreText");
        this.countdownLabel = root.Q<Label>("countdownText");
        this.levelLabel = root.Q<Label>("levelText");

        //Connecting the Hearts in the UI
        this.heart1 = root.Q<VisualElement>("Heart1");
        this.heart2 = root.Q<VisualElement>("Heart2");
        this.heart3 = root.Q<VisualElement>("Heart3");
        this.amountOfHearts = 3;

        this.scoreTextLabel.text = $"Score: {score}";
        this.levelLabel.text = $"Level {level}";
        //Starting the Countdown
        StartCoroutine(CountdownAfterGettimgHit());

        //Locating the two NavMesh objects containing the Maps
         navMeshOpen = GameObject.Find("NavMesh");
         navMeshClose = GameObject.Find("NavMeshClosed");

        //Opening the gates
        StartCoroutine(OpeningTheGates());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Application.Quit();
            Debug.Log("Imagine the game closed now, thanks!");
        }
    }
    public void KillingGhosts()
    {
        GetComponent<AudioSource>().PlayOneShot(gettingHurt);
        //Increasing the score upon murdering a poor innocent Ghost 
        this.score += 50;

        //Regaining hearts when murdering a Ghost, up to three 
        if (amountOfHearts == 1)
        {
            amountOfHearts++;
            heart2.style.opacity = 100;
        }
        else if (amountOfHearts == 2)
        {
            amountOfHearts++;
            heart1.style.opacity = 100;
        }
    }
    public void GettingHit()
    {
        GetComponent<AudioSource>().PlayOneShot(gettingHurt);
        //reducing the amount of hearts
        amountOfHearts--;

        //Resetting all Ghosts to their spawn points 
        var enemies = FindObjectsOfType<EnemyScript>();
        foreach (var enemy in enemies)
        {
            enemy.GhostDeed();
        }

        //Resetting the Player to their Spawn point
        var player = FindObjectOfType<PlayerControl>();
        player.transform.position = player.spawnPoint;

        if (amountOfHearts == 2)
        {
            //Heart 1 is removed 
            heart1.style.opacity = 0;
            //Starting Cooldown Countdown
            StartCoroutine(CountdownAfterGettimgHit());
        }
        else if (amountOfHearts == 1)
        {
            //Heart2 is removed
            heart2.style.opacity = 0;
            //Starting Cooldown Countdown
            StartCoroutine(CountdownAfterGettimgHit());
        }
        else if (amountOfHearts == 0)
        {
            //heart 3 is removed and The Game stops as the Game Over Screen appears 
            heart3.style.opacity = 0;
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Countdown to the Game starting
    private IEnumerator CountdownAfterGettimgHit()
    {
        Debug.Log("Starting Countdown");
        countdown = 3;
        // Countdown is now 3
        countdownLabel.text = $"Countdown:<br>{countdown}";
        countdown--;
        yield return new WaitForSeconds(1);
        //Countdown is now 2
        countdownLabel.text = $"Countdown:<br>{countdown}";
        countdown--;
        yield return new WaitForSeconds(1);
        //Countdown is now 1
        countdownLabel.text = $"Countdown:<br>{countdown}";
        yield return new WaitForSeconds(1);
        countdownLabel.text = "";
    }

    //Increasing the score when collecting cookies 
    public void IncreaseScore(int points)
    {
        //Increasing the score
        this.score += points;
        //Increasing the cookies collected 
        this.cookiesCollected += 1;
        //Displaying the gathered Score for the Player
        this.scoreTextLabel.text = $"Score: {score}";

        //Checking the winning condition/ if all cookies have been eaten
        if (cookiesCollected >= cookiesToNextLevel)
        {
            gameWonPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public IEnumerator OpeningTheGates()
    {
        //Closing the Gate so that Ghosts cannot get out
        navMeshClose.SetActive(true); 
        navMeshOpen.SetActive(false);

        //Waiting 3 seconds
        yield return new WaitForSeconds(3.0f);

        //Reopening the Gates so that Ghost can now get out
        navMeshOpen.SetActive(true);
        navMeshClose.SetActive(false);
    }
}
