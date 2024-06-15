using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem; 

public class UIManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public Button restartButton;
    public static UIManager Instance { get; private set; }
    // Event to notify other scripts when the game over menu is activated
    public static event Action OnGameOverMenuActivated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= EnableGameOverMenu;
    }

    private void Update()
    {
        if (gameOverMenu.activeSelf && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            RestartLevel();
        }
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        // Optionally, disable player input here if needed
        // ThirdPersonController.Instance.DisablePlayerMovement();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
