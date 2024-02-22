using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("Elements")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button tryButton;
    [SerializeField] private KeyboardColorizer keyboardColorizer;

    [Header("Settings")]
    private int currentWordContainerIndex;
    private bool canAddLetter = true;
    private bool shouldReset;

    [Header("Events")]
    public static Action onLetterAdded;
    public static Action onLetterRemoved;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Initialize();

        KeyboardKey.onKeyPressd += KeyPressCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        KeyboardKey.onKeyPressd -= KeyPressCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                if(shouldReset)
                    Initialize();
                break;
            case GameState.LevelComplete:
                shouldReset = true;
                break;
            case GameState.GameOver:
                shouldReset = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        currentWordContainerIndex = 0;
        canAddLetter = true;

        DisableTryButton();
        for (int i = 0; i < wordContainers.Length; i++)
        {
            wordContainers[i].Initialize();
        }

        shouldReset = false;
    }

    private void KeyPressCallback(char letter)
    {
        if (!canAddLetter)
            return;

        wordContainers[currentWordContainerIndex].Add(letter);
        if (wordContainers[currentWordContainerIndex].IsComplete())
        {
            canAddLetter = false;
            EnableTryButton();
        }

        onLetterAdded?.Invoke();

    }

    public void CheckWord()
    {
        string wordToCheck = wordContainers[currentWordContainerIndex].GetWord();
        string secretWord = WordManager.instance.GetSecretWord();

        wordContainers[currentWordContainerIndex].Colorize(secretWord);
        keyboardColorizer.Colorize(secretWord, wordToCheck);
        if (wordToCheck == secretWord)
        {
            SetLevelComplete();
        }
        else
        {
            //Debug.Log("Wrong Word!");
            currentWordContainerIndex++;
            DisableTryButton();

            if (currentWordContainerIndex >= wordContainers.Length)
            {
                //Debug.Log("Game Over");
                DataManager.instance.ResetScore();
                GameManager.instance.SetGameState(GameState.GameOver);
            }
            else
            {
                canAddLetter = true;
            }
        }
    }

    private void SetLevelComplete()
    {
        UpdateData();
        GameManager.instance.SetGameState(GameState.LevelComplete);
    }

    private void UpdateData()
    {
        int scoreToAdd = 6 - currentWordContainerIndex;

        DataManager.instance.IncreaseScore(scoreToAdd);
        DataManager.instance.AddCoins(scoreToAdd * 3);
    }

    public void BackspacePressedCallBack()
    {
        //Debug.Log("back");

        if (!GameManager.instance.IsGameState())
            return;

        bool removeLetter = wordContainers[currentWordContainerIndex].RemoveLetter();

        if (removeLetter)
            DisableTryButton();

        canAddLetter = true;

        onLetterRemoved?.Invoke();
    }

    private void EnableTryButton()
    {
        tryButton.interactable = true;
    }

    private void DisableTryButton()
    {
        tryButton.interactable = false;
    }

    public WordContainer GetCurrentWordContainer()
    {
        return wordContainers[currentWordContainerIndex];
    }
}
