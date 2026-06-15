using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BlackjackCardGame : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform cardParent;
    [SerializeField] private int maxCardsPerTurn = 5;

    [Header("Card Sprites")]
    [SerializeField] private Sprite[] cardSprites;

    [Header("Game Settings")]
    [SerializeField] private int blackjackLimit = 21;
    [SerializeField] private int maxTurns = 10;
    [SerializeField] private string winSceneName = "Win";
    [SerializeField] private string gameOverSceneName = "GameOver";

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI messageText;

    private BoardManager boardManager;

    private List<GameObject> currentCards = new List<GameObject>();
    private Dictionary<string, int> cardValues = new Dictionary<string, int>();

    private int currentTurn = 1;
    private int currentScore = 0;
    private bool gameEnded = false;

    private void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();

        SetupCardValues();
        UpdateUI();

        if (messageText != null)
            messageText.text = "Presiona SPACE para sacar cartas.";
    }

    private void Update()
    {
        if (gameEnded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTurn();
        }
    }

    private void SetupCardValues()
    {
        cardValues.Add("A", 11);
        cardValues.Add("2", 2);
        cardValues.Add("3", 3);
        cardValues.Add("4", 4);
        cardValues.Add("5", 5);
        cardValues.Add("6", 6);
        cardValues.Add("7", 7);
        cardValues.Add("8", 8);
        cardValues.Add("9", 9);
        cardValues.Add("10", 10);
        cardValues.Add("J", 10);
        cardValues.Add("Q", 10);
        cardValues.Add("K", 10);
    }

    private void DrawCard()
    {
        if (currentCards.Count >= maxCardsPerTurn)
        {
            if (messageText != null)
                messageText.text = "Máximo 3 cartas. Presiona ENTER para terminar turno.";
            return;
        }

        if (cardPrefab == null || spawnPoint == null || cardSprites.Length == 0)
            return;

        int randomIndex = Random.Range(0, cardSprites.Length);
        Sprite selectedSprite = cardSprites[randomIndex];

        string cardName = GetCardName(selectedSprite.name);
        int value = GetCardValue(cardName);

        GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, cardParent);

        CardData cardData = newCard.GetComponent<CardData>();
        if (cardData != null)
        {
            cardData.SetupCard(selectedSprite, cardName, value);
        }

        currentCards.Add(newCard);
        currentScore += value;

        ArrangeCards();

        if (messageText != null)
            messageText.text = "Carta: " + cardName + " | Valor: " + value;

        UpdateUI();
    }

    private string GetCardName(string spriteName)
    {
        foreach (string key in cardValues.Keys)
        {
            if (spriteName.Contains(key))
                return key;
        }

        return "2";
    }

    private int GetCardValue(string cardName)
    {
        if (cardValues.ContainsKey(cardName))
            return cardValues[cardName];

        return 2;
    }

    private void ArrangeCards()
    {
        for (int i = 0; i < currentCards.Count; i++)
        {
            if (currentCards[i] != null)
            {
                Vector3 newPosition = spawnPoint.position + new Vector3(i * 1.2f, 0f, 0f);
                currentCards[i].transform.position = newPosition;
            }
        }
    }

    public void EndTurn()
    {
        if (currentCards.Count == 0)
        {
            if (messageText != null)
                messageText.text = "Debes sacar al menos una carta.";
            return;
        }

        if (boardManager == null)
            return;

        if (currentScore <= blackjackLimit)
        {
            boardManager.MovePlayer(currentScore);

            if (messageText != null)
                messageText.text = "Avanzas " + currentScore + " espacios.";
        }
        else
        {
            boardManager.MovePlayer(-currentScore);

            if (messageText != null)
                messageText.text = "Te pasaste. Retrocedes " + currentScore + " espacios.";
        }

        CheckGameState();
    }

    private void CheckGameState()
    {
        if (boardManager.ReachedGoal())
        {
            gameEnded = true;
            SceneManager.LoadScene(winSceneName);
            return;
        }

        if (boardManager.IsBeforeStart())
        {
            gameEnded = true;
            SceneManager.LoadScene(gameOverSceneName);
            return;
        }

        currentTurn++;

        if (currentTurn > maxTurns)
        {
            gameEnded = true;
            SceneManager.LoadScene(gameOverSceneName);
            return;
        }

        ClearTurnCards();
        currentScore = 0;
        UpdateUI();
    }

    private void ClearTurnCards()
    {
        foreach (GameObject card in currentCards)
        {
            if (card != null)
                Destroy(card);
        }

        currentCards.Clear();
    }

    private void UpdateUI()
    {
        if (turnText != null)
            turnText.text = "Turno: " + currentTurn + " / " + maxTurns;

        if (scoreText != null)
            scoreText.text = "Puntos: " + currentScore + " / " + blackjackLimit;
    }
}