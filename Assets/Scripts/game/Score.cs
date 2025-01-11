using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject[] targetObjects; // 충돌을 감지할 오브젝트 배열
    public TextMeshProUGUI scoreText; // 스코어를 기록할 텍스트
    public AudioSource effectSource; //
    public GameObject canvas4;
    public GameObject canvas9;
    public MovingMap movingMap; // MovingMap 스크립트 참조
    private int score = 0; // 현재 스코어

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (other.gameObject == targetObjects[i])
            {
                targetObjects[i].SetActive(false); // 충돌된 오브젝트 비활성화
                effectSource.Play();
                score += 1; // 스코어 1점 추가
                UpdateScoreText(); // 스코어 텍스트 업데이트
                if (targetObjects[i].name == "telescope")
                {
                    canvas4.SetActive(true);
                    movingMap.MovingPause();
                }
                
                if (targetObjects[i].name == "fish")
                {
                    canvas9.SetActive(true);
                    movingMap.MovingPause();
                }
                break;
            }
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
