using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // 상하 이동 범위
    public float moveDistance = 1.0f; // 최대 이동 범위
    // 이동 속도
    public float speed = 2.0f;

    // 시작 위치 (Y 좌표 기준)
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        // 시작 위치 저장
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // sin 함수로 상하 이동
        float newY = startY + Mathf.Sin(Time.time * speed) * moveDistance;

        // 새로운 위치 적용
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
