using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkerType { None, Circle, Cross }

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer markerSpriteRenderer;

    [SerializeField] Sprite circleMarkerSprite;
    [SerializeField] Sprite crossMarkerSprite;

    public int index; //셀이 몇번인지

    MarkerType markerType;

    public MarkerType MarkerType
    {
        get
        {
            return markerType;
        }
        set
        {
            switch (value)
            {
                case MarkerType.None:
                    markerSpriteRenderer.sprite = null;
                    break;
                case MarkerType.Circle:
                    markerSpriteRenderer.sprite = circleMarkerSprite;
                    break;
                case MarkerType.Cross:
                    markerSpriteRenderer.sprite = crossMarkerSprite;
                    break;
            }
            markerType = value;
        }
    }
}
