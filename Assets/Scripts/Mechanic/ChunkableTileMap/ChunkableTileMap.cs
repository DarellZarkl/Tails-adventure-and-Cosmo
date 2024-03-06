using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class ChunkableTileMap : Node2D
{
    public int PixelSize{get;set;} = 16;
    public int ChunkSize{get;set;} = 16;
    public int MaximumLoadDistance{get;set;} = 2;

    [Export]
    public NodePath CharacterPath{get;set;}
    public CharacterController currentCharacter{get;set;}

    [Export]
    public Godot.Collections.Array<PackedScene> AllTileMaps{get;set;}
    public List<MapChunk> CurrentLoaded{get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
    public override void _EnterTree()
    {
        GD.Print("getting controller");
        this.currentCharacter = GetNode(CharacterPath).GetChildren().Where(x=>x is CharacterController).FirstOrDefault() as CharacterController;
        this.HandleCurrentPosition();
    }
    public async void HandleCurrentPosition(){
        GD.Print("calculate currentChunkCoordinate");
        var currentPos = GetCurrentChunkCoordinate();
        var currently = CurrentLoaded.Where(x=>x.Coordinates.X==currentPos.X&&x.Coordinates.Y==currentPos.Y).FirstOrDefault();
        
        GD.Print($"current position : {currentPos.X}, {currentPos.Y}");
        if(currently==null){
            currently = AllTileMaps.Select(x=>x.Instantiate<MapChunk>()).Where(x=>x.Coordinates.X==currentPos.X && x.Coordinates.Y == currentPos.Y).FirstOrDefault();
            if(currently==null){
                return;
            }
        }
        ChunkBoundBox box = ChunkBoundBox.GetFromPosition(currentCharacter.GlobalPosition,ChunkSize*PixelSize);
        await DeepLoad(currently,MaximumLoadDistance,box);
        await DeepUnload(currently,MaximumLoadDistance,box);
    }
    public Task DeepLoad(MapChunk chunk, int depth, ChunkBoundBox playerCoords){
        GD.Print($"deepLoad");
        foreach(PackedScene packed in chunk.ToLoadOnEnter){
            if(!AllTileMaps.Contains(packed)){
                throw new Exception("MapChunk not in masterList !");
            }
            var instance = packed.Instantiate<MapChunk>();
            
            GD.Print($"Checking : {instance.Coordinates.X},{instance.Coordinates.Y}");
            if(!CurrentLoaded.Any(x=>x.Coordinates.X==instance.Coordinates.X && x.Coordinates.Y==instance.Coordinates.Y)){
                if(instance.Coordinates.X<=playerCoords.XPos 
                && instance.Coordinates.X>=playerCoords.XNeg
                && instance.Coordinates.Y<=playerCoords.YPos 
                && instance.Coordinates.Y>=playerCoords.YNeg){
                    
                    GD.Print($"Adding loaded instances...");
                    CurrentLoaded.Add(instance);
                    this.AddChild(instance);
                }
            }
        }
        return Task.CompletedTask;
    }
    public Task DeepUnload(MapChunk chunk, int depth, ChunkBoundBox playerCoords){
        List<MapChunk> todelete = new List<MapChunk>();
        foreach(MapChunk checkChunk in CurrentLoaded.ToList()){
            if(checkChunk.Coordinates.X>playerCoords.XPos 
            || checkChunk.Coordinates.X<playerCoords.XNeg
            || checkChunk.Coordinates.Y>playerCoords.YPos 
            || checkChunk.Coordinates.Y<playerCoords.YNeg){
                GD.Print($"Unload : {checkChunk.Coordinates.X},{checkChunk.Coordinates.Y}");
                todelete.Add(checkChunk);
                this.RemoveChild(checkChunk);
                CurrentLoaded.Remove(checkChunk);
            }
        }
        //foreach(MapChunk todel in todelete){
        //}
        return Task.CompletedTask;
    }
    public ChunkCoordinate GetCurrentChunkCoordinate(){
      
        int posX = (int)(Math.Floor(this.currentCharacter.GlobalPosition.X/ChunkSize*PixelSize));
          GD.Print($"XPos : {posX}");
        int posY = (int)(Math.Floor(this.currentCharacter.GlobalPosition.Y/ChunkSize*PixelSize));
          GD.Print($"YPos : {posY}");
        return new ChunkCoordinate(){ X = posX, Y = posY};
    }
	public void MoveToPosition(MapChunk map){
		map.GlobalPosition = new Vector2(map.Coordinates.X*ChunkSize/2*PixelSize,
		map.Coordinates.Y*ChunkSize/2*PixelSize);
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
