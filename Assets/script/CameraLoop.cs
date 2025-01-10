using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLoop : MonoBehaviour
{
    // 이동 시작 위치와 끝 위치
    public Transform startPoint;   // 시작 위치
    public Transform endPoint;     // 끝 위치

    // 이동 속도
    public float speed = 15.0f;

    // 현재 방향
    private bool movingToEnd = true;

    void Update()
    {
        // 차량의 현재 위치
        Vector3 currentPosition = transform.position;

        // 목표 위치 설정
        Vector3 targetPosition = movingToEnd ? endPoint.position : startPoint.position;

        // 차량 이동
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달하면 방향 전환
        if (Vector3.Distance(currentPosition, targetPosition) < 0.01f)
        {
            movingToEnd = !movingToEnd; // 방향 전환
        }
    }
}
