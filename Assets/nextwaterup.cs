using UnityEngine;
using UnityEngine.SceneManagement; // �����ͨѴ��á������¹ Scene
public class nextwaterup : MonoBehaviour
{
    public string sceneToLoad; // ���ͧ͢ Scene ������Ŵ������������ѵ��

    // �ѧ��ѹ���ж١���¡������ա�ê�Ẻ 2D (OnTriggerEnter2D)
    private void OnTriggerEnter2D(Collider2D other) {
        // ��Ǩ�ͺ��Ҽ���誹�繼������������ (�硷����Ѻ������)
        if (other.CompareTag("Player")) {
            // ����¹ Scene ��ѧ Scene ����˹����
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
