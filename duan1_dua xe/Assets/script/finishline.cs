using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int totalLaps = 3; // Total number of laps to complete
    private RaceManager raceManager;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
        if (raceManager == null)
        {
            Debug.LogError("RaceManager not found in the scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            raceManager.CarCrossedFinishLine(other.gameObject);
        }
    }
}
