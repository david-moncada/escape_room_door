using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine.UI;
public class NumberPad : MonoBehaviour
{
    public string sequence;

    public KeycardSpawner cardSpawner;

    public TextMeshProUGUI inputDisplayText;

    private string m_CurrentEnteredCode = "";

    private void Awake()
    {
        inputDisplayText.text = "Code Input: \n";
    }

    public void ButtonPressed(int valuePressed)
    {
        Debug.Log("Button Pressed: " + valuePressed);
        if (m_CurrentEnteredCode.Length >= sequence.Length)
        {
            return;
        }
        //append the number to the string (it will be converted to a string automatically but we could call the converting
        //function explicitly to make it more clear : valuePressed.ToString()
        m_CurrentEnteredCode += valuePressed;

        //if it's the first character inputed, we reset the display string. if we were reseting when the code is entered
        //we wouldn't have time to see the result (success/failure) so only reset on the next button press
        if (m_CurrentEnteredCode.Length == 1)
        {
            inputDisplayText.text += "*";
            inputDisplayText.color = Color.black;
        }
        else //otherwise we append a new * to the string
        {
            inputDisplayText.text += "*";
        }

        if (m_CurrentEnteredCode.Length == sequence.Length)
        {
            //we finished typing the sequence, check if we have a number of right input equal to the sequence
            if (m_CurrentEnteredCode == sequence)
            {
                Debug.Log("Right sequence entered");
                inputDisplayText.color = Color.green;
                inputDisplayText.text = "Code Valid!";
                //we are right! Spawn the keycard
                cardSpawner.SpawnKeyCard();
            }
            else
            {
                Debug.Log("Wrong sequenced entered");
                inputDisplayText.color = Color.red;
                inputDisplayText.text = "Invalid Code!";
            }

            //we reset the sequence checker
            ResetSequence(true);
        }
    }

    public void ResetSequence(bool clearText)
    {
        m_CurrentEnteredCode = "";

        if (clearText)
        {
            inputDisplayText.text = "Code Input: \n";
            inputDisplayText.color = Color.black;
        }
    }

}
