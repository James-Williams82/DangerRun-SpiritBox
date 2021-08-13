using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening.Core.Easing;
using UnityEngine.Events;

public class SpiritBox : MonoBehaviour
{
    public bool isOn = false;

    public bool isDimmed = false;
    [Space]
    public TextMeshProUGUI sweepText;
    [Space]
    public List<Button> buttons = new List<Button>();

    public UnityEvent OnEvent;

    public UnityEvent OffEvent;

    public List<Material> SpiritBoxMaterials = new List<Material>();


    private Material currentMaterial;
    private Renderer spiritBoxRenderer;
    private float sweepRate = 3.5f;

    public void InitSpiritBox()
    {
        spiritBoxRenderer = this.GetComponent<Renderer>();

        currentMaterial = SpiritBoxMaterials[1];

        spiritBoxRenderer.material = currentMaterial;

        isDimmed = false;

        isOn = false;

        ToggleButtons(false);

        sweepText.gameObject.SetActive(false);

        OffEvent.Invoke();
    }

    public void TogglePower()
    {
        isOn = !isOn;

        if(isOn)
        {
            PowerOn();
        }
        else
        {
            PowerOff();
        }
    }

    public void PowerOff()
    {
        currentMaterial = SpiritBoxMaterials[1];

        spiritBoxRenderer.material = currentMaterial;

        ToggleButtons(false);

        OffEvent.Invoke();
    }

    public void PowerOn()
    {
        currentMaterial = SpiritBoxMaterials[0];

        spiritBoxRenderer.material = currentMaterial;

        ToggleButtons(true);

        OnEvent.Invoke();
    }

    public void DimScreen()
    {
        if (isOn)
        {
            isDimmed = !isDimmed;

            if (isDimmed)
            {
                currentMaterial = SpiritBoxMaterials[0];

                spiritBoxRenderer.material = currentMaterial;
            }
            else
            {
                currentMaterial = SpiritBoxMaterials[2];

                spiritBoxRenderer.material = currentMaterial;
            }
        }
    }

    public void ToggleButtons(bool b)
    {
        foreach (Button button in buttons)
        {
            button.interactable = b;
        }
    }

    public void SetSweepRate()
    {
        sweepRate = Random.Range(1.0f , 3.5f);

        sweepText.text = (Mathf.Round(sweepRate * 10) / 10).ToString() + "s";
    }

    public void SetSweepRateUp()
    {
        sweepRate += .5f;

        sweepText.text = (Mathf.Round(sweepRate * 10) / 10).ToString() + "s";
    }

    public void SetSweepRateDown()
    {
        sweepRate -= .5f;

        sweepText.text = (Mathf.Round(sweepRate * 10) / 10).ToString() + "s";
    }

}
