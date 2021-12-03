using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectable : MonoBehaviour
{
    [Tooltip("Button or Toggle on this object.")]
    [SerializeField] private Selectable selectableComponent;

    [SerializeField] private Character character;
    [SerializeField] private Image OnImage;
    [SerializeField] private Image OffImage;
    [SerializeField] private Image[] imagesForColor;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image playerIDImage;
    [SerializeField] private TextMeshProUGUI playerIDText;
    [SerializeField] private string playerIDTextBase = "P";
    
    // Start is called before the first frame update
    void Start()
    {
        OnImage.sprite = character.DefaultSprite;
        OnImage.color = character.SpriteColor;
        OffImage.sprite = character.DefaultSprite;
        nameText.text = character.characterName;

        for (int i = 0; i < imagesForColor.Length; i++)
        {
            imagesForColor[i].color = character.DefaultColor;
        }
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetPlayerIDText(int id)
    {
        playerIDImage.gameObject.SetActive(true);
        playerIDText.text = playerIDTextBase + id.ToString();
    }
}
