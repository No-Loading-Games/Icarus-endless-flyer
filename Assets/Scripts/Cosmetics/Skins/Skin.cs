using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
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
        _animator.runtimeAnimatorController = SkinsManager.Instance.CurrentSkinAnimatorOverride;

        Debug.Log("SOMETHING 3 IS NOW " + _animator.runtimeAnimatorController);
    }
}
