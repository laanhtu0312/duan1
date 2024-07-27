using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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