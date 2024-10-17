using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class endtostart : MonoBehaviour
{
    public Image imageToShow;         // �ٻ�Ҿ�����ʴ�
    public float displayTime = 5f;    // ���ҷ���ͧ����ʴ��Ҿ (�Թҷ�)
    public string nextSceneName;      // ���ͧ͢ Scene �Ѵ价���ͧ�������¹�

    private void Start() {
        // ������鹡�÷ӧҹ�ͧ Coroutine �����ʴ��Ҿ�������¹�չ
        StartCoroutine(DisplayImageAndChangeScene());
    }

    private IEnumerator DisplayImageAndChangeScene() {
        // �ʴ��ٻ�Ҿ
        imageToShow.gameObject.SetActive(true);

        // �����ҵ������˹�
        yield return new WaitForSeconds(displayTime);

        // ��͹�ٻ�Ҿ��������Ҥú
        imageToShow.gameObject.SetActive(false);

        // ����¹��ѧ Scene ����
        SceneManager.LoadScene(nextSceneName);
    }
}