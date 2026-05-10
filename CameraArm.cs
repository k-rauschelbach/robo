using Godot;
using System;

namespace Robo;
public partial class CameraArm : Node3D
{
	[Export] public float MouseSensitivity = 0.002f;
	
	private bool _combatState = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (_combatState)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Middle))
			{
				if (@event  is InputEventMouseMotion mouseMotion)
				{
					RotateY(-mouseMotion.Relative.X * MouseSensitivity);

					var rotation = Rotation;
					rotation.X += -mouseMotion.Relative.Y * MouseSensitivity;
					rotation.X = Mathf.Clamp(rotation.X, -0.8f, 0.4f);
					Rotation = rotation;
				}
			}
		}
	}

	public void ExploreCamera()
	{
		_combatState = false;
	}

	public void CombatCamera()
	{
		_combatState = true;
	}
}
