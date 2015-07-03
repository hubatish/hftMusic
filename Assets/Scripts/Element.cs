using UnityEngine;
using System.Collections;

/// <summary>
/// Just store the element I am
/// </summary>
public class Element : MonoBehaviour {
    public int elementNumber
    {
        set
        {
            _elementNumber = value;
        }
        get
        {
            return _elementNumber;
        }
    }
    [SerializeField]
    private int _elementNumber;

    public bool isSame(Element other)
    {
        return other.elementNumber == elementNumber;
    }

    protected SpriteRenderer renderer
    {
        get
        {
            if (_renderer == null)
            {
                _renderer = gameObject.GetComponent<SpriteRenderer>();
            }
            return _renderer;
        }
    }
    private SpriteRenderer _renderer;

    public void SetTransparent()
    {
        renderer.color = new Color(renderer.color.a, renderer.color.g, renderer.color.b, 0.4f);
    }

    public void SetVisible()
    {
        renderer.color = new Color(renderer.color.a, renderer.color.g, renderer.color.b, 1f);
    }
}
