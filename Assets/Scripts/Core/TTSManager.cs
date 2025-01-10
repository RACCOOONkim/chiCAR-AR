using System;
using UnityEngine;

public class TTSManager : MonoBehaviour
{
    private OpenAIWrapper openAIWrapper;
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private TTSModel model = TTSModel.TTS_1;
    [SerializeField] private TTSVoice voice = TTSVoice.Onyx;
    [SerializeField, Range(0.25f, 4.0f)] private float speed = 1f;

    private byte[] audioData; 

    private void OnEnable()
    {
        if (!openAIWrapper) this.openAIWrapper = FindObjectOfType<OpenAIWrapper>();
        if (!audioPlayer) this.audioPlayer = GetComponentInChildren<AudioPlayer>();

        if (audioPlayer != null)
        {
            audioPlayer.OnAudioFinished += HandleAudioFinished;
        }
    }

    private void OnDisable()
    {
        if (audioPlayer != null)
        {
            audioPlayer.OnAudioFinished -= HandleAudioFinished;
        }
    }

    private void OnValidate() => OnEnable();

    public async void SynthesizeText(string text)
    {
        Debug.Log("Trying to synthesize " + text);
        audioData = await openAIWrapper.RequestTextToSpeech(text, model, voice, speed);
        if (audioData != null)
        {
            Debug.Log("Audio data received.");
            PlayAudio(); // 오디오 받으면 자동으로 재생
        }
        else
        {
            Debug.LogError("Failed to get audio data from OpenAI.");
        }
    }

    public void PlayAudio()
    {
        if (audioData != null)
        {
            Debug.Log("Playing audio.");
            audioPlayer.ProcessAudioBytes(audioData); // AudioPlayer로 음성 데이터 재생
        }
        else
        {
            Debug.LogError("No audio data available to play.");
        }
    }

    private void HandleAudioFinished()
    {
        Debug.Log("Audio playback finished.");
    }

    public void SynthesizeAndPlay(string text, TTSModel model, TTSVoice voice, float speed)
    {
        this.model = model;
        this.voice = voice;
        this.speed = speed;
        
        SynthesizeText(text); // 텍스트를 음성으로 변환 (재생은 하지 않음)
        
        // 나중에 PlayAudio()를 호출하여 재생할 수 있음
    }
}