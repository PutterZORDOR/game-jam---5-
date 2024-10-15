using UnityEngine;

public class Open_Skill_Pannel : MonoBehaviour
{
    [Header("Random Pannel")]
    public GameObject Random_Pannel;

    public void Open_Random_Pannel()
    {
        Random_Pannel.SetActive(true);
    }
    public void Close_Random_Pannel()
    {
        Random_Pannel.SetActive(false);
    }
}
