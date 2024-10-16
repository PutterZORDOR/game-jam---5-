using UnityEngine;

public class pararelmap : MonoBehaviour
{
    public GameObject cam; // ���ͧ��ѡ
    public float parallaxEffect; // �������Ǣͧ parallax effect
    private float length, startPos; // �纵��˹����������Ф�����Ǣͧ sprite

    void Start() {
        // �ѹ�֡���˹����������Ф�����Ǣͧ sprite ������繾����ѧ
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // ������Ǣͧ sprite
    }

    void Update() {
        // �ӹǳ�������͹���ͧ�����ѧ��� parallax effect
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // ��Ǩ�ͺ���ǹ��Ӣͧ�����ѧ ����͡��ͧ����͹����Թ���Тͧ sprite
        float cameraOffset = cam.transform.position.x * (1 - parallaxEffect);

        // �������͹���ͧ价ҧ����Թ�ͺ�ͧ sprite �����絵��˹� startPos
        if (cameraOffset > startPos + length) {
            startPos += length;
        } else if (cameraOffset < startPos - length) {
            startPos -= length;
        }
    }
}