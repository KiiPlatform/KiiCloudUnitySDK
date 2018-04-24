using UnityEngine;

public class ScalableGUI
{
    Vector2 _offset;

    public ScalableGUI(int w = 320, int h = 480, bool isPortrait = false)
    {
        float width = isPortrait ? h : w;
        float height = isPortrait ? w : h;

        float target_aspect = width / height;
        float window_aspect = (float)Screen.width / (float)Screen.height;
        float scale = window_aspect / target_aspect;

        Rect _rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        if (1.0f > scale)
        {
            _rect.x = 0;
            _rect.width = 1.0f;
            _rect.y = (1.0f - scale) / 2.0f;
            _rect.height = scale;

            Scale = (float)Screen.width / width;
        }
        else
        {
            scale = 1.0f / scale;
            _rect.x = (1.0f - scale) / 2.0f;
            _rect.width = scale;
            _rect.y = 0.0f;
            _rect.height = 1.0f;

            Scale = (float)Screen.height / height;
        }

        _offset.x = _rect.x * Screen.width;
        _offset.y = _rect.y * Screen.height;
    }

    public float Scale
    {
        get;
        set;
    }

    Rect ScalableRect(float x, float y, float width, float height)
    { 
        Rect rect = new Rect(_offset.x + (x * Scale), _offset.y + (y * Scale), width * Scale, height * Scale);
        return rect;
    }

    int ScalableFontSize(float fontSize = 14)
    {
        return (int)(fontSize * Scale);
    }

    public void Label(float x, float y, float width, float height, string text, float fontSize = 14)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = ScalableFontSize(fontSize);
        GUI.Label(ScalableRect(x, y, width, height), text, style);
    }

    public bool Button(float x, float y, float width, float height, string text, float fontSize = 14)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = ScalableFontSize(fontSize);
        return GUI.Button(ScalableRect(x, y, width, height), text, style);
    }

    public string TextField(float x, float y, float width, float height, string text, float fontSize = 14)
    {
        GUIStyle style = new GUIStyle(GUI.skin.textField);
        style.fontSize = ScalableFontSize(fontSize);
        return GUI.TextField(ScalableRect(x, y, width, height), text, style);
    }

    public string TextArea(float x, float y, float width, float height, string text, float fontSize = 14)
    {
        GUIStyle style = new GUIStyle(GUI.skin.textField);
        style.fontSize = ScalableFontSize(fontSize);
        return GUI.TextArea(ScalableRect(x, y, width, height), text, style);
    }

    public bool Toggle(float x, float y, float width, float height, bool value, string text, float fontSize = 14)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = ScalableFontSize(fontSize);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        int labelLeftMargin = (int)(2 * Scale);
        GUI.Label(ScalableRect(x + height + labelLeftMargin, y, width - height - labelLeftMargin, height), text, labelStyle);

        string check = "";
        if (value)
        {
            check = "x";
        }
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = (int)(height * Scale * 0.7);
        style.alignment = TextAnchor.MiddleCenter;
        int buttonMargin = (int)(2 * Scale);
        return GUI.Button(ScalableRect(x + buttonMargin, y + buttonMargin, height - (2 * buttonMargin), height - (2 * buttonMargin)), check, style);
    }

    public Vector2 ScrollView(float x, float y, float width, float height, Vector2 currentScrollPosition, string[] text, bool alwaysShowHorizontal = false, bool alwaysShowVertical = false, int fontSize = 14, int scrollbarSize = 15)
    {
        Vector2 scrollPosition = new Vector2(currentScrollPosition.x, currentScrollPosition.y);
        Rect rect = ScalableRect(x, y, width, height);
        GUI.Box(rect, "");
        GUILayout.BeginArea(rect);
        {
            // Set scrollbar base size
            GUI.skin.verticalScrollbar.fixedWidth = scrollbarSize * Scale;
            GUI.skin.verticalScrollbarThumb.fixedWidth = scrollbarSize * Scale;
            GUI.skin.horizontalScrollbar.fixedHeight = scrollbarSize * Scale;
            GUI.skin.horizontalScrollbarThumb.fixedHeight = scrollbarSize * Scale;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical);

            System.Array.Reverse(text);
            foreach (string t in text)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = ScalableFontSize();
                style.normal.textColor = Color.white;
                GUILayout.Label(t.Trim(), style);
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();

        return scrollPosition;
    }

}
