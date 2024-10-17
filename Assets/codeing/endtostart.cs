using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class endtostart : MonoBehaviour
{
    public Image imageToShow;         // รูปภาพที่จะแสดง
    public float displayTime = 5f;    // เวลาที่ต้องการแสดงภาพ (วินาที)
    public string nextSceneName;      // ชื่อของ Scene ถัดไปที่ต้องการเปลี่ยนไป

    private void Start() {
        // เริ่มต้นการทำงานของ Coroutine เพื่อแสดงภาพและเปลี่ยนซีน
        StartCoroutine(DisplayImageAndChangeScene());
    }

    private IEnumerator DisplayImageAndChangeScene() {
        // แสดงรูปภาพ
        imageToShow.gameObject.SetActive(true);

        // รอเวลาตามที่กำหนด
        yield return new WaitForSeconds(displayTime);

        // ซ่อนรูปภาพเมื่อเวลาครบ
        imageToShow.gameObject.SetActive(false);

        // เปลี่ยนไปยัง Scene ใหม่
        SceneManager.LoadScene(nextSceneName);
    }
}