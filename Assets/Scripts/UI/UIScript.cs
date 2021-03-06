﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    Slider healthBar;               // The health bar of the player
    [SerializeField]
    TextMeshProUGUI weaponText;         // The weapon information displayed to the player
    [SerializeField]
    GameObject playerUI;                // Holds all information presented during normal play
    [SerializeField]
    GameObject pausePanel;              // Holds the pause menu

    PlayerController playerController;        // A reference to the player's controller
    PlayerHealth playerHealth;      // A reference to the player's health

    bool isPaused;

    private void Awake()
    {
        // Set references to the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerController = player.GetComponent<PlayerController>();

        // Subscribed to the events for changing states and changing HP
        playerHealth.PlayerHealthChanged += UpdateHealth;
        playerController.PlayerStateChanged += UpdateWeaponDisplay;

        isPaused = false;
    }

    private void Start()
    {
        // Set the maximum value of the health bar to the player's max hp
        healthBar.maxValue = playerHealth.MaxHP;

        // Set the initial value of health based on the player's current health
        UpdateHealth();

        // Set the initial weapon display based on the current weapon
        UpdateWeaponDisplay();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            playerUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            playerUI.SetActive(true);
        }
    }

    private void UpdateHealth()
    {
        healthBar.value = playerHealth.CurrentHP;
    }

    private void UpdateWeaponDisplay()
    {
        //Debug.Log("The player has swapped weapons");
        // Updated the weapon information based on the current state of the player
        switch (playerController.CurrentStateIndex)
        {
            case 0:
                weaponText.text = "Weapon: Guitar";
                break;

            case 1:
                weaponText.text = "Weapon: Record";
                break;

            case 2:
                weaponText.text = "Weapon: Trumpet";
                break;

            default:
                weaponText.text = "Weapon: Null";
                break;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from any events
        playerHealth.PlayerHealthChanged -= UpdateHealth;
        playerController.PlayerStateChanged -= UpdateWeaponDisplay;
    }

    //-------------- To be called by other scripts

    public void LoadSceneById(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}

