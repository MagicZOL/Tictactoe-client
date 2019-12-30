using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkerType { None, Circle, Cross }

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer markerSpriteRenderer;

    [SerializeField] Sprite circleMarkerSprite;
    [SerializeField] Sprite crossMarkerSprite;

    private BoxCollider2D cachedBoxColider2D;
    public BoxCollider2D CachedBoxColider2D
    {
        get
        {
            if(!cachedBoxColider2D)
            {
                cachedBoxColider2D = GetComponent<BoxCollider2D>();
            }
            return cachedBoxColider2D;
        }
    }

    private SpriteRenderer cachedSpriteRenderer;
    public SpriteRenderer CachedSpriteRenderer
    {
        get
        {
            if(!cachedSpriteRenderer)
            {
                cachedSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            return cachedSpriteRenderer;
        }
    }

    MarkerType markerType;

    public MarkerType MarkerType
    {
        get
        {
            return markerType;
        }
        set
        {
            if(markerType != MarkerType.None)
            {
                //o,x 가 할당된 상태라면
                return;
            }

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

    public void SetActiveTouch(bool active)
    {
        CachedBoxColider2D.enabled = active;
        CachedSpriteRenderer.color = (active == true) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }
}
