using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] Texture2D HandCursor;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(HandCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(Camera.main.GetComponent<MainCursor>().BasicCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
