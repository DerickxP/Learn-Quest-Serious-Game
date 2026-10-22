using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class MemoryGame : MonoBehaviour
{
    [Header("Referências")]
    public GameObject cardPrefab;
    public GameObject Victory; 
    public GameObject VictoryMaxPairs;
    public GameObject GameOverPanel;
    public Button nextLevelButton;
    public Transform boardArea;
    public Sprite[] cardFaces;
    public string cardFacePath = "CardFaces";
    public float offsetX = 2f;
    public float offsetY = 2.5f;

    private int totalPairs = 2;
    private List<int> randomFaces = new List<int>();
    private Card firstCard;
    private Card secondCard;
    private bool canFlip = true;
    private int matchesFound = 0;

    public Timer timer;
    private int consecutiveMatches = 0;
    public int hint = 1;
    public Button hintButton;
    public int AttemptCount = 0;
    public TMP_Text attemptText;
    public AnimacaoImagem animacaoImagem;

    void Start()
    {
        LoadCardFaces();
        InitializeCards(totalPairs);
        nextLevelButton.onClick.AddListener(NextLevel);
        hintButton.onClick.AddListener(UseHint);
        UpdateAttempt();
    }

    void LoadCardFaces()
    {
        cardFaces = Resources.LoadAll<Sprite>(cardFacePath);
        if (cardFaces.Length == 0)
        {
            Debug.LogError("Nenhuma face de carta encontrada na pasta especificada.");
        }
    }

    void InitializeCards(int pairs)
    {
        matchesFound = 0;
        randomFaces.Clear();
        if (pairs > cardFaces.Length)
        {
            Debug.LogError("Não há sprites suficientes para preencher todos os cartões.");
            return;
        }

        for (int i = 0; i < pairs; i++)
        {
            randomFaces.Add(i);
            randomFaces.Add(i);
        }

        Shuffle(randomFaces);
        int totalCards = pairs * 2;
        int gridRows = Mathf.FloorToInt(Mathf.Sqrt(totalCards));
        int gridCols = Mathf.CeilToInt((float)totalCards / gridRows);

        if (gridRows * gridCols < totalCards)
        {
            gridCols++;
        }

        float totalWidth = (gridCols - 1) * offsetX;
        float totalHeight = (gridRows - 1) * offsetY;
        Vector3 startPos = new Vector3(-totalWidth / 2, totalHeight / 2, 0);

        foreach (Transform child in boardArea)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gridRows; i++)
        {
            for (int j = 0; j < gridCols; j++)
            {
                if (randomFaces.Count <= 0)
                {
                    return;
                }

                GameObject newCard = Instantiate(cardPrefab, boardArea);

                Vector3 cardPos = startPos + new Vector3(j * offsetX, -i * offsetY, 0);
                newCard.transform.localPosition = cardPos;

                Card card = newCard.GetComponent<Card>();
                int randomIndex = Random.Range(0, randomFaces.Count);
                card.cardValue = randomFaces[randomIndex];
                card.SetupCard(cardFaces[card.cardValue]);

                randomFaces.RemoveAt(randomIndex);
                card.OnCardClicked += HandleCardClicked;
            }
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void HandleCardClicked(Card card)
    {
        Debug.Log("Cartão clicado: " + card.cardValue);
        if (Victory.activeSelf || VictoryMaxPairs.activeSelf) 
        {
            return;
        }

        if (canFlip && !card.IsFlipped && (firstCard == null || secondCard == null))
        {
            card.Flip();
            if (firstCard == null)
            {
                firstCard = card;
            }
            else if (secondCard == null && card != firstCard)
            {
                secondCard = card;
                AttemptCount++;
                UpdateAttempt();
                Debug.Log("Número de tentativas: " + AttemptCount);

                StartCoroutine(CheckMatch());
            }
        }
    }

    IEnumerator CheckMatch()
    {
        canFlip = false;

        yield return new WaitForSeconds(1.5f);

        if (firstCard.cardValue == secondCard.cardValue)
        {
            Debug.Log("Par Encontrado!");
            matchesFound++;
            consecutiveMatches++;

            if (consecutiveMatches >= 2)
            {
                Debug.Log("Adicionando 10 segundos ao timer!");
                timer.AddTime(10f);

                if (Victory.activeSelf == false && VictoryMaxPairs.activeSelf == false)
                {
                    if (animacaoImagem != null)
                    {
                        StartCoroutine(animacaoImagem.AnimarImagem());
                    }
                }

                consecutiveMatches = 0;
            }

            if (matchesFound >= totalPairs)
            {
                Debug.Log("Você venceu!");
                if (totalPairs == cardFaces.Length)
                {
                    VictoryMaxPairs.SetActive(true); 
                }
                else
                {
                    Victory.SetActive(true); 
                }
                timer.TogglePause(true);
            }
        }
        else
        {
            firstCard.Flip();
            secondCard.Flip();
            consecutiveMatches = 0;
        }

        firstCard = null;
        secondCard = null;
        canFlip = true;
    }

    void NextLevel()
    {
        if (animacaoImagem != null && animacaoImagem.gameObject.activeSelf)
        {
            animacaoImagem.gameObject.SetActive(false);
        }

        if (matchesFound >= 4)
        {
            totalPairs += 2;
        }
        else
        {
            totalPairs++;
        }

        int maxPairs = cardFaces.Length;
        if (totalPairs > maxPairs)
        {
            totalPairs = maxPairs;
        }

        AttemptCount = 0;
        UpdateAttempt();

        foreach (Transform child in boardArea)
        {
            Destroy(child.gameObject);
        }

        InitializeCards(totalPairs);
        Victory.SetActive(false);
        VictoryMaxPairs.SetActive(false);
        timer.ResetTimer(60f);
        hint = 1;
        hintButton.interactable = true;
    }


    public void UseHint()
    {
        if (hint > 0 && canFlip)
        {
            hint--;
            List<Card> unmatchedCards = new List<Card>();
            foreach (Transform child in boardArea)
            {
                Card card = child.GetComponent<Card>();
                if (!card.IsFlipped)
                {
                    unmatchedCards.Add(card);
                }
            }

            if (unmatchedCards.Count >= 2)
            {
                Card card1 = unmatchedCards[Random.Range(0, unmatchedCards.Count)];
                Card card2 = unmatchedCards.Find(c => c.cardValue == card1.cardValue && c != card1);

                if (card2 != null)
                {
                    StartCoroutine(RevealCardsTemporarily(card1, card2));
                }
            }
            if (hint <= 0)
            {
                hintButton.interactable = false;
            }
        }
    }

    IEnumerator RevealCardsTemporarily(Card card1, Card card2)
    {
        card1.Flip();
        card2.Flip();
        yield return new WaitForSeconds(1.5f);
        card1.Flip();
        card2.Flip();
    }

    void UpdateAttempt()
    {
        attemptText.text = AttemptCount.ToString();
    }
}
