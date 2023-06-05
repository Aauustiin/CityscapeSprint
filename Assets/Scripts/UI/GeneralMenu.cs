using UnityEngine;
using UnityEngine.UI;

public class GeneralMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelected;

    private void OnEnable()
    {
        firstSelected.Select();
    }
}
