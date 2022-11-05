using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textAsset;
    private string startText;
    private string currentText;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float punctuationPause = 0.2f;


    void Start()
    {
        textAsset = gameObject.GetComponent<TextMeshProUGUI>();
        startText = textAsset.text;
        StartCoroutine(Effect());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Effect()
    {
        for(int i = 0; i < startText.Length; i++)
        {
            currentText = startText.Substring(0, i);
            textAsset.text = currentText;

            if(startText[i].ToString() == "." || startText[i].ToString() == "!" || startText[i].ToString() == "?")
            {
                yield return new WaitForSeconds(punctuationPause);
            }
            else
            {
                yield return new WaitForSeconds(speed);
            }
        }
    }
}
