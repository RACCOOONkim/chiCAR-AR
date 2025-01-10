using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoop : MonoBehaviour
{
    // 이동 시작 위치와 끝 위치
    public Transform SeoulStart;
    public Transform SeoulEnd;

    public Transform DaejeonStart;
    public Transform DaejeonEnd;

    public Transform BusanStart;
    public Transform BusanEnd;

    // 이동 속도
    public float speed = 2.0f;

    // 현재 이동 상태
    private bool movingToEnd = true;

    // 현재 맵 상태
    private enum Map { Seoul, Daejeon, Busan }
    private Map currentMap = Map.Seoul;

    // 각 맵에서 이동할 루프 횟수
    private int loopsLeft = 2; // 서울에서는 2번, 대전은 3번, 부산은 3번

    // 차량과 카메라의 이동을 제어하는 함수
    void Update()
    {
        // 차량과 카메라의 현재 위치
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

        // 현재 맵에 맞는 시작 위치와 끝 위치 설정
        Transform startPoint = null;
        Transform endPoint = null;

        switch (currentMap)
        {
            case Map.Seoul:
                startPoint = SeoulStart;
                endPoint = SeoulEnd;
                break;
            case Map.Daejeon:
                startPoint = DaejeonStart;
                endPoint = DaejeonEnd;
                break;
            case Map.Busan:
                startPoint = BusanStart;
                endPoint = BusanEnd;
                break;
        }

        // 목표 위치 설정
        Vector3 targetPosition = movingToEnd ? new Vector3(endPoint.position.x, 0, endPoint.position.z) : new Vector3(startPoint.position.x, 0, startPoint.position.z);

        // 차량 이동
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달하면 다음 이동 준비
        if (Vector3.Distance(currentPosition, targetPosition) < 0.01f)
        {
            // 끝점에 도달하면 위치 리셋
            if (movingToEnd)
            {
                transform.position = new Vector3(startPoint.position.x, transform.position.y, startPoint.position.z); // 위치 리셋
            }

            movingToEnd = !movingToEnd; // 방향 전환

            // 루프 횟수 감소
            if (!movingToEnd)
            {
                loopsLeft--;

                // 각 맵에서 지정된 루프 횟수만큼 이동한 후 다음 맵으로 이동
                if (loopsLeft <= 0)
                {
                    // 다음 맵으로 이동
                    if (currentMap == Map.Seoul)
                    {
                        currentMap = Map.Daejeon;
                        loopsLeft = 3; // 대전은 3번
                    }
                    else if (currentMap == Map.Daejeon)
                    {
                        currentMap = Map.Busan;
                        loopsLeft = 3; // 부산은 3번
                    }
                }
            }
        }
    }
}
