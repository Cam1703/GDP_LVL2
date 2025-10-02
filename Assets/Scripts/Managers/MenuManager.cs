using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;
    public GameObject inventory;

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
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnInventoryButton()
    { 
        inventory.SetActive(true);
    }

    public void OnPlaySceneButton()
    {
        inventory.SetActive(false);
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
