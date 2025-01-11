using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFish : MonoBehaviour
{
    public MovingMap movingMap;
    void Start()
    {
        movingMap.Resume();
    }

    void Update()
    {
        
    }
}
