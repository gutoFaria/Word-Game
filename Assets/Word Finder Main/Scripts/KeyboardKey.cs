using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

enum Validity{ None, Valid, Potential, Invalid}


public class KeyboardKey : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image render;
    [SerializeField] private TextMeshProUGUI letterText;

    [Header("Settings")]
    private Validity validity;

    [Header("Events")]
    public static Action<char> onKeyPressd;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SendKeyPressedEvent);
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SendKeyPressedEvent()
    {
        onKeyPressd?.Invoke(letterText.text[0]);
    }

    public char GetLetter()
    {
        return letterText.text[0];
    }

    public void Initialize()
    {
        render.color = Color.white;
        validity = Validity.None;
    }

    public void SetValid()
    {
        render.color = Color.green;
        validity = Validity.Valid;
    }

    public void SetPotencial()
    {
        if(validity == Validity.Valid)
            return;

        render.color = Color.yellow;
        validity = Validity.Potential;
    }

    public void SetInValid()
    {
        if(validity == Validity.Valid || validity == Validity.Potential)
            return;

        render.color = Color.gray;
        validity = Validity.Invalid;
    }

    public bool IsUntouched()
    {
        return validity == Validity.None;
    }
}
