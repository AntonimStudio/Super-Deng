using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/Tutorial")]
public class TutorialSettings : ScriptableObject
{
    [SerializeField] private StepInTutorial[] _messages;
    public int CountMessages => _messages.Length;

    public StepInTutorial this[int i]
    {
        get
        {
            return _messages[i];
        }
    }
}

[Serializable]
public class StepInTutorial
{
    [SerializeField] private string _message;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isBeat;
    [SerializeField] private bool _isKilling;

    public string Message => _message;
    public bool isMoving => _isMoving;
    public bool isBeat => _isBeat;
    public bool isKilling => _isKilling;

    public StepInTutorial(string message, bool isMoving, bool isBeat, bool isKilling)
    {
        _message = message;
        _isMoving = isMoving;
        _isBeat = isBeat;
        _isKilling = isKilling;
    }
}