using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUI : MonoBehaviour
{
    public GameObject otherObject; // 활성화할 다른 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateOtherObjectAfterDelay());
    }

    private IEnumerator ActivateOtherObjectAfterDelay()
    {
        yield return new WaitForSeconds(3f); // 5초 대기

        gameObject.SetActive(false); // 본 오브젝트 비활성화
        if (otherObject != null)
        {
            otherObject.SetActive(true); // 다른 오브젝트 활성화
        }
    }
}
