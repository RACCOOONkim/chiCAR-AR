using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    private enum ProcessStage { Stage1, Stage2, Stage3 }
    private ProcessStage currentStage = ProcessStage.Stage1;
    public MovingMap movingMap;
    public GameObject daejeonStart;
    public GameObject busanStart;
    public GameObject canvas2;
    public GameObject canvas6;

    // Start is called before the first frame update
    void Start()
    {
        StartStage1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 첫 번째 단계 시작
    public void StartStage1()
    {
        currentStage = ProcessStage.Stage1;
        Debug.Log("Stage 1 started.");
        // Stage 1 시작 로직 추가
    }

    // 두 번째 단계 시작
    private void StartStage2()
    {
        currentStage = ProcessStage.Stage2;
        Debug.Log("Daejoen.");
        movingMap.MovingPause();
        canvas2.SetActive(true);
        // Stage 2 시작 로직 추가
    }

    // 세 번째 단계 시작
    private void StartStage3()
    {
        currentStage = ProcessStage.Stage3;
        Debug.Log("Stage 3 started.");
        canvas6.SetActive(true);
        movingMap.MovingPause();
        // Stage 3 시작 로직 추가
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentStage == ProcessStage.Stage1 && other.gameObject == daejeonStart)
        {
            StartStage2();
        }
        else if (currentStage == ProcessStage.Stage2 && other.gameObject == busanStart)
        {
            StartStage3();
        }
    }
}
