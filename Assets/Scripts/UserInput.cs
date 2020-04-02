using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetMouseclick();
    }

    void GetMouseclick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit)
            {
                // what has been hit?
                switch (hit.collider.tag)
                {
                    case "Deck":
                        //print("clicked: deck");
                        GetComponent<MagicTower>().DrawCard(true);
                        break;
                    case "PlayableCard":
                        //print("clicked: card");
                        GetComponent<MagicTower>().PlayableCard(hit.transform.gameObject);
                        break;
                    default:
                        print("clicked something else: " + hit.collider.tag);
                        break;
                }
            }
        }
    }
}
