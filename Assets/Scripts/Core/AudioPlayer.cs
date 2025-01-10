using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Animator animator;
    private const bool deleteCachedFile = true;
    public event Action OnAudioFinished; // 오디오 재생 완료 이벤트

    private void OnEnable()
    {
        if (!audioSource) this.audioSource = GetComponent<AudioSource>();
        if (!animator) animator = GetComponent<Animator>();
    }

    private void OnValidate() => OnEnable();

    public void ProcessAudioBytes(byte[] audioData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "audio.mp3");
        File.WriteAllBytes(filePath, audioData);

        // 애니메이션 시작
        animator?.ResetTrigger("Running");
        animator?.SetTrigger("Idle");

        StartCoroutine(LoadAndPlayAudio(filePath));
    }
    
    private IEnumerator LoadAndPlayAudio(string filePath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = audioClip;
            audioSource.Play();

            // 오디오 재생이 끝날 때까지 대기
            yield return new WaitWhile(() => audioSource.isPlaying);

            // 애니메이션 종료
            animator?.ResetTrigger("Talking");
            animator?.SetTrigger("Done");

            // 오디오 재생 완료 이벤트 호출
            OnAudioFinished?.Invoke();
        }
        else Debug.LogError("Audio file loading error: " + www.error);
        
        if (deleteCachedFile) File.Delete(filePath);
    }
}