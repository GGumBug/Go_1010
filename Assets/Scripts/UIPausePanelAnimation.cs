using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePanelAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject  imageBackGroundOverlay;
    [SerializeField]
    private Animator    animator;

    public void OnApper()
    {
        imageBackGroundOverlay.SetActive(true);

        gameObject.SetActive(true);

        animator.SetTrigger("onAppear");
    }

    public void OnDisappear()
    {
        animator.SetTrigger("onDisappear");
    }

    public void EndOfDisappear()
    {
        imageBackGroundOverlay.SetActive(false);

        gameObject.SetActive(false);
    }
}
