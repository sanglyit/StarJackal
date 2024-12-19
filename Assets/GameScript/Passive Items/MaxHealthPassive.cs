using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPassive : PassiveItem
{
    protected override void ApplyModifier()
    {
        // Increase the runtime max health
        player.runtimeMaxHealth += passiveItemData.Modifier;

        // Clamp the current health to the new max health
        player.CurrentHealth = Mathf.Clamp(player.CurrentHealth, 0, player.runtimeMaxHealth);

        // Update the health bar to reflect changes
        player.UpdateHealthBar();
    }
}
