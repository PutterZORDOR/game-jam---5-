using UnityEngine;

public class Open_Skill_Pannel : MonoBehaviour
{
    [Header("Random Pannel")]
    public GameObject Random_Pannel;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (!MenuManager.instance.RandomMenu)
            {
                Open_Random_Pannel();
            }
            else if (!MenuManager.instance.ItemStuck)
            {
                Close_Random_Pannel();
            }
        }
    }

    public void Open_Random_Pannel()
    {
        Time.timeScale = 0f;
        MenuManager.instance.RandomMenu = true;
        Random_Pannel.SetActive(true);
    }
    public void Close_Random_Pannel()
    {
        MenuManager.instance.RandomMenu = false;
        Random_Pannel.SetActive(false);
        if (!MenuManager.instance.RandomMenu && !MenuManager.instance.isPaused)
        {
            Time.timeScale = 1f;
        }
    }
}
