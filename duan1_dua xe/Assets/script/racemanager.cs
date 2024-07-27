using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
   
        public List<CarController> cars;
        public Transform finishLine;
        public List<CarRaceData> carRaceDataList;
        private int raceFinishedCount = 0;

        void Start()
        {
            carRaceDataList = new List<CarRaceData>();

            foreach (CarController car in cars)
            {
                carRaceDataList.Add(new CarRaceData(car));
            }
        }

        public void CarCrossedFinishLine(GameObject carObject)
        {
            CarRaceData carData = carRaceDataList.Find(c => c.car.gameObject == carObject);

            if (carData != null && carData.currentLap < carData.totalLaps)
            {
                carData.currentLap++;
                carData.lapTimes.Add(Time.time - carData.currentLapStartTime);
                carData.currentLapStartTime = Time.time;

                if (carData.currentLap == carData.totalLaps)
                {
                    raceFinishedCount++;
                    carData.finalPosition = raceFinishedCount;
                    carData.totalTime = Time.time - carData.raceStartTime;
                    Debug.Log(carData.car.name + " finished in position " + carData.finalPosition);
                }
            }
        }
    }

    [System.Serializable]
    public class CarRaceData
    {
        public CarController car;
        public int currentLap = 0;
        public List<float> lapTimes;
        public int finalPosition = -1;
        public float raceStartTime;
        public float currentLapStartTime;
        public float totalTime;
        public int totalLaps;

        public CarRaceData(CarController carController)
        {
            car = carController;
            lapTimes = new List<float>();
            raceStartTime = Time.time;
            currentLapStartTime = Time.time;
            totalLaps = carController.totalLaps;
        }
    }
