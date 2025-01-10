using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userView : MonoBehaviour
{
    Transform playerTransform;
    Vector3 Offset;
    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("FordCar").transform;
        Offset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position; 
    }
}
