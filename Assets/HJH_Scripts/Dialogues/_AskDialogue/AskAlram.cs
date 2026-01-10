using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskAlram : MonoBehaviour
{
    public static AskAlram Instance { get; private set; }
    public AskPoint[] allPoints;
    private void Awake()
    {
        Instance = this;    
    }
    void Start()
    {   
      allPoints = FindObjectsOfType<AskPoint>();      
    }

}
