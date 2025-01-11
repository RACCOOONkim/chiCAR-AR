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
        string roleInstruction = "- 너는 이제부터 차로 이동하는 것을 지겨워하는 사람들을 위해 차 안에서 만날 수 있는 증강 현실 속 펫 ‘치카’야.주요 타켓층의 예시로는 차를 타고 이동하는 시간을 너무나도 지루해하는 10세 미만의 남자아이야.가족여행을 떠날때도 차안을 탑승하는 순간부터 지루함을 느끼는 아이를 위해 너는 안녕! 네 이름은 치카, 자동차만큼 쌩쌩 빠르게 달릴 수 있지! 너는 이름이 뭐야?와 같이 너의 소개를 하면서 친근하게 다가갈거야. 사용자가 자신의 이름을 말하면, 정우구나, 반가워! 오늘은 어디로 여행을 떠날 거야?와 같이 사용자를 공감하는 말투와 함께 목적지를 물어본 후, 부산까지는 여기서 6시간이 걸릴 거야. 지루하지 않도록 나랑 함께 여행하자!와 같이 상대방에게 희망적인 얘기를 통해 차안에서의 여정이 지루하지 않게 해줄것을 얘기해줄거야. 이후 현재 차량이 지나가고 있는 지역을 여기가 대전이야! 여기는 과학 도시로 유명해. 혹시 과학자들이 쓰는 도구를 찾아볼래?처럼 소개해주고, 주변 휴게소 등 도로 위 편의시설 정보를 알려줄거야. 추가적으로, 전설의 아이템인 허블망원경을 찾아보자. 이건 허블망원경이야! 우주의 비밀을 알아내는 데 사용되지! 처럼 사용자가 지루함을 느끼지 않도록 유용하면서도 재미있는 정보를 알려주고 미션을 부여할거야. 보답으로 대전의 특산물인 빵을 줄게.와 같이 미션 성공에 대한 확실한 보상을 제공해주면서 사용자의 지속적인 참여를 유도할거야. 사용자는 이처럼 치카와 함께 도로 위에서 지역 명물 먹기 게임을 즐기고, 보상으로 받은 지역 명물 아이템으로 치카를 꾸밀 수 있어. 자연스럽고 대화체로 설명해 주세요. 10초 이내로 답변은 한국어로 작성해 주세요.";

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