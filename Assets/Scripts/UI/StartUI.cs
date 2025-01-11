using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가
using UnityEngine.UI; // Toggle을 사용하기 위해 추가

public class StartUI : MonoBehaviour
{
    public GameObject targetObject; // 비활성화할 오브젝트
    public float movingTime = 5f; 

    public Toggle toggle; // Toggle UI 요소
    public NpcController npcController; // NpcController 컴포넌트
    public GameObject pivot1;
    public GameObject npcCharacter;
    public MovingMap map; // Map

    // Start is called before the first frame update
    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    // Toggle 값이 변경될 때 호출되는 메서드
    void OnToggleValueChanged(bool isOn)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!isOn); // Toggle이 켜지면 오브젝트를 비활성화
        }
        // Toggle이 켜질 때 MoveToPivot1WithScale 함수 호출
        if (isOn)
        {
            MoveToPivot1WithScale();
        }
        gameObject.SetActive(!isOn);

        Invoke(nameof(SetNpcControllerEnabled), 5f);
    }

    void SetNpcControllerEnabled()
    {
        npcController.enabled = toggle.isOn;
    }

    public void MoveToPivot1WithScale()
    {
        if (pivot1 != null)
        {
            npcCharacter.transform.DOMove(pivot1.transform.position, movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DORotateQuaternion(pivot1.transform.rotation, movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DOScale(pivot1.transform.localScale, movingTime).SetEase(Ease.Linear);
        }
    }

    public void MoveBackFromPivot1WithScale()
    {
        npcController.enabled = false;
        if (pivot1 != null)
        {
            npcCharacter.transform.DOMove(new Vector3(-0.354f, 1.039f, 0.475f), movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DORotate(new Vector3(npcCharacter.transform.rotation.eulerAngles.x, npcCharacter.transform.rotation.eulerAngles.y + 180, npcCharacter.transform.rotation.eulerAngles.z), movingTime).SetEase(Ease.Linear);
            npcCharacter.transform.DOScale(0.1f, movingTime).SetEase(Ease.Linear);
        }
    }
}
