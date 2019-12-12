using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingupPopupPanel : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnClick()
    {
        gameObject.SetActive(true);
    }

    public void OffClick()
    {
        gameObject.SetActive(false);
    }
}
