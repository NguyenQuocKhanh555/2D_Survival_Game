using UnityEngine;

public abstract class AnimalState
{
    protected Animal animal;

    public AnimalState(Animal animal)
    {
        this.animal = animal;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
