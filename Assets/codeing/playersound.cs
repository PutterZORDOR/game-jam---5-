using UnityEngine;

public class playersound : MonoBehaviour
{
    public Animator animator; // �������͡Ѻ Animator ���������͹����ѹ
    public float attackCooldown = 0.5f; // �������ҷ������ö��������駶Ѵ�
    private float nextAttackTime = 0f;

    void Update() {
        // ��Ǩ�ͺ��Ҽ����蹡�����������������Ҿͷ��������ա������
        if (Time.time >= nextAttackTime && Input.GetButtonDown("Fire1")) {
            Attack(); // ���¡��ѧ��ѹ�������
            nextAttackTime = Time.time + attackCooldown; // ��駤�� cooldown ����Ѻ������դ��駶Ѵ�
        }
    }

    void Attack() {
        // ���¡��ҹ�͹����ѹ����
        animator.SetTrigger("Attack");

        // ������§�������� AudioManager
        AudioManager.instance.PlaySFX("Attack_Sound");
    }
}
