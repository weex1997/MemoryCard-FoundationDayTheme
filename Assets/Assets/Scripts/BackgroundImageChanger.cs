using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageChanger: MonoBehaviour
{
    public Sprite[] backgroundImages; // Array to hold your background images

    void Start()
    {
        if (backgroundImages.Length > 0)
        {
            int randomIndex = Random.Range(0, backgroundImages.Length); // Randomly pick an index
            GetComponent<Image>().sprite = backgroundImages[randomIndex]; // Set the chosen image
        }
    }
}
