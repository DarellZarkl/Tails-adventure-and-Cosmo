using CollisionsManagement;
using Godot;
using System;

public partial class DeadZone : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
    public override void _EnterTree()
    {
        base._EnterTree();
		Collisionhandler.Instance.AddCollisionArea(this,CollisionGroups.DeadZone);
    }
    public override void _ExitTree()
    {
        base._ExitTree();
		Collisionhandler.Instance.RemoveCollisionArea(this, CollisionGroups.DeadZone);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}
}
