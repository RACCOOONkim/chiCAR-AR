using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Toggle을 사용하기 위해 추가

public class StartUI : MonoBehaviour
{
    public GameObject targetObject; // 비활성화할 오브젝트
    public Toggle toggle; // Toggle UI 요소
    public NpcController npcController; // NpcController 컴포넌트

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
            npcController.enabled = isOn; // NpcController 컴포넌트 활성화 여부 설정
        }
        gameObject.SetActive(!isOn);
    }
}
