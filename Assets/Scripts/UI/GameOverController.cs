using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private Button tryAgainBtn;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Connecting the UI Overlay and most importantly the Button
        var root = GetComponent<UIDocument>().rootVisualElement;
        this.tryAgainBtn = root.Q<Button>("TryAgain");
        this.tryAgainBtn.RegisterCallback<ClickEvent>(OnTryAgainButtonClicked);
    }

    //Setting what the Button is supposed to do upon clicked
    private void OnTryAgainButtonClicked(ClickEvent tryAgainEvent)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Debug.Log("Button clicked");
    }
}
