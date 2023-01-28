using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCursor : MonoBehaviour
{
    public Texture2D BasicCursor;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(BasicCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
