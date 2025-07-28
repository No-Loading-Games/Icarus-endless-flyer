using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Head Wear", menuName = "Head Wear")]
public class HeadWearSO : ScriptableObject
{
    public Sprite lockedIcon;
    public Sprite unlockedIcon;
    public string headWearName;
    [TextArea()]
    public string desription;

    public bool isPurchasable;
    public bool isUnlocked;
    public int price;

    public string headWearNamePP;
    public string isUnlockedPP;

    public bool isEquipped;

    public AnimationClip flyingAnimation;
    public AnimatorOverrideController headWearAnimator;

    public List<AnimationClip> animationClips;
}
