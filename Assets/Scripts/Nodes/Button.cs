using CollisionsManagement;
using GameTriggers;
using Godot;
using System;

public partial class Button : Interactible
{
    [Export]
    public NodePath CollisionShape{get;set;}
    private Area2D Collision {get;set;}
    
    [Export]
	public string CollisionGroupName{get;set;} ="Door001";

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _EnterTree()
    {
        
        this.Collision = this.GetNode<Area2D>(CollisionShape);
        Collisionhandler.Instance.AddCollisionArea(Collision,CollisionGroupName);
        GD.Print($"Door connected to {CollisionGroupName}");
    }
}