using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContentFitter : MonoBehaviour
{
    [SerializeField]
    private RectTransform _childRectTransform;

    private RectTransform _selfRectTransform;
    private RectTransform _newSelfRectTransform;

    public Vector2 sizeDelta;
    public Vector2 newSizeDelta;

    [SerializeField]
    private float _padding = 12;

    public bool _noChanges;

    private void Start()
    {
        _newSelfRectTransform = GetComponent<RectTransform>();
        _selfRectTransform = _newSelfRectTransform;

        _noChanges = true;

        sizeDelta = _selfRectTransform.sizeDelta;
        newSizeDelta = _newSelfRectTransform.sizeDelta;

        StartCoroutine(UpdateCanvasSize());
    }

    private void UpdateParentCanvasSize()
    {
        sizeDelta = new Vector2(_childRectTransform.sizeDelta.x, _childRectTransform.rect.height);
        _selfRectTransform.sizeDelta = new Vector2(_selfRectTransform.sizeDelta.x, _childRectTransform.rect.height + _padding);
    }

    private IEnumerator UpdateCanvasSize()
    {
        while(_noChanges)
        {
            _newSelfRectTransform.sizeDelta = new Vector2(_newSelfRectTransform.sizeDelta.x, _childRectTransform.rect.height + _padding);

            newSizeDelta = _newSelfRectTransform.sizeDelta;

            Debug.Log("UPDATING CANVAS SIZE from " + sizeDelta.y + " to " + newSizeDelta.y);
            if (sizeDelta.y != newSizeDelta.y && newSizeDelta.y > _padding)
            {
                _noChanges = false;

                Debug.Log("STOPPING UPDATING CANVAS SIZE");
            }

            yield return null;
        }

        StopAllCoroutines();

    }

}
