using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가

public class NpcController : MonoBehaviour
{
    public Transform handTransform;
    public float delay = 0.5f; // 지연 시간 설정
    public float sensitivity = 0.5f; // 민감도
    private Vector3 lastHandPosition;
    private Quaternion lastHandRotation;

    void Start()
    {
        if (handTransform != null)
        {
            lastHandPosition = handTransform.position;
            //lastHandRotation = handTransform.rotation;
        }
    }

    void Update()
    {
        if (handTransform != null)
        {
            Vector3 handDelta = handTransform.position - lastHandPosition;
            //Quaternion handRotationDelta = handTransform.rotation * Quaternion.Inverse(lastHandRotation);

            lastHandPosition = handTransform.position;
            //lastHandRotation = handTransform.rotation;

            Vector3 targetPosition = transform.position + handDelta * sensitivity;
            //Quaternion targetRotation = transform.rotation * Quaternion.Slerp(Quaternion.identity, handRotationDelta, sensitivity);

            transform.DOMove(targetPosition, delay).SetEase(Ease.Linear);
            //transform.DORotateQuaternion(targetRotation, delay).SetEase(Ease.Linear);
        }
    }
}