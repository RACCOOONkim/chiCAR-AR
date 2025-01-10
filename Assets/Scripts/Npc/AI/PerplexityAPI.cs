using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PerplexityAPI : MonoBehaviour
{
    [SerializeField] private TTSManager ttsManager;
    private string apiKey = "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";
    private string url = "https://api.perplexity.ai/chat/completions";

    public static string responseText;

    //public Button requestButton;

    void Start()
    {
        //requestButton.onClick.AddListener(() => StartCoroutine(RequestResponse()));
    }

    public IEnumerator RequestResponse()
    {
        string userMessage = responseText;
        string roleInstruction = "자연스럽고 대화체로 설명해 주세요. 답변은 한국어로 작성해 주세요.";

        var jsonData = new
        {
            model = "llama-3.1-sonar-small-128k-online",
            messages = new[]
            {
                new { role = "user", content = roleInstruction + userMessage }
            }
        };

        string jsonString = JsonConvert.SerializeObject(jsonData);

        Debug.Log("Request JSON: " + jsonString);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);
                SpeakConsultText(); // Process and speak the text

                // Try parsing the response if it's JSON
                try
                {
                    var parsedResponse = JsonConvert.DeserializeObject(responseText);
                    Debug.Log("Parsed Response: " + parsedResponse);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"Error: {request.error}, Response Code: {request.responseCode}");
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }

    void SpeakConsultText()
    {
        if (ttsManager != null && !string.IsNullOrEmpty(responseText))
        {
            string contentText = PreprocessingSpeak(responseText);
            if (!string.IsNullOrEmpty(contentText))
            {
                ttsManager.SynthesizeText(contentText); // Pass only the content to TTS
            }
        }
    }

    private string PreprocessingSpeak(string jsonResponse)
    {
        try
        {
            // Parse the JSON response
            JObject parsedResponse = JObject.Parse(jsonResponse);

            // Navigate to the content field within the choices array
            string content = parsedResponse["choices"]?[0]?["message"]?["content"]?.ToString();

            if (!string.IsNullOrEmpty(content))
            {
                Debug.Log("Extracted Content: " + content);
                return content;
            }
            else
            {
                Debug.LogError("Content not found in response.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing JSON response: " + ex.Message);
            return null;
        }
    }
}