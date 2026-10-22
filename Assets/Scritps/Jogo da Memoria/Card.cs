using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{
    public int cardValue;
    public SpriteRenderer cardBack;
    public SpriteRenderer cardFace;
    public event Action<Card> OnCardClicked;
    private bool isFlipped = false;

    public void SetupCard(Sprite face)
    {
        cardFace.sprite = face;
        cardBack.gameObject.SetActive(true);
        cardFace.gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        Debug.Log("Carta clicada: " + cardValue);

        if (OnCardClicked != null)
        {
            OnCardClicked(this);
        }
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        cardBack.gameObject.SetActive(!isFlipped);
        cardFace.gameObject.SetActive(isFlipped);
    }

    public bool IsFlipped
    {
        get { return isFlipped; }
    }
}
