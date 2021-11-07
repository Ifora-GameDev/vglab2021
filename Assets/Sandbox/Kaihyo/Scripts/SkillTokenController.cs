using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum SkillTokenState
{
    Unavailable,
    Available,
    Hacked,
    Broken,
}

public class SkillTokenController : MonoBehaviour
{
    [SerializeField] private SkillColorPalette _colorPalette;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeState(SkillTokenState state)
    {
        switch(state)
        {
            case SkillTokenState.Available:
                {
                    _image.color = _colorPalette.availableColor;
                    break;
                }
            case SkillTokenState.Unavailable:
                {
                    _image.color = _colorPalette.unavalaibleColor;
                    break;
                }
            case SkillTokenState.Broken:
                {
                    _image.color = _colorPalette.brokenColor;
                    break;
                }
            case SkillTokenState.Hacked:
                {
                    _image.color = _colorPalette.hackedColor;
                    break;
                }
            default:
                {
                    _image.color = _colorPalette.availableColor;
                    break;
                }
        }
    }
}
