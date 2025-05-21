using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.HasPicked() && !GameManager.instance.GameisOver())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Depending on the GameManager's GameObject name, handle either Card or Card_2
                if (GameManager.instance.gameObject.name == "GameManager_Phase1")
                {
                    HandleCardInput<Card>(hit);
                }
                else if (GameManager.instance.gameObject.name == "GameManager_Phase2")
                {
                    HandleCardInput<Card_2>(hit);
                }
            }
        }
    }

    private void HandleCardInput<T>(RaycastHit hit) where T : MonoBehaviour
    {
        T currentCard = hit.transform.GetComponent<T>();
        if (currentCard != null)
        {
            // Cast to either Card or Card_2 and process input
            if (typeof(T) == typeof(Card))
            {
                ProcessCardInput(currentCard as Card);
            }
            else if (typeof(T) == typeof(Card_2))
            {
                ProcessCard2Input(currentCard as Card_2);
            }
        }
    }

    private void ProcessCardInput(Card card)
    {
        card.FlipOpen(true);
        GameManager.instance.AddCardToPickedlist(card);
    }

    private void ProcessCard2Input(Card_2 card)
    {
        card.FlipOpen(true);
        GameManager.instance.AddCardToPickedlist(card);
    }
}
