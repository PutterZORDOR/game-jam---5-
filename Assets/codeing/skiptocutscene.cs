using UnityEngine;
using UnityEngine.SceneManagement;

public class skiptocutscene : MonoBehaviour
{
   

    public string _newGamelevel2;

    public void NewGameDialogYes() {
        SceneManager.LoadScene(_newGamelevel2);
    }
}

