using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


using UnityEngine.UI;


public class 22 : MonoBehaviour
{
   public class backgroundmusic : MonoBehaviour
{
    private static backgroundmusic Backgroundmusic;
    private void Awake()
    {
        if (Backgroundmusic == null)
        {
            Backgroundmusic = this;
            DontDestroyOnLoad(Backgroundmusic);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}

public class ResourceManager : MonoBehaviour
{
    public int gold = 100;
    public Text goldText;

    void Start()
    {
        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
            return true;
        }
        return false;
    }

    void UpdateGoldText()
    {
        goldText.text = "Gold: " + gold.ToString();
    }
}

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

public class Soundmanager : MonoBehaviour
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
        Save();
        UpdateButtonIcon();
    }
    private void UpdateButtonIcon()
    {
        if (muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}

public class loadshopp : MonoBehaviour
{

    void Start()
    {

    }
    public void Openscene()
    {

        SceneManager.LoadScene("shopmenu");

    }
}

public class CarAIHandle : MonoBehaviour
{
    public checkpoint checkpointManager;
    public float speed = 10f;
    public float rotationSpeed = 100f;
    public float checkpointDistanceThreshold = 1f;
    public float detectionDistance = 5f;
    public LayerMask obstacleLayer;
    public float avoidForce = 10f;
    public float avoidanceRadius = 1f; // Bán kính để tránh va chạm với các xe khác

    private Rigidbody2D rb;
    private int currentCheckpointIndex = 0;
    private bool isReversing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (checkpointManager == null || checkpointManager.checkpoints.Length == 0) return;

        Transform targetCheckpoint = checkpointManager.GetNextCheckpoint(currentCheckpointIndex);
        Vector2 direction = (Vector2)targetCheckpoint.position - rb.position;
        direction.Normalize();

        // Tránh chướng ngại vật và các xe khác
        AvoidObstacles(ref direction);
        AvoidOtherCars(ref direction);

        // Nếu đang di chuyển lùi, đảo ngược hướng di chuyển
        if (isReversing)
        {
            direction = -direction;
        }

        // Di chuyển về phía checkpoint
        rb.velocity = direction * speed;

        // Quay về phía checkpoint
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);

        // Kiểm tra nếu xe đã đến gần checkpoint
        if (Vector2.Distance(rb.position, targetCheckpoint.position) < checkpointDistanceThreshold)
        {
            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpointManager.checkpoints.Length;
        }
    }

    void AvoidObstacles(ref Vector2 direction)
    {
        RaycastHit2D hitFront = Physics2D.Raycast(transform.position, transform.up, detectionDistance, obstacleLayer);
        if (hitFront.collider != null)
        {
            direction += (Vector2)transform.right * avoidForce;
            isReversing = true; // Đổi hướng di chuyển khi gặp chướng ngại vật
        }
        else
        {
            isReversing = false; // Quay lại hướng di chuyển ban đầu khi không còn chướng ngại vật
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, detectionDistance / 2, obstacleLayer);
        if (hitLeft.collider != null)
        {
            direction += (Vector2)transform.right * avoidForce;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, detectionDistance / 2, obstacleLayer);
        if (hitRight.collider != null)
        {
            direction += (Vector2)transform.right * avoidForce * -1;
        }

        direction.Normalize();
    }

    void AvoidOtherCars(ref Vector2 direction)
    {
        Collider2D[] hitCars = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, obstacleLayer);
        foreach (Collider2D hitCar in hitCars)
        {
            if (hitCar.gameObject != gameObject) // Tránh xe khác
            {
                Vector2 avoidDirection = (Vector2)(transform.position - hitCar.transform.position);
                direction += avoidDirection.normalized * avoidForce;
            }
        }
        direction.Normalize();
    }

}
