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
}
