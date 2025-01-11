using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFish : MonoBehaviour
{
    public GameObject otherObject; // 활성화할 다른 오브젝트
    // public GameObject deactivateObject; // 활성화할 다른 오브젝트
    // Start is called before the first frame update
    void Start()
    {
        if (otherObject != null)
        {
            otherObject.SetActive(true); // 다른 오브젝트 활성화
            // deactivateObject.SetActive(false);
        }
        StartCoroutine(ActivateOtherObjectAfterDelay());
    }

    private IEnumerator ActivateOtherObjectAfterDelay()
    {
        yield return new WaitForSeconds(5f); // 5초 대기
        gameObject.SetActive(false); // 본 오브젝트 비활성화
    }
}
