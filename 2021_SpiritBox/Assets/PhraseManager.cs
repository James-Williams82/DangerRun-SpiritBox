using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Phrase
{
    public bool locked;
    public string _phrase;
    public UnityEvent phraseEvent;
}

public class PhraseManager : MonoBehaviour
{
    public TMP_InputField GetInput;

    public string currentPhrase;

    public List<Phrase> Phrases = new List<Phrase>();

    public UnityEvent correctPhraseEvent;

    public UnityEvent incorrectPhraseEvent;


    public void CheckPhrase()
    {
        int value = 0;

        currentPhrase = GetInput.text.ToLower();

        foreach (Phrase p in Phrases)
        {
            value ++;

            if (currentPhrase == p._phrase.ToLower())
            {
                Debug.Log("The phrase " + currentPhrase + " matches " + p._phrase);

                p.phraseEvent.Invoke();

                p.locked = true;

                GetInput.text = "";

                value = 0;

                correctPhraseEvent.Invoke();
            }
            else
            {
                Debug.Log("The phrase " + currentPhrase + " does not match " + p._phrase);

                if (value == Phrases.Count)
                {
                    incorrectPhraseEvent.Invoke();

                    GetInput.text = "";

                    value = 0;
                }

            }


            /*
            if (p.locked)
            {
                Debug.Log("The phrase " + p._phrase + " has been unlocked");

                return;
            }
            else
            {
               
            }
            */
        }
    }


}
