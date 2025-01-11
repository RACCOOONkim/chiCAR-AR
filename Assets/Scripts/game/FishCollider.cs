using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCollider : MonoBehaviour
{
    public GameObject objectToActivate; // 활성화할 오브젝트
    public GameObject objectToDeactivate; // 비활성화할 오브젝트
    public MovingMap movingMap; // MovingMap 스크립트 참조



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "findfish")
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true); // 오브젝트 활성화
            }

            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false); // 오브젝트 비활성화
            }

            if (movingMap != null)
            {
                movingMap.MovingPause(); // MovingMap 일시정지
            }
        }
    }
}
