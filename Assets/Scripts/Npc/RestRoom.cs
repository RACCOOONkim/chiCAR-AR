using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가
using UnityEngine.UI;

public class RestRoom : MonoBehaviour
{
    public GameObject targetObject; // 비활성화할 오브젝트
    public float movingTime = 3f;

    public Toggle toggle; 
    public NpcController npcController; 
    public GameObject startPoint;
    public GameObject npcCharacter;

    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isOn); // Toggle이 켜지면 오브젝트를 비활성화
        }
        // Toggle이 켜질 때 MoveTostartPoint 함수 호출
        if (isOn)
        {
            MoveTostartPoint();
        }
        else
        {
            MoveBackFromstartPoint();
        }

        Invoke(nameof(SetNpcControllerEnabled), 5f);
    }

    void SetNpcControllerEnabled()
    {
        npcController.enabled = toggle.isOn;
    }

    public void MoveTostartPoint()
    {
        if (startPoint != null)
        {
            npcCharacter.transform.DOMove(startPoint.transform.position, movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DORotateQuaternion(startPoint.transform.rotation, movingTime).SetEase(Ease.Linear);
            npcController.enabled = false;
        }
    }

    public void MoveBackFromstartPoint()
    {
        if (startPoint != null)
        {
            npcCharacter.transform.DOMove(transform.position, movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DORotateQuaternion(transform.rotation, movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DOScale(transform.localScale, movingTime).SetEase(Ease.Linear);
        }
    }
}
