using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuloadfromgame : MonoBehaviour
{

    void Start()
    {
        
    }
    public void Openscene()
    {

        SceneManager.LoadScene("Gamemenu");
    
    }
}
