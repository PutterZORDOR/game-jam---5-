using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public bool isPaused;
    [Header("UI Controller")]
    public GameObject Menu;
    public GameObject SoundMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PauseGame()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Menu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void OpenSoundMenu()
    {
        SoundMenu.SetActive(true);
    }
    public void CloseSoundMenu()
    {
        SoundMenu.SetActive(false);
    }
    public void LoadFirstScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }
}
