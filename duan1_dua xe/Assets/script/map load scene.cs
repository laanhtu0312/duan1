using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscene : MonoBehaviour
{

    void Start()
    {
        
    }
    public void Openscene()
    {

        SceneManager.LoadScene("SampleScene");
    
    }
}
