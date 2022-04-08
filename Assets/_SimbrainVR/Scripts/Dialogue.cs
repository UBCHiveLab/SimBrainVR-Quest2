using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;


    private void Start()
    {
        textComponent.text = string.Empty;
        //StartDialogue();
        StartCoroutine(TypeAllLines());
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }*/

    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void TypeSpecificLine(string line)
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        textComponent.text = string.Empty;
        
        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    IEnumerator TypeAllLines()
    {
        index = 0;

        while(index < lines.Length)
        {
            textComponent.text = string.Empty;

            foreach (char c in lines[index].ToCharArray())
            {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            index++;
        }
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //nothing else in dialogue.
            gameObject.SetActive(false);
        }
    }
}
