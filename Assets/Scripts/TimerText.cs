using UnityEngine;
using TMPro;

public class TimerText
{
    private TMP_Text textElement;

    public TimerText(TMP_Text text)
    {
        textElement = text;
    }

    public void ConfigureText(int fontSize, TextAnchor alignment)
    {
        textElement.fontSize = fontSize;
        textElement.alignment = ConvertAlignment(alignment);
    }

    public void SetText(string text)
    {
        textElement.text = text;
    }

    public void SetActive(bool isActive)
    {
        textElement.gameObject.SetActive(isActive);
    }

    private TextAlignmentOptions ConvertAlignment(TextAnchor alignment)
    {
        return alignment switch
        {
            TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
            TextAnchor.UpperCenter => TextAlignmentOptions.Top,
            TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
            TextAnchor.MiddleLeft => TextAlignmentOptions.MidlineLeft,
            TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
            TextAnchor.MiddleRight => TextAlignmentOptions.MidlineRight,
            TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
            TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
            TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
            _ => TextAlignmentOptions.Center,
        };
    }
}