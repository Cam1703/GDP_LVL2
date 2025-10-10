using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject screens;

    public GameObject pauseScreen;
    public GameObject inventoryScreen;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        inventoryScreen = screens.transform.Find("InventoryScreen").gameObject;
        pauseScreen = screens.transform.Find("PauseScreen").gameObject;
    }

    private void Update()
    {
        if (Time.timeScale != 0.0f)
        {
            if (InputManager.inventoryFlag && !inventoryScreen.activeSelf)
            {
                ScreenOn(inventoryScreen);
            }
            else if (InputManager.inventoryFlag && inventoryScreen.activeSelf)
            {
                PlaySceneOn(inventoryScreen);
            }
        }
        if (InputManager.pauseFlag && !pauseScreen.activeSelf)
        {
            ScreenOn(pauseScreen);
            Time.timeScale = 0.0f;
        }
        else if (InputManager.pauseFlag && pauseScreen.activeSelf)
        {
            PlaySceneOn(pauseScreen);
            Time.timeScale = 1.0f;
        }
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ScreenOn(GameObject screen)
    { 
        screen.SetActive(true);
    }

    public void PlaySceneOn(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
