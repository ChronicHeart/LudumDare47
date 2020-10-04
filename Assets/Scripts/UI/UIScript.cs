using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    Slider healthBar;               // The health bar of the player
    [SerializeField]
    Image weaponImage;              // The weapon displayed in the UI

    PlayerController playerController;        // A reference to the player's controller
    PlayerHealth playerHealth;      // A reference to the player's health

    private void Awake()
    {
        // Set references to the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerController = player.GetComponent<PlayerController>();

        // Subscribed to the events for changing states and changing HP
        playerHealth.PlayerHealthChanged += UpdateHealth;
        playerController.PlayerStateChanged += UpdateWeaponDisplay;
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

    private void UpdateHealth()
    {
        healthBar.value = playerHealth.CurrentHP;
    }

    private void UpdateWeaponDisplay()
    {
        Debug.Log("The player has swapped weapons");
    }

    private void OnDestroy()
    {
        // Unsubscribe from any events
        playerHealth.PlayerHealthChanged -= UpdateHealth;
        playerController.PlayerStateChanged -= UpdateWeaponDisplay;
    }
}

