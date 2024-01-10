using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RandomNumberAPIHandler : MonoBehaviour
{
    public int lastFetchedNumber; // Variable to store the last fetched number

    // Coroutine to fetch a random number from the specified API
    public IEnumerator GetRandomNumber()
    {
        // API URL to request a random number between 1 to 6
        string apiUrl = "http://www.randomnumberapi.com/api/v1.0/random?min=1&max=6&count=1";

        // Create a UnityWebRequest to communicate with the API
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Send the web request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check if the request was successful
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                // Log an error if the request fails
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Extract the response from the API
                string response = webRequest.downloadHandler.text;
                Debug.Log("API Response: " + response);

                // Remove unnecessary characters from the response
                response = response.Replace("[", "").Replace("]", "");

                int randomNumber;

                // Try parsing the response to an integer
                if (int.TryParse(response, out randomNumber))
                {
                    // Log the fetched random number
                    Debug.Log("Fetched Random Number: " + randomNumber);

                    // Store the fetched number in the variable
                    lastFetchedNumber = randomNumber;
                }
                else
                {
                    // Log an error if parsing fails
                    Debug.LogError("Failed to parse random number! Response: " + response);
                }
            }
        }
    }
}
