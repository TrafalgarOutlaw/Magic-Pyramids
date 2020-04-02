using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidHierarchy : MonoBehaviour
{
    public GameObject pyramidChild1;
    public GameObject pyramidChild2;
    bool startListener = false;

    void Awake()
    {
        startListener = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startListener)
        {
            if (pyramidChild1.transform.childCount == 0 && pyramidChild2.transform.childCount == 0)
            {
                transform.GetChild(0).GetComponent<SelectableCard>().faceUp = true;
                transform.GetChild(0).gameObject.AddComponent<Rigidbody2D>();
                transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                transform.GetChild(0).gameObject.AddComponent<BoxCollider2D>();
                transform.GetChild(0).tag = "PlayableCard";
                startListener = false;
            }
        }
    }
}
