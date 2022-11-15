using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthOverlay : MonoBehaviour
{
    Character character;
    TextMeshProUGUI textContainer;

    void DisplayHealth()
    {
        textContainer.SetText("Health: {0} / {1}", character.currentHealth, character.maxHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();
        print(character);
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
        print(textContainer);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }
}
