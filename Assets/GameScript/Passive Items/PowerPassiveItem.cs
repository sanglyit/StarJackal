using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentStrength += passiveItemData.Multiplier;
    }
}
