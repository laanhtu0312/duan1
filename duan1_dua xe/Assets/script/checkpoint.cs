using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public Transform[] checkpoints; // Array of checkpoint positions

    public Transform GetNextCheckpoint(int currentCheckpointIndex)
    {
        return checkpoints[(currentCheckpointIndex + 1) % checkpoints.Length];
    }

}
