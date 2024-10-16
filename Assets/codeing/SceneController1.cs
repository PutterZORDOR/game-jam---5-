using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController1 : MonoBehaviour
{
    public static SceneController1 instance;
    [SerializeField] Animator transitionAnim;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void NextLevel() {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("start");
    }
}

