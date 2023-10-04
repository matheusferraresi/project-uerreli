using System;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image cursorImage;

    private void Awake()
    {
        cursorImage = GetComponent<Image>();
    }

    void Start()
    {
        Cursor.visible = false;

        if (Application.isPlaying)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        cursorImage.transform.position = cursorPos;
    }
}