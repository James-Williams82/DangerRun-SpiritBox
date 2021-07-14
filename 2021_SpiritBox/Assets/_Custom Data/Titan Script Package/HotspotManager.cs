using Com.FastEffect.DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotspotManager : MonoBehaviour
{
    [System.Serializable]
    public class IntUEvent:UnityEvent<int>{}

    [SerializeField]
    private BoolReference m_inAssistedMode = new BoolReference(true);

    [Tooltip("This is uses a 1-Count range, instead of a 0-(Count-1) range")]
    public IntReference m_initialHotspotIndex = new IntReference(0);

    public List<GameObject> Hotspots = new List<GameObject>();

    [SerializeField]
    private IntUEvent m_onHotspotChange = new IntUEvent();

    private int m_currentIndex = 0;
    public void SetCurrentIndex(int index)
    {
        m_currentIndex = index % Hotspots.Count;
    }
    public void SetHotspot(int newHotspot) 
    {
        if(Hotspots.Count > 0)
        {
            foreach (GameObject h in Hotspots)
            {
                h.SetActive(false);
            }
            m_currentIndex = newHotspot % Hotspots.Count;
            Hotspots[m_currentIndex].SetActive(true);
            m_onHotspotChange.Invoke(m_currentIndex+1);
        }
    }
    public void NextHotspot()
    {
        if(Hotspots.Count > 0)
        {
            Hotspots[m_currentIndex].SetActive(false);
            m_currentIndex = (m_currentIndex + 1) % Hotspots.Count;
            Hotspots[m_currentIndex].SetActive(true);
            m_onHotspotChange.Invoke(m_currentIndex+1);
        }
    }
    public void PreviousHotspot()
    {
        if(Hotspots.Count > 0)
        {
            Hotspots[m_currentIndex].SetActive(false);
            m_currentIndex = (m_currentIndex - 1) % Hotspots.Count;
            Hotspots[m_currentIndex].SetActive(true);
            m_onHotspotChange.Invoke(m_currentIndex+1);
        }
    }

    void Awake()
    {
        // should not include inactive hotspots
        foreach(Hotspot spot in GetComponentsInChildren<Hotspot>(false))
        {
            if(!Hotspots.Contains(spot.gameObject))
            {
                Hotspots.Add(spot.gameObject);
            }
        }
    }

    public void OnEnable()
    {
        if(m_initialHotspotIndex > 0)
        {
            m_currentIndex = (m_initialHotspotIndex-1) % Hotspots.Count;
        }
        if(m_inAssistedMode)
        {
            AssistedMode();
        }
    }

    public void AssistedMode()
    {
        foreach (GameObject h in Hotspots)
        {
            h.SetActive(false);
        }
        m_currentIndex = m_currentIndex % Hotspots.Count;
        //Debug.LogFormat("index: {0}, count: {1}",m_currentIndex,Hotspots.Count);
        Hotspots[m_currentIndex].SetActive(true);
        m_onHotspotChange.Invoke(0);
    }

    public void UnAssistedMode()
    {
        Debug.Log("Unassisted");

        foreach (GameObject hspot in Hotspots)
        {
            hspot.SetActive(true);
        }

        m_onHotspotChange.Invoke(0);
    }




}
