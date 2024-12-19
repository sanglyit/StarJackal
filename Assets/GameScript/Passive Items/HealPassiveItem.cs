using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentHeal += passiveItemData.Modifier;
    }
}
