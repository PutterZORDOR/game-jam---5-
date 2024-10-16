using UnityEngine;

public class background : MonoBehaviour
{
    public Transform player; // ������ ���ͨ�����᷹ͧ����
    public float followSpeed = 0.1f; // ��������㹡�õԴ��� ��ҷ���Өз�����ѹ����͹�������� �
    public Vector2 offset = new Vector2(0, 0); // ���˹� offset ������� background ����㹵��˹觷���������

    void Update() {
        // ��� background ��Ѻ������˹觢ͧ���������͡��ͧ �¤ӹ֧�֧ offset
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // �������͹ background ����������蹴��¤������Ƿ���������
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
