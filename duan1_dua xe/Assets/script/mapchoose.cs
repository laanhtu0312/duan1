using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapscene : MonoBehaviour
{

    void Start()
    {
        
    }
    public void Openscene()
    {

        SceneManager.LoadScene("mapmenu");
    
    }
}
