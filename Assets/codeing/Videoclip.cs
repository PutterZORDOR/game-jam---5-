using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Drag and drop the VideoPlayer component here
    public RawImage rawImage;       // Drag and drop the RawImage component here
    public VideoClip[] videoClips;  // Array of video clips to cycle through
    public string[] sceneNames;     // Array of scene names to switch to after the last clip
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
            // If it's the last clip, stop the video and randomize the scene
            videoPlayer.Stop();

            // Sufficient clicking is required to randomize and load a new scene
            int randomSceneIndex = Random.Range(0, sceneNames.Length);
            SceneManager.LoadScene(sceneNames[randomSceneIndex]);
        }
    }

    // Check when the video finishes
    void CheckVideoEnd(VideoPlayer vp) {
        // Do nothing when the video ends; only change the scene on click
    }
}

