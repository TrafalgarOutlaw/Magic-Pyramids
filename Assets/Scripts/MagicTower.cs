using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MagicTower : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject playableCard1Pos;
    public GameObject playableCard2Pos;
    public GameObject playableCard3Pos;
    public GameObject deckObject;
    public GameObject pyramids;
    public UserInput UserIput;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    private List<string> deck;

    private string[] bottoms;
    private string[] tops;
    private string playable;

    private GameObject newCard1;
    private double newCard1Value;
    private GameObject newCard2;
    private double newCard2Value;
    private GameObject newCard3;
    private double newCard3Value;

    private short extraCard = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        deckObject.SetActive(true);
        //Instantiate(playableCard1Pos.transform.parent.gameObject);
        /*GameObject newPyramids =  Instantiate(pyramids);
        Destroy(pyramids);
        pyramids = newPyramids;*/
        //pyramids.SetActive(true);
        //playableCards.SetActive(true);
        bottoms = new string[10];
        tops = new string[18];

        foreach(GameObject card in bottomPos)
        {
            card.SetActive(true);
        }

        foreach (GameObject card in topPos)
        {
            card.SetActive(true);
        }

        PlayCards();
        UserIput.enabled = true;
    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);

        MagicTowerSort();
        StartCoroutine(MagicTowersDeal());
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach(string s in suits)
        {
            foreach(string v in values)
            {
                newDeck.Add(s + v);
            }
        }

        return newDeck;
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while(n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator MagicTowersDeal()
    {
        for (int i = 0; i < tops.Length; i++)
        {
            if(topPos[i].transform.childCount > 0)
            {
                Destroy(topPos[i].transform.GetChild(0).gameObject);
            }
            yield return new WaitForSeconds(0.02f);
            GameObject tmpCard = Instantiate(cardPrefab, topPos[i].transform.position, Quaternion.identity, topPos[i].transform);
            tmpCard.name = tops[i];
            tmpCard.GetComponent<SpriteRenderer>().sortingOrder = tmpCard.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
        }

        for (int i = 0; i < bottoms.Length; i++)
        {
            if (bottomPos[i].transform.childCount > 0)
            {
                Destroy(bottomPos[i].transform.GetChild(0).gameObject);
            }
            yield return new WaitForSeconds(0.02f);
            GameObject tmpCard = Instantiate(cardPrefab, bottomPos[i].transform.position, Quaternion.identity, bottomPos[i].transform);
            tmpCard.name = bottoms[i];
            tmpCard.GetComponent<SelectableCard>().faceUp = true;
            tmpCard.GetComponent<SpriteRenderer>().sortingOrder = tmpCard.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
            tmpCard.tag = "PlayableCard";
            tmpCard.AddComponent<Rigidbody2D>();
            tmpCard.GetComponent<Rigidbody2D>().gravityScale = 0;
            tmpCard.AddComponent<BoxCollider2D>();
            //print("Parent from" + tmpCard.name + ": " + tmpCard.transform.parent.name);
        }

        if (playableCard1Pos.transform.childCount > 0)
        {
            Destroy(playableCard1Pos.transform.GetChild(0).gameObject);
        }
        yield return new WaitForSeconds(0.02f);
        newCard1 = Instantiate(cardPrefab, playableCard1Pos.transform.position, Quaternion.identity, playableCard1Pos.transform);
        newCard1.name = playable;
        newCard1.GetComponent<SelectableCard>().faceUp = true;
        newCard1Value = GetCardValue(newCard1);
        newCard1.GetComponent<SpriteRenderer>().sortingOrder = newCard1.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

        for (int i = 0; i < pyramids.transform.childCount - 10; i++)
        {
            pyramids.transform.GetChild(i).GetComponent<PyramidHierarchy>().enabled = true;
        }

        deckObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = deck.Count.ToString();
    }

    void MagicTowerSort()
    {
        for (int i = 0; i < tops.Length; i++)
        {
            tops[i] = deck.Last<string>();
            deck.RemoveAt(deck.Count - 1);
        }

        for (int i = 0; i < bottoms.Length; i++)
        {
            bottoms[i] = deck.Last<string>();
            deck.RemoveAt(deck.Count - 1);
        }

        playable = deck.Last<string>();
        deck.RemoveAt(deck.Count - 1);
    }

    public void DrawCard(bool drawClicked)
    {
        if (deck.Count == 0)
        {
            return;
        }

        playable = deck.Last<string>();
        deck.RemoveAt(deck.Count - 1);

        deckObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = deck.Count.ToString();

        if(drawClicked)
        {
            extraCard = 0;
        }

        if (extraCard < 3)
        {
            Destroy(newCard1);
            Destroy(newCard2);
            Destroy(newCard3);
            newCard1 = Instantiate(cardPrefab, playableCard1Pos.transform.position, Quaternion.identity, playableCard1Pos.transform);
            newCard1.name = playable;
            newCard1.GetComponent<SelectableCard>().faceUp = true;
            newCard1Value = GetCardValue(newCard1);

            Destroy(newCard2);
            Destroy(newCard3);
        }

        if (extraCard == 3)
        {
            Destroy(newCard2);
            newCard2 = Instantiate(cardPrefab, playableCard2Pos.transform.position, Quaternion.identity, playableCard2Pos.transform);
            newCard2.name = playable;
            newCard2.GetComponent<SelectableCard>().faceUp = true;
            newCard2Value = GetCardValue(newCard2);
        }

        if (extraCard == 6)
        {
            Destroy(newCard3);
            newCard3 = Instantiate(cardPrefab, playableCard3Pos.transform.position, Quaternion.identity, playableCard3Pos.transform);
            newCard3.name = playable;
            newCard3.GetComponent<SelectableCard>().faceUp = true;
            newCard3Value = GetCardValue(newCard3);
        }
        
        if(deck.Count == 0)
        {
            TestGameOver();
            deckObject.SetActive(false);
            //StartRound();
        }
    }



    public void PlayableCard(GameObject cardClicked)
    {
        double clickedCardValue = GetCardValue(cardClicked);
        double compareVal1 = (newCard1Value + 1) % 13;
        double compareVal2 = (newCard1Value - 1) % 13;
        compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;
        /*
         * print("PlayableCard clicked: " + cardClicked.name);
         * print("PlayableCard clicked value: " + clickedCardValue);
         */

        if (clickedCardValue == compareVal1 || clickedCardValue == compareVal2)
        {
            //cardClicked.transform.parent.gameObject.SetActive(false);
            cardClicked.transform.parent = newCard1.transform.parent;
            newCard1Value = clickedCardValue;
            cardClicked.transform.localPosition = Vector3.zero;
            Destroy(newCard1);
            cardClicked.tag = "Untagged";
            newCard1 = cardClicked;
            newCard1.GetComponent<SpriteRenderer>().sortingOrder = newCard1.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

            extraCard++;
            if (extraCard == 3 || extraCard == 6)
            {
                DrawCard(false);
            }

            if (deck.Count == 0)
            {
                TestGameOver();
            }
            return;
        }

        if (newCard2 != null)                                                    /* If second card is available and first card did not match with clicked card.  */
        {
            compareVal1 = (newCard2Value + 1) % 13;
            compareVal2 = (newCard2Value - 1) % 13;
            compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;

            if (clickedCardValue == compareVal1 || clickedCardValue == compareVal2)
            {
                cardClicked.transform.parent = newCard2.transform.parent;
                newCard2Value = clickedCardValue;
                cardClicked.transform.localPosition = Vector3.zero;
                Destroy(newCard2);
                cardClicked.tag = "Untagged";
                newCard2 = cardClicked;
                newCard2.GetComponent<SpriteRenderer>().sortingOrder = newCard2.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

                extraCard++;
                if (extraCard == 6)
                {
                    DrawCard(false);
                }

                if (deck.Count == 0)
                {
                    TestGameOver();
                }
                return;
            }

        }

        if (newCard3 != null)                                                     /* If third card is available and first/second card did not match with clicked card.   */
        {
            compareVal1 = (newCard3Value + 1) % 13;
            compareVal2 = (newCard3Value - 1) % 13;
            compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;

            if (clickedCardValue == compareVal1 || clickedCardValue == compareVal2)
            {
                cardClicked.transform.parent = newCard3.transform.parent;
                newCard3Value = clickedCardValue;
                cardClicked.transform.localPosition = Vector3.zero;
                Destroy(newCard3);
                cardClicked.tag = "Untagged";
                newCard3 = cardClicked;
                newCard3.GetComponent<SpriteRenderer>().sortingOrder = newCard3.transform.parent.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

                if (deck.Count == 0)
                {
                    TestGameOver();
                }
                return;
            }
        }
    }

    double GetCardValue(GameObject card)
    {
        char charVal = card.name[1];
        double retVal;

        switch (charVal)
        {
            case 'A':
                retVal = 0;
                return retVal;
            case 'K':
                retVal = 12;
                return retVal;
            case 'Q':
                retVal = 11;
                return retVal;
            case 'J':
                retVal = 10;
                return retVal;
            default:
                if (card.name.Length > 2)
                {
                    retVal = 9;
                }
                else
                {
                    retVal = Char.GetNumericValue(charVal) - 1 ;
                }
                return retVal;
        }
    }

    void TestGameOver()
    {
        double fieldcardValue;
        double compareVal1;
        double compareVal2;
        double playbleCardVal;

        GameObject[] allFieldcards = GameObject.FindGameObjectsWithTag("PlayableCard");
        foreach (GameObject card in allFieldcards)
        {
            fieldcardValue = GetCardValue(card);

            if (playableCard1Pos.transform.childCount > 0)
            {
                playbleCardVal = GetCardValue(playableCard1Pos.transform.GetChild(1).gameObject);
                compareVal1 = (playbleCardVal + 1) % 13;
                compareVal2 = (playbleCardVal - 1) % 13;
                compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;

                if (fieldcardValue == compareVal1 || fieldcardValue == compareVal2)
                {
                    return;
                }
            }

            if (playableCard2Pos.transform.childCount > 0)
            {
                playbleCardVal = GetCardValue(playableCard2Pos.transform.GetChild(1).gameObject);
                compareVal1 = (playbleCardVal + 1) % 13;
                compareVal2 = (playbleCardVal - 1) % 13;
                compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;

                if (fieldcardValue == compareVal1 || fieldcardValue == compareVal2)
                {
                    return;
                }
            }

            if (playableCard3Pos.transform.childCount > 0)
            {
                playbleCardVal = GetCardValue(playableCard3Pos.transform.GetChild(1).gameObject);
                compareVal1 = (playbleCardVal + 1) % 13;
                compareVal2 = (playbleCardVal - 1) % 13;
                compareVal2 = compareVal2 < 0 ? compareVal2 + 13 : compareVal2;

                if (fieldcardValue == compareVal1 || fieldcardValue == compareVal2)
                {
                    return;
                }
            }
        }
        print("IS OVER");
        UserIput.enabled = false;
    }
}
