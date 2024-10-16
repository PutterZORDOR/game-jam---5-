using UnityEngine;

public class Bomb_Effect : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("BeginBomb");
    }
    public void lastBomb()
    {
        gameObject.SetActive(false);
    }
}
