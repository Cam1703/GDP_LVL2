using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject screens;

    private GameObject pauseScreen;
    private GameObject inventoryLayout;
    private GameObject inventoryScreens;

    public GameObject forward;
    public GameObject backward;

    public GameObject languageScreen;
    public GameObject buttons;

    private float buttonCooldown;
    private const float buttonThreshold = 0.4f;
    private int screensCount;
    private int selected;

    private GameObject nowButton = null;
    private Vector3 nowScale = Vector3.one;
    private const float selectionScaleFactor = 1.2f;
    private GameObject initialButton;

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
        initialButton = EventSystem.current.firstSelectedGameObject;
        
        buttons = inventoryLayout.transform.Find("Buttons").gameObject;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != nowButton)
        {
            if (nowButton != null)
            {
                nowButton.transform.localScale = nowScale;
            }
            nowButton = EventSystem.current.currentSelectedGameObject;
            nowScale = nowButton.transform.localScale;
            nowButton.transform.localScale *= selectionScaleFactor;
        }

        if (Time.timeScale != 0.0f)
        {
            if (InputManager.inventoryOnFlag)
            {
                ScreenOn(inventoryLayout);
                inventoryScreens.transform.GetChild(0).gameObject.SetActive(true);
                InputManager._playerInput.SwitchCurrentActionMap("UI");

                buttonCooldown = buttonThreshold + 0.1f;
                EventSystem.current.SetSelectedGameObject(initialButton);
            }
            else if (InputManager.inventoryOffFlag)
            {
                PlaySceneOn(inventoryLayout);
                inventoryScreens.transform.GetChild(selected).gameObject.SetActive(false);
                InputManager._playerInput.SwitchCurrentActionMap("Player");

            }
            else if (inventoryLayout.activeSelf)
            {   
                if (buttonCooldown > buttonThreshold)
                {
                    if (EventSystem.current.currentSelectedGameObject == forward && InputManager.navigation.x == 1)
                    {
                        buttonCooldown = 0.0f;
                        InventoryForward();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == backward && InputManager.navigation.x == -1)
                    {
                        buttonCooldown = 0.0f;
                        InventoryBackward();
                    }
                }
                else
                {
                    buttonCooldown += Time.deltaTime;
                }
            }
        }
        
        if (InputManager.pauseOnFlag)
        {
            ScreenOn(pauseScreen);
            Time.timeScale = 0.0f;
            InputManager._playerInput.SwitchCurrentActionMap("UI");
        }
        else if (InputManager.pauseOffFlag)
        {
            PlaySceneOn(pauseScreen);
            Time.timeScale = 1.0f;
            InputManager._playerInput.SwitchCurrentActionMap("Player");
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
