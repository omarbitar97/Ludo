using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour
{
    // Variables for die animation and game logic
    public Image dieImage; // Image component displaying the die
    public List<Sprite> dieSprites = new List<Sprite>();
    public string boardTextureAddress = "Ludo Board"; // The address you set in Addressables for the board
    public string chipTextureAddress = "Chip"; // The address you set in Addressables for the board
    public Image boardImage; // Attach the board image in the inspector
    public Image chipImage; // Attach the chip image in the inspector
    public LudoGameManager ludoGameManager; // Reference to the LudoGameManager script
    public RandomNumberAPIHandler apiHandler; // Reference to the RandomNumberAPIHandler script
    public GameObject dieIndicator;  // Reference to the die Indicator Game Object
    public GameObject chipIndicator; // Reference to the chip Indicator Game Object
    private bool chipMoved = false; 
    private void Start()
    {
        // Load board,die faces and chip textures when the game starts
        LoadBoardTexture();
        LoadChipTexture();
        LoadDieFaceTextures();
    }
    // Initiates the die roll animation
    public void ToggleRollDie()
    {
        StartCoroutine(RollDie());
    }

    // Coroutine to animate the die roll
    private IEnumerator RollDie()
    {
        const float rollDuration = 1f; // Duration of the rolling animation
        const float delayBetweenSprites = 0.1f; // Delay between sprite changes

        dieIndicator.SetActive(false); // Deactivate Die Indicator GameObejct 
        if (chipMoved == false)
        chipIndicator.SetActive(true); // Activate Chip Indicator GameObejct

        ludoGameManager.hasRolled = true; // Signal that a roll has started
        float timer = 0f;
        int spriteIndex = 0;

        // Loop to animate the changing die faces
        while (timer < rollDuration)
        {
            dieImage.sprite = dieSprites[spriteIndex]; // Display current die face

            yield return new WaitForSeconds(delayBetweenSprites); // Wait for a short duration

            timer += delayBetweenSprites;
            spriteIndex = (spriteIndex + 1) % dieSprites.Count; // Cycle through die faces
        }

        // After the animation, fetch the random number from the API
        yield return StartCoroutine(apiHandler.GetRandomNumber());
        UpdateDieImage(apiHandler.lastFetchedNumber); // Update die image based on fetched number
    }

    // Updates the die image based on the fetched random number
    private void UpdateDieImage(int randomNumber)
    {
        dieImage.sprite = dieSprites[randomNumber - 1]; // Display the corresponding die face
    }

    // Moves the chip on the board based on the fetched random number
    public void MoveChipOnBoard(int steps)
    {
        chipMoved = true;
        chipIndicator.SetActive(false);
        ludoGameManager.MoveChip(apiHandler.lastFetchedNumber); // Move chip based on fetched number
    }

    private void LoadBoardTexture()
    {
        Addressables.LoadAssetAsync<Sprite>(boardTextureAddress).Completed += OnBoardTextureLoaded;
    }

    private void OnBoardTextureLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite boardSprite = handle.Result;
            boardImage.sprite = boardSprite;
        }
        else
        {
            Debug.LogError("Failed to load board texture.");
        }
    }

    private void LoadChipTexture()
    {
        Addressables.LoadAssetAsync<Sprite>(chipTextureAddress).Completed += OnChipTextureLoaded;
    }

    private void OnChipTextureLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite chipSprite = handle.Result;
            chipImage.sprite = chipSprite;
        }
        else
        {
            Debug.LogError("Failed to load chip texture.");
        }
    }

    private void LoadDieFaceTextures()
    {
        // Load all die face textures using Addressables
        for (int i = 1; i <= 6; i++)
        {
            string dieFaceAddress = i.ToString();
            Addressables.LoadAssetAsync<Sprite>(dieFaceAddress).Completed += OnDieFaceLoaded;
        }
    }

    private void OnDieFaceLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite dieFaceSprite = handle.Result;
            dieSprites.Add(dieFaceSprite);
        }
        else
        {
            Debug.LogError("Failed to load die face texture.");
        }
    }
}
