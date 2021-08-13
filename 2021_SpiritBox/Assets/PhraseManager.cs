using Com.FastEffect.DataTypes;
using DG.Tweening;
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
    public string audioKey = "";
    public float delay;
    public List<string> _phrase = new List<string>();
    public UnityEvent phraseEvent;
}

public class PhraseManager : MonoBehaviour
{
    public CanvasGroup canvas;

    public TMP_InputField UserInputField;

    public bool disableInput = false;

    public string userEnteredPhrase;

    public FloatValue inputDelay;

    public List<Phrase> Phrases = new List<Phrase>();

    public UnityEvent correctPhraseEvent;

    public UnityEvent incorrectPhraseEvent;

    public void CheckPhrase()
    {
        userEnteredPhrase = UserInputField.text.ToUpper();

        

        for (int i = 0; i < Phrases.Count; i++)
        {
            if (Phrases[i]._phrase.Contains(userEnteredPhrase.ToUpper()))
            {
                Debug.Log("Phrase " + userEnteredPhrase +" Found");

                //Phrase Specific Response
                Phrases[i].phraseEvent.Invoke();

                //Universal Correct Response
                correctPhraseEvent.Invoke();

                if (disableInput)
                {
                    //Disable Input 
                    DisableInput();
                }

                //Break Out if Phrase Found
                break;
            }
            else
            {
                Debug.Log(userEnteredPhrase + " Phrase Not Found");
                
                //Universal Incorrect Response
                incorrectPhraseEvent.Invoke();
            }
        }
    }

    public void DisableInput()
    {
        float delay = inputDelay.Value;

        canvas.interactable = false;

        UserInputField.interactable = false;

        Sequence sequence = DOTween.Sequence();

        sequence.PrependInterval(delay);

        sequence.OnComplete(EnableInput);
    }

    public void EnableInput()
    {
        UserInputField.text = "";

        canvas.interactable = true;

        UserInputField.interactable = true;

        inputDelay.Value = 0;

    }

}
