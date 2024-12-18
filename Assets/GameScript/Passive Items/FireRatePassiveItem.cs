using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentFireRate *= 1 + passiveItemData.Modifier / 100f;
    }
}
