using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class EtriSTT : MonoBehaviour
{
    [Header("REST API")]
    public string key = "21fac2d4-a6de-4255-b8d0-9cf70733faf9"; // ETRI API 키
    public string uri = "http://aiopen.etri.re.kr:8000/WiseASR/Recognition";

    public TMP_Text outputText; // 텍스트 결과를 표시할 UI

    private AudioClip _recordedClip;
    private bool _isRecording = false;
    [SerializeField] private PerplexityAPI _perplexityAPI;





    public void StartRecording()
    {
        if (_isRecording)
            return;

        _isRecording = true;
        _recordedClip = Microphone.Start(null, false, 5, 16000); // 16kHz로 5초간 녹음
        outputText.text = "치카에게 질문해주세요!";

        StartCoroutine(StopRecordingAfterDelay(5f)); // 5초 후 자동으로 녹음 중지
    }

    private IEnumerator StopRecordingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!_isRecording)
            yield break;

        _isRecording = false;
        Microphone.End(null);

        if (_recordedClip == null)
        {
            outputText.text = "No audio recorded.";
            yield break;
        }

        byte[] audioData = ConvertAudioClipToWav(_recordedClip);
        string audioBase64 = System.Convert.ToBase64String(audioData);

        StartCoroutine(SendAudioForSTT(audioBase64));
    }

    private byte[] ConvertAudioClipToWav(AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        MemoryStream stream = new MemoryStream();

        // WAV 헤더 작성
        WriteWavHeader(stream, clip);

        // 샘플 데이터를 PCM 형식으로 변환하여 스트림에 기록
        foreach (float sample in samples)
        {
            short intSample = (short)(sample * short.MaxValue);
            stream.WriteByte((byte)(intSample & 0xFF));
            stream.WriteByte((byte)((intSample >> 8) & 0xFF));
        }

        return stream.ToArray();
    }

    private void WriteWavHeader(Stream stream, AudioClip clip)
    {
        int frequency = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
        stream.Write(System.BitConverter.GetBytes(36 + samples * channels * 2), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
        stream.Write(System.BitConverter.GetBytes(16), 0, 4); // Subchunk1Size (16 for PCM)
        stream.Write(System.BitConverter.GetBytes((short)1), 0, 2); // AudioFormat (1 for PCM)
        stream.Write(System.BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(System.BitConverter.GetBytes(frequency), 0, 4);
        stream.Write(System.BitConverter.GetBytes(frequency * channels * 2), 0, 4); // ByteRate
        stream.Write(System.BitConverter.GetBytes((short)(channels * 2)), 0, 2); // BlockAlign
        stream.Write(System.BitConverter.GetBytes((short)16), 0, 2); // BitsPerSample

        stream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        stream.Write(System.BitConverter.GetBytes(samples * channels * 2), 0, 4); // Subchunk2Size
    }

    private IEnumerator SendAudioForSTT(string audioBase64)
    {
       var requestJson = new
       {
           request_id = "reserved field",
           argument = new
           {
               language_code = "korean", // 언어 코드 설정
               audio = audioBase64
           }
       };

       string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestJson);

       using (UnityWebRequest www = new UnityWebRequest(uri, "POST"))
       {
           www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
           www.downloadHandler = new DownloadHandlerBuffer();
           www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
           www.SetRequestHeader("Authorization", key); // Authorization 헤더에 API 키 추가

           yield return www.SendWebRequest();

           if (www.result == UnityWebRequest.Result.Success)
           {
               var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseBody>(www.downloadHandler.text);
               outputText.text = "질문: " + responseBody.return_object.recognized;

               PerplexityAPI.responseText = responseBody.return_object.recognized;
               Debug.Log("질문: " + responseBody.return_object.recognized);
               
               StartCoroutine(_perplexityAPI.RequestResponse()); // 자동으로 PerplexityAPI 요청!
           }
           else
           {
               outputText.text = "Transcription failed.";
           }
       }
   }
}

[System.Serializable]
public class ResponseBody
{
   public string request_id;
   public int result;
   public ReturnObject return_object;

   [System.Serializable]
   public class ReturnObject
   {
       public string recognized;
   }
}