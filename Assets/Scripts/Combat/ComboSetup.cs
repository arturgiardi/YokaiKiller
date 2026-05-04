using UnityEngine;
using System.Collections;

[System.Serializable]
public class Combo
{
    public string animationName = "Animation";
    public float dodgeTime = 0;
    public float minTime = 0;
    public float nextChain = 0.1f;
    public float maxTime = 0.2f;
}
[System.Serializable]
[CreateAssetMenu(menuName = "Combo Setup")]
public class ComboSetup : ScriptableObject
{
    public Combo[] sequence;
    public Combo[] airSequence;
    public Combo[] airMovingSequence;
    public Combo[] crouchingSequence;
}