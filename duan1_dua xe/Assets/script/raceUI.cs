using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceUIManager : MonoBehaviour
{
    public Text lapText;
    public Text timeText;
    public Text positionText;
    public RaceManager raceManager;
    public CarController playerCar;

    void Update()
    {
        CarRaceData playerData = raceManager.carRaceDataList.Find(c => c.car == playerCar);

        if (playerData != null)
        {
            lapText.text = "Lap: " + playerData.currentLap + "/" + playerData.totalLaps;
            timeText.text = "Time: " + (Time.time - playerData.raceStartTime).ToString("F2") + "s";

            if (playerData.finalPosition > 0)
            {
                positionText.text = "Position: " + playerData.finalPosition;
            }
        }
    }
}
