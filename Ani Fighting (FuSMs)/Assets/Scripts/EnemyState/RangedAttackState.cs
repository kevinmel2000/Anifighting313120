﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : IEnemyState
{
    private Enemy enemy;

    private float castMagicTimer;
    private float castMagicCoolDown = 2;
    private bool canCastMagic = true;

    public string getStateName()
    {
        return "rangedAttack";
    }

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        CastingMagic();
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void CastingMagic()
    {
        if (canCastMagic)
        {
            canCastMagic = false;
            enemy.CharaAnimator.SetTrigger("rangedAttack");
        }
        castMagicTimer += Time.deltaTime;

        if (castMagicTimer >= castMagicCoolDown)
        {
            canCastMagic = true;
            castMagicTimer = 0;
            //enemy.ChangeState(new IdleState());
            FuzzyStateMachines.Instance.initiateFuSMs();
            FuzzyStateMachines.Instance.runFuSMs();

            switch (((MovementType.enemy)FuzzyStateMachines.Instance.ChoosenRuleIndex).ToString())
            {
                //masih bug sedikit disini
                case "idle":
                    enemy.ChangeState(new IdleState());
                    break;
                case "walk":
                    enemy.ChangeState(new WalkState());
                    break;
                case "walkBackward":
                    enemy.ChangeState(new WalkBackwardState());
                    break;
                case "lightAttack":
                    enemy.ChangeState(new LightAttackState());
                    break;
                case "heavyAttack":
                    enemy.ChangeState(new HeavyAttackState());
                    break;
                case "rangedAttack":
                    enemy.ChangeState(new RangedAttackState());
                    break;
                case "jump":
                    enemy.ChangeState(new JumpState());
                    break;
                case "crouch":
                    enemy.ChangeState(new CrouchState());
                    break;
            }
        }
    }
}