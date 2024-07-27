using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lapManager : MonoBehaviour
{
    public int totalLaps = 3;
    public Text timerText;
    public GameObject endGamePanel;
    public Text resultsText;
    public Text timeText;
    public Text rewardText;

    private Dictionary<string, int> lapsCompleted = new Dictionary<string, int>();
    private List<string> finishOrder = new List<string>();
    private float raceTime;
    private bool raceFinished = false;

    void Start()
    {
        endGamePanel.SetActive(false); // Ban đầu ẩn Panel
    }

    void Update()
    {
        if (!raceFinished)
        {
            raceTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(raceTime / 60F);
        int seconds = Mathf.FloorToInt(raceTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.text = "Time: " + niceTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string carName = other.gameObject.name;

        if (!lapsCompleted.ContainsKey(carName))
        {
            lapsCompleted[carName] = 0;
        }

        lapsCompleted[carName]++;

        if (lapsCompleted[carName] >= totalLaps && !finishOrder.Contains(carName))
        {
            finishOrder.Add(carName);
            other.gameObject.GetComponent<CarController>().enabled = false; // Dừng xe

            if (other.CompareTag("Player"))
            {
                raceFinished = true;
                EndRace();
            }
        }
    }

    void EndRace()
    {
        // Hiển thị bảng kết thúc game
        string results = "Race Results:\n";
        for (int i = 0; i < finishOrder.Count; i++)
        {
            results += (i + 1).ToString() + ". " + finishOrder[i] + "\n";
        }

        int playerPosition = finishOrder.IndexOf("Player") + 1;
        if (playerPosition == 0) playerPosition = finishOrder.Count + 1; // Player chưa hoàn thành vòng đua

        int reward = CalculateReward(playerPosition, raceTime);

        // Đảm bảo text được cập nhật trước khi hiển thị panel
        resultsText.text = "Your Position: " + playerPosition + "\n" + "Results:\n" + results;
        timeText.text = "Your Time: " + timerText.text;
        rewardText.text = "Reward: $" + reward.ToString();

        Debug.Log("Race finished!");
        Debug.Log("Results: " + results);
        Debug.Log("Time: " + timerText.text);
        Debug.Log("Reward: $" + reward.ToString());

        SavePlayerMoney(reward);
        endGamePanel.SetActive(true); // Hiển thị Panel khi cuộc đua kết thúc
    }

    int CalculateReward(int playerPosition, float raceTime)
    {
        int reward = 0;

        if (playerPosition == 1)
        {
            reward = raceTime < 120 ? Random.Range(150, 201) : Random.Range(70, 101);
        }
        else if (playerPosition == 2)
        {
            reward = 100;
        }
        else if (playerPosition == 3)
        {
            reward = 80;
        }
        else if (playerPosition == 4)
        {
            reward = 70;
        }
        else if (playerPosition == 5)
        {
            reward = 60;
        }
        else if (playerPosition == 6)
        {
            reward = 50;
        }

        return reward;
    }

    void SavePlayerMoney(int reward)
    {
        int currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        currentMoney += reward;
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        PlayerPrefs.Save();
    }
}