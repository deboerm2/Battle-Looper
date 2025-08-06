using UnityEngine;
[CreateAssetMenu]
public class Effects : ScriptableObject
{
    public MindEffects mindEffect;
    public BodyEffects bodyEffect;
    public EnvironmentEffects environmentEffect;

    public float duration;
    [Tooltip("Multiplier by which duration scales with loop size. gets applied once for every 10 points on loop")]
    public float durationMult = 0.8f;
}

public enum MindEffects
{
    none, Charm, Frenzy, MoraleBoost
}

public enum BodyEffects
{
    none, Fraility, Medicine
}

public enum EnvironmentEffects
{
    none, DOT, Mud, Fog
}