using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{
    public float speed = 10f; // Vận tốc cơ bản của xe
    public float acceleration = 2f; // Gia tốc của xe
    public float rotationSpeed = 100f; // Độ quay của xe
    public int totalLaps = 3;

    private Rigidbody2D rb;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Khóa sự quay của Rigidbody2D để ngăn chặn tự động xoay khi va chạm
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Lấy input từ các phím mũi tên
        float moveInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        // Tính vận tốc dựa trên gia tốc
        float currentSpeed = moveInput * speed;

        // Di chuyển xe
        Vector2 movement = transform.up * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Quay xe
        if (moveInput != 0) // Chỉ quay xe khi xe đang di chuyển
        {
            float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
    }
}