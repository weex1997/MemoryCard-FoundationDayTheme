using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager_2: MonoBehaviour
{
    [SerializeField] private int pairAmount;
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private float offset = 1.6f;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<GameObject> cardDeck = new List<GameObject>();
    [HideInInspector] public int width;
    [HideInInspector] public int height;

    private Dictionary<int, List<Sprite>> pairedSprites = new Dictionary<int, List<Sprite>>();

    void Start()
    {
        InitializePairedSprites();
        GameManager.instance.SetPairs(pairAmount);
        CreatePlayerField();
    }

    private void InitializePairedSprites()
    {
        // Check for an even number of sprites
        if (spriteList.Count % 2 != 0)
        {
            Debug.LogError("Sprite list does not contain an even number of elements.");
            return;
        }

        // Pair adjacent sprites with unique IDs
        for (int i = 0; i < spriteList.Count; i += 2)
        {
            int pairId = i / 2;
            if (pairId < pairAmount)
            {
                pairedSprites[pairId] = new List<Sprite> { spriteList[i], spriteList[i + 1] };
            }
        }
    }

    private void CreatePlayerField()
    {
        int totalCards = pairAmount * 2;

        // Shuffle the IDs to randomize pair positions
        List<int> shuffledPairIds = new List<int>();
        for (int i = 0; i < pairAmount; i++)
        {
            shuffledPairIds.Add(i);
        }
        ShuffleList(shuffledPairIds);

        // Create cards for each pair
        for (int i = 0; i < totalCards; i += 2)
        {
            int pairId = shuffledPairIds[i / 2];
            List<Sprite> pairSprites = pairedSprites[pairId];

            // Create two cards for the pair
            CreateCard(pairId, pairSprites[0], i);
            CreateCard(pairId, pairSprites[1], i + 1);
        }

        ShuffleCards();
        ArrangeCardsOnField();
    }

    private void CreateCard(int id, Sprite sprite, int indexInDeck)
    {
        Vector3 pos = Vector3.zero;
        GameObject newCard = Instantiate(cardPrefab, pos, Quaternion.identity);
        newCard.GetComponent<Card_2>().SetCard(id, new List<Sprite> { sprite });
        cardDeck.Add(newCard);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < cardDeck.Count; i++)
        {
            int index = Random.Range(0, cardDeck.Count);
            var temp = cardDeck[i];
            cardDeck[i] = cardDeck[index];
            cardDeck[index] = temp;
        }
    }

    private void ArrangeCardsOnField()
    {
        int num = 0;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (num < cardDeck.Count)
                {
                    Vector3 pos = new Vector3(x * offset, 0, z * offset);
                    cardDeck[num].transform.position = pos;
                    num++;
                }
            }
        }
    }


    void OnDrawGizmos() // to make the PairAmount and Width , Height equals and not causing errors
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {

                Vector3 pos = new Vector3(x * offset, 0, z * offset);
                Gizmos.DrawWireCube(pos, new Vector3(1, 0.1f, 1));
            }
        }
    }
}
