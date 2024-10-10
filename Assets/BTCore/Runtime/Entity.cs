using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity
{
    public int ID { get; private set; }
    public Vector3 Position;
    public bool Alive = true;


    public Entity() { EntityManager.AllEntity.Add(this); this.ID = EntityManager.AllEntity.Max((a) => a.ID) + 1; }
}
public class EntityManager
{
    public static List<Entity> AllEntity = new();

    public static Entity Get(int id)
    {
        return AllEntity.Find(a => a.ID == id);
    }
}