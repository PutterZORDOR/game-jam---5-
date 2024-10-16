using UnityEngine;

public class Event : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
