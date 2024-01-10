using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoGameManager : MonoBehaviour
{
    // Variables to handle game board and chip movement
    public List<Transform> boardCells; // Transforms representing the board cells
    public Transform chipTransform; // Reference to the chip's Transform
    public Transform initialTransform; // Initial position of the chip
    int playerPosition = 0; // Player's current position on the board
    public bool hasRolled = false; // Indicates whether the dice has been rolled

    // Method to move the chip based on dice value
    public void MoveChip(int steps)
    {
        if (hasRolled) // Check if the dice has been rolled
        {
            int newPosition = playerPosition + steps;

            // Check if the new position is within the board bounds
            if (newPosition < boardCells.Count)
            {
                // Conditions for specific cell positions
                if (playerPosition >= 52 && newPosition == 52 && steps != 5)
                {
                    Debug.Log("You need to roll 5 to enter!");
                    return; // Do not move if not rolled 5
                }
                else if (playerPosition >= 53 && newPosition == 53 && steps != 4)
                {
                    Debug.Log("You need to roll 4 to enter!");
                    return; // Do not move if not rolled 4
                }
                else if (playerPosition >= 54 && newPosition == 54 && steps != 3)
                {
                    Debug.Log("You need to roll 3 to enter!");
                    return; // Do not move if not rolled 3
                }
                else if (playerPosition >= 55 && newPosition == 55 && steps != 2)
                {
                    Debug.Log("You need to roll 2 to enter!");
                    return; // Do not move if not rolled 2
                }
                else if (playerPosition >= 56 && newPosition == 56 && steps != 1)
                {
                    Debug.Log("You need to roll 1 to enter!");
                    return; // Do not move if not rolled 1
                }

                playerPosition = newPosition; // Update player's position
                Transform newPositionTransform = boardCells[playerPosition];

                // Move the chip to the new position
                MoveChipTo(newPositionTransform.position);
                hasRolled = false; // Reset roll status after movement
            }
        }
    }

    // Method to move the chip's Transform to a new position
    void MoveChipTo(Vector3 newPosition)
    {
        chipTransform.position = newPosition;
    }

    // Reset the chip's position to the initial position
    public void ResetChipPosition()
    {
        chipTransform.position = initialTransform.position;
        playerPosition = 0; // Reset player's position to start
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
