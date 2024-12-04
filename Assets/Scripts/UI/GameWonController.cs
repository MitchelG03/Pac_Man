using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameWonController : MonoBehaviour
{

    private Button continueBtn;
    private int level;
    private SceneManagement sceneManagement;

    void OnEnable()
    {   
        //Connecting the UI overlay to this script
        var root = GetComponent<UIDocument>().rootVisualElement;
        this.continueBtn = root.Q<Button>("Continue");
        this.continueBtn.RegisterCallback<ClickEvent>(OnContinueButtonClicked);

        //Connecting to the SceneManagement to get the current level;
        this.sceneManagement = FindObjectOfType<SceneManagement>();
        level = sceneManagement.level;
    }

    //Determining what the Button is gonna do upon being clicked 
    private void OnContinueButtonClicked(ClickEvent continueEvent)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1f;
        Debug.Log("Btn clicked");
    }
}
