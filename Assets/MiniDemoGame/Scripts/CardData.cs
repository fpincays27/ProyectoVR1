using UnityEngine;

public class CardData : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public string CardName { get; private set; }
    public int CardValue { get; private set; }

    public void SetupCard(Sprite cardSprite, string cardName, int cardValue)
    {
        CardName = cardName;
        CardValue = cardValue;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = cardSprite;
    }
}