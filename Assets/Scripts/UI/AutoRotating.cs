using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가

public class AutoRotating : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RotateAndDeactivate();
    }

    private void RotateAndDeactivate()
    {
        // 오브젝트를 y축으로 1초 동안 한 바퀴 회전
        transform.DORotate(new Vector3(0, 720, 0), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
        {
            // 1초 뒤에 오브젝트 비활성화
            gameObject.SetActive(false);
        });
    }
}
