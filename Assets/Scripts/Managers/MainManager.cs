using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager instance;

    public static MenuManager menuManager;
    //public static PauseManager pauseManager;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(this);

        //uiManager = GetComponent<UIManager>();
        menuManager = GetComponent<MenuManager>();
        //pauseManager = GetComponent<PauseManager>();
        //_soundManager = GetComponent<SoundManager>();
    }

}