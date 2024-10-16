using UnityEngine;

public class waterup : MonoBehaviour
{
    public Transform targetPoint; // �ش������·����͡����ش����Ͷ֧
    public float speed = 2f; // ��������㹡������͹���
    public Transform player; // ���˹觢ͧ������

    private bool isMoving = true; // ��Ǩ�ͺ��Һ��͡���ѧ����͹��������������

    void Update() {
        // ��Ǩ�ͺ��Һ��͡�ѧ������͹�������
        if (isMoving) {
            // ��Ѻ���͡����Ҩش������´��¤������Ƿ���˹�
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // ��Ǩ�ͺ��Һ��͡�֧�ش������������ѧ
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f) {
                // ��ش���͡����Ͷ֧�ش����˹�
                isMoving = false;
                Debug.Log("Block has reached the target point and stopped.");
            }
        }
    }

    // ��Ǩ�Ѻ��ê������ҧ���͡��м�����
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // ��ش���͡����ͪ��Ѻ������
            isMoving = false;
            Debug.Log("Block has collided with the player and stopped.");
            Time.timeScale = 0f; // ��ش��÷ӧҹ�ͧ��
        }
    }
}