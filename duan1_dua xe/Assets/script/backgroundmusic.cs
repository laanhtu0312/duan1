using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
