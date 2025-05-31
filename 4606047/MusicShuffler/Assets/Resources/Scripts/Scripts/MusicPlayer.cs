using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the MusicShuffle component that shuffles track names.")]
    public MusicShuffle musicShuffle;
    [Tooltip("AudioSource used to play the music tracks.")]
    public AudioSource audioSource;

    [Header("Settings")]
    [Tooltip("Folder within Resources where the audio clips are stored (e.g., 'Music').")]
    public string musicFolder = "Music";

    private List<string> currentShuffle;
    private int currentTrackIndex = 0;

    void Start()
    {
        if (musicShuffle == null || audioSource == null)
        {
            Debug.LogError("MusicShuffle and AudioSource references must be assigned in MusicPlayer.");
            return;
        }
        // Get the initial shuffle order.
        currentShuffle = musicShuffle.Shuffle();
        currentTrackIndex = 0;
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        while (true)
        {
            if (currentShuffle.Count == 0)
            {
                Debug.LogWarning("No tracks to play.");
                yield break;
            }

            string trackName = currentShuffle[currentTrackIndex];
            // Attempt to load the AudioClip from Resources/Music.
            AudioClip clip = Resources.Load<AudioClip>(musicFolder + "/" + trackName);
            if (clip == null)
            {
                Debug.LogError("Could not load AudioClip for track: " + trackName);
            }
            else
            {
                audioSource.clip = clip;
                audioSource.Play();
                // Wait for the clip's duration.
                yield return new WaitForSeconds(clip.length);
            }

            // Move to the next track in the shuffle order.
            currentTrackIndex++;
            if (currentTrackIndex >= currentShuffle.Count)
            {
                // Re-shuffle and restart.
                currentShuffle = musicShuffle.Shuffle();
                currentTrackIndex = 0;
            }
        }
    }
}
