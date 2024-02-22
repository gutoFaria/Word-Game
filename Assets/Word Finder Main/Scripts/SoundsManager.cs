using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    [Header("Sounds")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource letterAddSound;
    [SerializeField] private AudioSource letterRemoveSound;
    [SerializeField] private AudioSource levelCompleteSound;
    [SerializeField] private AudioSource gameOverSound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InputManager.onLetterAdded += PlayerLetterAddedSound;
        InputManager.onLetterRemoved += PlayerLetterRemovedSound;

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.LevelComplete:
                levelCompleteSound.Play();
                break;
            case GameState.GameOver:
                gameOverSound.Play();
                break;
        }
    }

    private void PlayerLetterAddedSound()
    {
        letterAddSound.Play();
    }

    private void PlayerLetterRemovedSound()
    {
        letterRemoveSound.Play();
    }

    private void OnDestroy()
    {
        InputManager.onLetterAdded -= PlayerLetterAddedSound;
        InputManager.onLetterRemoved -= PlayerLetterRemovedSound;

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void EnableSounds()
    {
        buttonSound.volume = 0.5f;
        letterAddSound.volume = 0.5f;
        letterRemoveSound.volume = 0.5f;
        levelCompleteSound.volume = 0.5f;
        gameOverSound.volume = 0.5f;
    }

    public void DisableSounds()
    {
        buttonSound.volume = 0;
        letterAddSound.volume = 0;
        letterRemoveSound.volume = 0;
        levelCompleteSound.volume = 0;
        gameOverSound.volume = 0;
    }
}
