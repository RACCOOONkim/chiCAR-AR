using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject otherObject; // 활성화할 다른 오브젝트
    void Start()
    {
        otherObject.SetActive(true); // 다른 오브젝트 활성화
    }

}
