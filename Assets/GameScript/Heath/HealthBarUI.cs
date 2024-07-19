using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Image _HealthBarForegroundImage;
    public void UpdateHealthBar(HealthController healthController)
    {
        _HealthBarForegroundImage.fillAmount = healthController.RemainingHealthPercentage;
    }


}
