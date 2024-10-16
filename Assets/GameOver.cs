using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void Restart()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
        SceneManager.LoadScene(0);
    }
}
