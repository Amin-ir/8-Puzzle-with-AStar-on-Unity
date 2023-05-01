using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuAnimationFunctionScript : MonoBehaviour {

    public Sprite[] numberSprites;
    public GameObject[] numberGameObjects;
    void changeNumber()
    {
        foreach (var item in numberGameObjects)
        {
            var random = Random.Range(0, 8);
            item.GetComponent<SpriteRenderer>().sprite = numberSprites[random];
        }
    }

}
