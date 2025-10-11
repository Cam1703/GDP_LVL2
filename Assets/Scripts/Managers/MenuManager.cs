using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject screens;

    public GameObject pauseScreen;
    public GameObject inventoryLayout;
    public GameObject inventoryScreens;

    int screensCount;
    int selected;

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

        inventoryLayout = screens.transform.Find("InventoryLayout").gameObject;
        inventoryScreens = inventoryLayout.transform.Find("InveontoryScreens").gameObject;
        screensCount = inventoryScreens.transform.childCount;

        pauseScreen = screens.transform.Find("PauseScreen").gameObject;
    }

    private void Update()
    {
        if (Time.timeScale != 0.0f)
        {
            if (InputManager.inventoryFlag && !inventoryLayout.activeSelf)
            {
                ScreenOn(inventoryLayout);
                inventoryScreens.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (InputManager.inventoryFlag && inventoryLayout.activeSelf)
            {
                PlaySceneOn(inventoryLayout);
                inventoryScreens.transform.GetChild(selected).gameObject.SetActive(false);

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

    public void InventoryForward()
    {
        for (int i = 0; i < screensCount; i++)
        {
            //Debug.Log(i);
            if (inventoryScreens.transform.GetChild(i).gameObject.activeSelf)
            {
                inventoryScreens.transform.GetChild(i).gameObject.SetActive(false);
                if (i < (screensCount - 1))
                {
                    selected = i + 1;
                }
                else
                {
                    selected = 0;   
                }
                inventoryScreens.transform.GetChild(selected).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void InventoryBackward()
    {
        for (int i = (screensCount - 1); i >= 0; i--)
        {
            //Debug.Log(i);
            if (inventoryScreens.transform.GetChild(i).gameObject.activeSelf)
            {
                inventoryScreens.transform.GetChild(i).gameObject.SetActive(false);
                if (i > 0 )
                {
                    selected = i - 1;
                }
                else
                {
                    selected = screensCount - 1;
                }
                inventoryScreens.transform.GetChild(selected).gameObject.SetActive(true);
                break;
            }
        }
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
