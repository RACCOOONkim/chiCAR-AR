using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnNpcController : MonoBehaviour
{

    // NpcController를 받아서 5초 뒤에 활성화하는 함수
    public void EnableNpcControllerAfterDelay(NpcController npcController)
    {
        if (npcController != null)
        {
            StartCoroutine(EnableAfterDelayCoroutine(npcController));
        }
    }

    private IEnumerator EnableAfterDelayCoroutine(NpcController npcController)
    {
        yield return new WaitForSeconds(5f); // 5초 대기
        npcController.enabled = true; // NpcController 활성화
    }
}
