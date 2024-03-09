using System.Collections;
using System.Collections.Generic;
using Godot;

namespace CollisionsManagement{
public class Collisionhandler {
    private static Collisionhandler _instance;
    public static Collisionhandler Instance{
        get{
            if(_instance==null){
                _instance = new Collisionhandler();
            }
            return _instance;
        }
    }
    private Collisionhandler(){
        
    }
    Dictionary<string,CollisionHandlingItem> handlers = new Dictionary<string,CollisionHandlingItem>(); 

    public CollisionHandlingItem GetHandlerFromName(string name){
        CollisionHandlingItem handler=null;
        if(handlers.TryGetValue(name,out handler)){
            return handler;
        }
        else{
            return null;
        }
    }
    public CollisionHandlingItem GetHandlerFromGroup(CollisionGroups collisionGroup){
        return this.GetHandlerFromName(collisionGroup.ToString());
    }
    public CollisionHandlingItem AddCollisionArea(Area2D area2D,string groupname){
        CollisionHandlingItem handler = this.GetHandlerFromName(groupname);
        if(handler==null){
            handler = new CollisionHandlingItem();
            handlers.Add(groupname,handler);
        }
        handler.AddCollisionArea(area2D);
        return handler;
    }
    public CollisionHandlingItem AddCollisionBody(IOnCollisionEnter body,string groupname){
        CollisionHandlingItem handler = this.GetHandlerFromName(groupname);
        if(handler==null){
            handler = new CollisionHandlingItem();
            handlers.Add(groupname,handler);
        }
        handler.AddBody(body);
        return handler;
    }
    public CollisionHandlingItem RemoveCollisionArea(Area2D area,string groupname){
        CollisionHandlingItem handler = this.GetHandlerFromName(groupname);
        if(handler!=null){
        handler.RemoveArea(area);
        }
        return handler;
    }
    public CollisionHandlingItem RemoveCollisionBody(IOnCollisionEnter body,string groupname){
        CollisionHandlingItem handler = this.GetHandlerFromName(groupname);
        if(handler!=null){
            handler.RemoveBody(body);
        }
        return handler;
    }

    public CollisionHandlingItem AddCollisionArea(Area2D area2D,CollisionGroups groupname){
        return this.AddCollisionArea(area2D,groupname.ToString());
    }
    public CollisionHandlingItem AddCollisionBody(IOnCollisionEnter body,CollisionGroups groupname){
        return AddCollisionBody(body,groupname.ToString());
    }
    public CollisionHandlingItem RemoveCollisionArea(Area2D area,CollisionGroups groupname){
        return RemoveCollisionArea(area,groupname.ToString());
    }
    public CollisionHandlingItem RemoveCollisionBody(IOnCollisionEnter body,CollisionGroups groupname){
        return this.RemoveCollisionBody(body,groupname.ToString());
    }
}

public enum CollisionGroups{
    DeadZone
}

public class CollisionHandlingItem{
        List<Area2D> Zones = new List<Area2D>();
    List<IOnCollisionEnter> Triggers = new List<IOnCollisionEnter>();

        public void AddCollisionArea(Area2D area){
        Zones.Add(area);
        foreach(var item in Triggers){
            area.BodyEntered+= item.OnEnterArea;
            area.BodyExited+= item.OnExitArea;
            GD.Print($"Connected item to {area.Name}");
        }
    }
    public void AddBody(IOnCollisionEnter item){
        Triggers.Add(item);
        foreach(var zone in Zones){
            zone.BodyEntered+=item.OnEnterArea;
            zone.BodyExited+=item.OnExitArea;
            GD.Print($"Connected item to {zone.Name}");
        }
    }
    public Area2D RemoveArea(Area2D Zone){
        foreach(var item in Triggers){
            Zone.BodyEntered-=item.OnEnterArea;
            Zone.BodyExited-=item.OnExitArea;
        }
        Zones.Remove(Zone);
        return Zone;
    }
    public IOnCollisionEnter RemoveBody(IOnCollisionEnter item){
        foreach(var zone in Zones){
            zone.BodyEntered-=item.OnEnterArea;
            zone.BodyExited-=item.OnExitArea;
        }
        Triggers.Remove(item);
        return item;
    }
}

public interface IOnCollisionEnter{
    void OnEnterArea(Node2D area);
    void OnExitArea(Node2D area);
}
}