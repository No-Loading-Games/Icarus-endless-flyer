using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadWear : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimator();
    }

    // Update is called once per frame
    void Update()
    {

        //_spriteRenderer.enabled = HeadWearManager.Instance.HeadWearIsEquipped;
    }

    public void InitializeAnimator()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = HeadWearManager.Instance.CurrentHeadWearAnimatorOverride;

        Debug.Log("SOMETHING 3 IS NOW " + _animator.runtimeAnimatorController);
        //Debug.Log("ROYAL ENABLED? " + HeadWearManager.Instance.HeadWearIsEquipped);
        //_spriteRenderer.enabled = HeadWearManager.Instance.HeadWearIsEquipped;

        //Debug.Log("Sprite renderer enabled?" + HeadWearManager.Instance.HeadWearIsEquipped);
    }

}
