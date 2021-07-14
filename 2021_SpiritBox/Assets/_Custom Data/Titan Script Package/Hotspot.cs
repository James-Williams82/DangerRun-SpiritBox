using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hotspot : MonoBehaviour
{
    public float delay = 1;

    public bool canClick = true;

    public UnityEvent OnClickEvents;

    public UnityEvent DelayedOnClickEvents;

    private void OnEnable()
    {
        canClick = true;
    }


    private void OnMouseDown()
    {
        if (canClick)
        {
            OnClickClcyeStart();
        }
    }

    public void OnClickClcyeStart()
    {
        StartCoroutine(OnClickCycle());
    }


    public IEnumerator OnClickCycle()
    {
        canClick = false;

        OnClickEvents.Invoke();

        yield return new WaitForSeconds(delay);

        DelayedOnClickEvents.Invoke();

        Debug.Log("The Hotspot " + this.gameObject.name + " was clicked!!");

        canClick = true;

    }



}
