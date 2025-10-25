using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartFunctions : MonoBehaviour
{

    public GameObject nowButton = null;
    private Vector3 nowScale = Vector3.one;
    
    private const float selectionScaleFactor = 1.2f;

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
    }
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
