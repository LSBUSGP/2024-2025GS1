using System.Collections.Generic;
using UnityEngine;

public class MusicShuffleTest : MonoBehaviour
{
    [Header("Test Settings")]
    [Tooltip("Reference to the MusicShuffle component to test.")]
    public MusicShuffle musicShuffle;
    [Tooltip("Number of iterations for the stress test.")]
    public int iterations = 1000;

    void Start()
    {
        // Run demonstration error test.
        RunErrorTest();

        // Run the stress test.
        RunStressTest();
    }

    public void RunStressTest()
    {
        if (musicShuffle == null)
        {
            Debug.LogError("MusicShuffle component not assigned in MusicShuffleTest.");
            return;
        }

        bool allPassed = true;
        for (int i = 0; i < iterations; i++)
        {
            List<string> result = musicShuffle.Shuffle();
            if (!TestNoConsecutiveDuplicates(result))
            {
                Debug.LogError("Stress test failed on iteration " + i);
                allPassed = false;
                break;
            }
        }
        if (allPassed)
            Debug.Log("All " + iterations + " iterations passed the no consecutive duplicates test.");
    }

    // Returns true if no two consecutive elements are the same.
    public bool TestNoConsecutiveDuplicates(List<string> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] == list[i - 1])
                return false;
        }
        return true;
    }

    public void RunErrorTest()
    {
        // Create a faulty sequence with consecutive duplicates.
        List<string> errorList = new List<string> { "Track1", "Track1", "Track2", "Track3" };
        bool testResult = TestNoConsecutiveDuplicates(errorList);
        if (!testResult)
        {
            Debug.Log("Error test caught a consecutive duplicate as expected.");
        }
        else
        {
            Debug.LogError("Error test did not catch the consecutive duplicate error!");
        }
    }
}
