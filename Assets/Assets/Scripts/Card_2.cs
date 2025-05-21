using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_2: MonoBehaviour
{
    int cardId;
    public SpriteRenderer cardFront;
    public Animator anim;
    public GameObject fxConfetti;

    // SetCard now takes a list of sprites and chooses one randomly
    public void SetCard(int _id, List<Sprite> pairedSprites)
    {
        cardId = _id;
        cardFront.sprite = pairedSprites[Random.Range(0, pairedSprites.Count)];
    }

    public void FlipOpen(bool flipped)
    {
        anim.SetBool("FlippedOpen", flipped);
    }

    public int GetCardId()
    {
        return cardId;
    }

    public void ActivateConfetti()
    {
        fxConfetti.SetActive(true);
    }
}
