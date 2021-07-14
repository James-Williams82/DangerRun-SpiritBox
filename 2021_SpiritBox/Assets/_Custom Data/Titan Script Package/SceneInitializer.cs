using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class SceneInitializer : MonoBehaviour, ISceneInitializer
{
    [Header("Delay after running PreSetup")]
    public float initDelay = 0;

    [Space]
    [Header("Events Fired Before Start of Level ")]
    public UnityEvent PreInitializationEvents;


    [Space]
    [Header("Events Fired at Start of Level ")]
    public UnityEvent InitializationEvents;

    [Space]
    [Header("Events Fired at End of Level ")]
    public UnityEvent UninitializationEvents;



    public void OpenEvents()
    {
        InitializationEvents.Invoke();
    }

    public void CloseEvents()
    {
        UninitializationEvents.Invoke();
    }

    private IEnumerator Start()
    {
        PreInitializationEvents.Invoke();

        Debug.Log("Setting Up Scene");

        yield return new WaitForSeconds(initDelay);

        Debug.Log("Ready To Start");

        InitializationEvents.Invoke();  
    }



}
