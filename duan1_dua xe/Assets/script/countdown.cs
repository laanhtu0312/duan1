using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdown : MonoBehaviour
{

    float currenttime = 0f;
    float startingtime = 10f;
    // Start is called before the first frame update

    [SerializeField] Text countdownText;
    void Start()
    {
        currenttime = startingtime;
    }

    // Update is called once per frame
    void Update()
    {
        currenttime -= 1 * Time.deltaTime;
        print(currenttime);
    }
}
