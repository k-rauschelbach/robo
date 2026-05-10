namespace Robo;

using Godot;

public enum PartType
{
    Head,
    Torso,
    Arm,
}

public partial class Part : Node3D
{
    [Export] public PartType PartType { get; set; } = PartType.Head;
    [Export] public float MaxHealth { get; set; } = 100.0f;

    private float _currentHealth;
    
    public override void _Ready()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
    }


}