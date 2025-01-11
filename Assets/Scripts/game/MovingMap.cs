using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가
using DG.Tweening.Core; // TweenExtensions를 사용하기 위해 추가
using DG.Tweening.Plugins.Options; // TweenExtensions를 사용하기 위해 추가

public class MovingMap : MonoBehaviour
{
    private Tween moveTween;
    public float moveSpeed = 60f;
    public GameObject final;
    public AudioSource bgmsource;

    // Start is called before the first frame update
    void Start()
    {
    }

    // 오브젝트 이동 시작
    public void StartMoving()
    {
        moveTween = transform.DOMoveZ(-370, moveSpeed).SetEase(Ease.Linear).SetAutoKill(false).OnComplete(OnMoveComplete);
    }

    // 이동 완료 시 호출되는 메서드
    private void OnMoveComplete()
    {
        if (final != null)
        {
            final.SetActive(true); // final 오브젝트 활성화
            bgmsource.Pause();
        }
    }

    // 이동 일시정지
    public void MovingPause()
    {
        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Pause();
        }
    }

    // 이동 재개
    public void Resume()
    {
        if (moveTween != null && !moveTween.IsPlaying())
        {
            moveTween.Play();
        }
    }
}
