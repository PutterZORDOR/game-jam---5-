using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Videoclip : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Drag and drop the VideoPlayer component here
    public RawImage rawImage;       // Drag and drop the RawImage component here
    public VideoClip[] videoClips;  // Array of video clips to cycle through
    public string nextSceneName;    // Name of the scene to switch to after the last clip
    private int currentVideoIndex = 0;

    void Start() {
        // Set the first video
        videoPlayer.clip = videoClips[currentVideoIndex];
        videoPlayer.Play();

        // Add an event listener for when the video ends
        videoPlayer.loopPointReached += CheckVideoEnd;
    }

    void Update() {
        // Set the texture of RawImage to the video texture
        if (videoPlayer.texture != null) {
            rawImage.texture = videoPlayer.texture;
        }
    }

    public void OnVideoClick() {
        // Move to the next video
        currentVideoIndex++;

        // Check if we've reached the last video clip
        if (currentVideoIndex < videoClips.Length) {
            videoPlayer.clip = videoClips[currentVideoIndex];
            videoPlayer.Play();
        } else {
            // If it's the last clip, stop or do nothing (scene will change after this clip)
            videoPlayer.Stop();
        }
    }

    // Check when the video finishes
    void CheckVideoEnd(VideoPlayer vp) {
        // If this is the last clip, change to the next scene
        if (currentVideoIndex >= videoClips.Length - 1) {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
