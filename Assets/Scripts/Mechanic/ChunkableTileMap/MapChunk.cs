using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapChunk : Node2D
{
	public List<TileMap> TileMaps {get{
		return this.GetChildren().Where(x=>x is TileMap).Select(x=>x as TileMap).ToList();
	}}
	[Export]
	public int X { get; set; }
	[Export]
	public int Y { get; set; }
	public ChunkCoordinate Coordinates{get;set;} = new ChunkCoordinate();
	[Export]
	public Godot.Collections.Array<PackedScene> ToLoadOnEnter {get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Coordinates=new ChunkCoordinate(){X=X, Y=Y};
	}
    public override void _EnterTree()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
public struct ChunkBoundBox{
	public static ChunkBoundBox GetFromPosition(Vector2 position,int maxdist){
		ChunkBoundBox box = new ChunkBoundBox();
		box.XPos = (int)(position.X+maxdist);
		box.XNeg = (int)(position.X+maxdist);
		box.YPos = (int)(position.Y+maxdist);
		box.YNeg = (int)(position.Y+maxdist);
		return box;
	}
	public int XPos{get;set;}
	public int YPos{get;set;}
	public int XNeg{get;set;}
	public int YNeg{get;set;}
}
public struct ChunkCoordinate{
	
	[Export]
	public int X {get;set;}
	[Export]
	public int Y {get;set;}
	[Export]
	public int Z {get;set;}
}
