using System.Collections.Generic;
using UnityEngine;

public class MusicShuffle : MonoBehaviour
{
    [Header("Music Tracks")]
    [Tooltip("List of track names to shuffle.")]
    public List<string> tracks = new List<string>();

    // Returns a new shuffled list where no track repeats consecutively.
    public List<string> Shuffle()
    {
        if (tracks == null || tracks.Count == 0)
            return new List<string>();

        // Create a copy of the track list.
        List<string> shuffled = new List<string>(tracks);
        int n = shuffled.Count;

        // Fisher–Yates shuffle to randomize the order.
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            // Swap elements
            string temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        // Post-process to avoid any consecutive duplicates.
        for (int i = 1; i < n; i++)
        {
            if (shuffled[i] == shuffled[i - 1])
            {
                bool swapped = false;
                // Try swapping with a later track.
                for (int j = i + 1; j < n; j++)
                {
                    if (shuffled[j] != shuffled[i - 1])
                    {
                        string temp = shuffled[i];
                        shuffled[i] = shuffled[j];
                        shuffled[j] = temp;
                        swapped = true;
                        break;
                    }
                }
                // If a swap wasn’t possible, try looking earlier in the list.
                if (!swapped)
                {
                    for (int j = 0; j < i - 1; j++)
                    {
                        if (shuffled[j] != shuffled[i])
                        {
                            string temp = shuffled[j];
                            shuffled[j] = shuffled[i];
                            shuffled[i] = temp;
                            swapped = true;
                            break;
                        }
                    }
                }
                // If still no swap is possible (e.g., list is all the same track), return the best possible order.
            }
        }

        return shuffled;
    }
}
