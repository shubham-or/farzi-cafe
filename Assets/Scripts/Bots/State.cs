using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        //IDLE, PATROL,
        PERSUE, REACHED_ING, INGS_FOUNDS, DISH_RCVD
    };


    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected Animator anim;
    protected AI player;
    protected State nextState;
    protected NavMeshAgent agent;

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float destOffset = 2.0f;

    public State(NavMeshAgent _agent, Animator _anim, AI _player)
    {
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;

    }

    public bool HasReachedDest_Ing(Vector3 destination)
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = destination.y = 0;
        Vector3 direction = destination - playerPos;

        if (direction.magnitude < destOffset)
        {
            return true;
        }
        return false;
    }

    public bool GotCurrentIng()
    {
        player.visited_Ing++;
        if (player.currentClue._clue.id == player.chasingClue._clue.id)
        {
            Debug.Log("Its a right ingredient");
            return true;
        }

        //Debug.Log("Its a wrong one");

        return false;

    }
}


public class Pursue : State
{
    public Pursue(NavMeshAgent _agent, Animator _anim, AI _player)
        : base(_agent, _anim, _player)
    {
        name = STATE.PERSUE;
        agent = _agent;
        //agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Prsue Update----");
        Vector3 destination = player.chasingClue.transform.position;
        destination.y = agent.transform.position.y;
        agent.SetDestination(destination);

        if (agent.hasPath)
        {
            if (HasReachedDest_Ing(player.chasingClue.transform.position))
            {
                //Debug.Log("Reached Destination Update----");

                nextState = new REACHED_ING(agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class REACHED_ING : State
{
    float rotationSpeed = 2.0f;

    public REACHED_ING(NavMeshAgent _agent, Animator _anim, AI _player)
        : base(_agent, _anim, _player)
    {
        name = STATE.REACHED_ING;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isShooting");

        agent.isStopped = true;

        if (GotCurrentIng())
        {
            player.ing_Found++;

            if (player.ing_Found == player.clueOrder.Count)
            {
                nextState = new INGS_FOUND(agent, anim, player);
                stage = EVENT.EXIT;
            }
            else
            {
                player.GetNextIngredient();
                nextState = new Pursue(agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            player.GetNextPoint();
            nextState = new Pursue(agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        //anim.ResetTrigger("isShooting");
        base.Exit();
    }

}

public class INGS_FOUND : State
{
    GameObject safeLocation;
    public INGS_FOUND(NavMeshAgent _agent, Animator _anim, AI _player)
        : base(_agent, _anim, _player)
    {
        name = STATE.DISH_RCVD;
        //safeLocation = GameObject.FindGameObjectWithTag("safe");
    }

    public override void Enter()
    {
        //anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("Prsue Update----");
        Vector3 destination = player.currentClue.transform.position; //will be replaced by dish position
        destination.y = agent.transform.position.y;
        agent.SetDestination(destination);

        if (agent.hasPath)
        {
            if (HasReachedDest_Ing(player.currentClue.transform.position)) //will be replaced by dish position
            {
                //Debug.Log("Reached Destination Update----");

                nextState = new DISH_RCVD(agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class DISH_RCVD : State
{
    GameObject safeLocation;
    public DISH_RCVD(NavMeshAgent _agent, Animator _anim, AI _player)
        : base(_agent, _anim, _player)
    {
        name = STATE.DISH_RCVD;
        //safeLocation = GameObject.FindGameObjectWithTag("safe");
    }

    public override void Enter()
    {
        //anim.SetTrigger("isRunning");
        Debug.Log("Dish Claimed");
        player.DestroyBot();
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        //anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
