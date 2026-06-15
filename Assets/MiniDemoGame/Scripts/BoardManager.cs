using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private int startPosition = 0;
    [SerializeField] private int goalPosition = 100;
    [SerializeField] private Transform playerToken;
    [SerializeField] private Transform[] boardSpaces;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI positionText;

    private int currentPosition;

    public int CurrentPosition => currentPosition;
    public int GoalPosition => goalPosition;

    private void Start()
    {
        currentPosition = startPosition;
        UpdatePlayerPosition();
    }

    public void MovePlayer(int steps)
    {
        currentPosition += steps;
        UpdatePlayerPosition();
    }

    public bool IsBeforeStart()
    {
        return currentPosition < startPosition;
    }

    public bool ReachedGoal()
    {
        return currentPosition >= goalPosition;
    }

    private void UpdatePlayerPosition()
    {
        if (boardSpaces != null && boardSpaces.Length > 0)
        {
            int index = Mathf.Clamp(currentPosition, 0, boardSpaces.Length - 1);

            if (playerToken != null)
                playerToken.position = boardSpaces[index].position;
        }

        if (positionText != null)
            positionText.text = "Posición: " + currentPosition;
    }
}