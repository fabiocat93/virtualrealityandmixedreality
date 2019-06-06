using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{

    private Text text;

    private int currentCount;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    public void Increment()
    {
        currentCount++;
        text.text = currentCount.ToString();
    }
}
