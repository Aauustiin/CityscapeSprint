using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoEndMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelected;

    private void OnEnable()
    {
        firstSelected.Select();
    }
}
