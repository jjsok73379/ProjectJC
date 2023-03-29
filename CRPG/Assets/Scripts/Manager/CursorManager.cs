using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Inst;

    int currentFrame;
    float frameTimer;
    int frameCount;

    [SerializeField]
    List<CursorAnimation> cursorAnimationList;

    CursorAnimation _cursorAnimation;

    public enum CursorType
    {
        Arrow, Grab, Sword, Magic
    }

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    [Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] CursorArray;
        public float frameRate;
        public Vector2 offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetActiveCursorType(CursorType.Arrow);
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer -= Time.deltaTime;
        if(frameTimer <= 0f)
        {
            frameTimer += _cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(_cursorAnimation.CursorArray[currentFrame], _cursorAnimation.offset, CursorMode.Auto);
        }
    }

    void SetActiveCursorAnimation(CursorAnimation cursorAnimation)
    {
        _cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate;
        frameCount = cursorAnimation.CursorArray.Length;
    }

    public void SetActiveCursorType(CursorType cursorType)
    {
        SetActiveCursorAnimation(GetCursorAnimation(cursorType));
    }

    CursorAnimation GetCursorAnimation(CursorType cursorType)
    {
        foreach(CursorAnimation cursorAnimation in cursorAnimationList)
        {
            if (cursorAnimation.cursorType == cursorType)
            {
                return cursorAnimation;
            }
        }
        // 커서타입을 찾을 수 없다!
        return null;
    }
}
