using UnityEngine;

public abstract class MobState
{
    protected Mob mob;

    public MobState(Mob mob)
    {
        this.mob = mob;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
