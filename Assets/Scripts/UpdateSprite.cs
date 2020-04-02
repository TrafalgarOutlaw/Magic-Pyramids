using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private SelectableCard selectableCard;
    private MagicTower magicTower;

    
    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = MagicTower.GenerateDeck();
        magicTower = FindObjectOfType<MagicTower>();

        int i = 0;
        foreach(string card in deck)
        {
            if(this.name == card)
            {
                cardFace = magicTower.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectableCard = GetComponent<SelectableCard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectableCard.faceUp)
        {
            spriteRenderer.sprite = cardFace;
        } else
        {
            spriteRenderer.sprite = cardBack;
        }
    }
}
