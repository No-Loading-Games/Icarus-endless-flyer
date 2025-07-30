using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SkinSO : ScriptableObject
{
    public Sprite lockedIcon;
    public Sprite unlockedIcon;
    public string skinName;
    [TextArea()]
    public string desription;

    public bool isPurchasable;
    public bool isUnlocked;

    public int price;
    public int scoreToUnlock; //best saved to an array when the game starts

    public string skinNamePP;
    public string isUnlockedPP;

    public bool isEquipped;

    public AnimationClip previewAnimation;
    public AnimatorOverrideController inGameAnimator;

    public List<AnimationClip> animationClips;
    public List<AnimationClip> frontArmAnimationClips;

}
