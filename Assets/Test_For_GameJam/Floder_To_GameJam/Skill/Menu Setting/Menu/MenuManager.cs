using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public bool isPaused;
    public bool RandomMenu;
    public bool ItemStuck;
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
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
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
        isPaused = false;
        if (!RandomMenu && !isPaused)
        {
            Time.timeScale = 1f;
        }
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
