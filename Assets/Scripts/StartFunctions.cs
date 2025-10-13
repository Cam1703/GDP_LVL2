using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFunctions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("NPC");
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
