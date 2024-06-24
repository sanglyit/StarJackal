using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathBarUI : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Image _heathBarForegroundImage;
    public void UpdateHeathBar(HeathController heathController)
    {
        _heathBarForegroundImage.fillAmount = heathController.RemainingHeathPercentage;
    }


}
