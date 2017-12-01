﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyStateMachines : MonoBehaviour {

    private static FuzzyStateMachines instance;
    public static FuzzyStateMachines Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<FuzzyStateMachines>();
            }
            return instance;
        }
    }

    float totalRange;
    int totalLightAttack, totalHeavyAttack, totalRangedAttack, 
        totalUpAttack, totalMiddleAttack, totalDownAttack;

    float totalInput, minRange, maxRange;

    float[] miuRange = new float[2]; //miuRangeMin, miuRangeMax
    float[] miuAttackType = new float[3]; //miuLightAttack, miuHeavyAttack, miuRangedAttack
    float[] miuAttackDirection = new float[3]; //miuUpDirection, miuMiddleDirection, miuBottomDirection

    float[] alphaRuleValue = new float[18];
    float totalRuleValue;

    float[] ruleValues = new float[18];

    [SerializeField]
    float[] finalRuleValues = new float[8];
    [SerializeField]
    MovementType.enemy[] movementType = new MovementType.enemy[8];

    [SerializeField]
    float choosenRuleIndex;
    public float ChoosenRuleIndex
    {
        get
        {
            return choosenRuleIndex;
        }
    }

    [SerializeField]
    FuSMsLogHistory fusmsLogHistory;

	void Start () {
		fusmsLogHistory = FindObjectOfType<FuSMsLogHistory>();
    }

    public void initiateFuSMs()
    {
        for (int i = 0; i < finalRuleValues.Length; i++)
        {
            finalRuleValues[i] = 0;
            movementType[i] = (MovementType.enemy)i;
        }
    }

    public void runFuSMs()
    {
        totalInput = PlayerInputManager.instance.getTotalInput().getTotalMovement();
        minRange = Player.Instance.Target.GetComponent<Enemy>().NearRange;
        maxRange = Player.Instance.Target.GetComponent<Enemy>().FarRange;

        totalRange = Mathf.Abs(Player.Instance.gameObject.transform.position.x - Player.Instance.Target.transform.position.x);
        totalLightAttack = PlayerInputManager.instance.getCountMovement(0).getTotalMovement() + 1;
        totalHeavyAttack = PlayerInputManager.instance.getCountMovement(1).getTotalMovement() + 1;
        totalRangedAttack = PlayerInputManager.instance.getCountMovement(2).getTotalMovement() + 1;
        totalUpAttack = PlayerInputManager.instance.getCountMovement(3).getTotalMovement() + 1;
        totalMiddleAttack = PlayerInputManager.instance.getCountMovement(4).getTotalMovement() + 1;
        totalDownAttack = PlayerInputManager.instance.getCountMovement(5).getTotalMovement() + 1;

        miuRange[0] = calculateMiuValue(totalRange, false, true);
        miuRange[1] = calculateMiuValue(totalRange, true, true);
        miuAttackType[0] = calculateMiuValue(totalLightAttack, true, false);
        miuAttackType[1] = calculateMiuValue(totalHeavyAttack, true, false);
        miuAttackType[2] = calculateMiuValue(totalRangedAttack, true, false);
        miuAttackDirection[0] = calculateMiuValue(totalUpAttack, true, false);
        miuAttackDirection[1] = calculateMiuValue(totalMiddleAttack, true, false);
        miuAttackDirection[2] = calculateMiuValue(totalDownAttack, true, false);

        totalRuleValue = 0;
        for (int i = 0; i < miuRange.Length; i++)
        {
            for (int j = 0; j < miuAttackDirection.Length; j++)
            {
                for (int k = 0; k < miuAttackType.Length; k++)
                {
                    alphaRuleValue[i * 9 + j * 3 + k] = calculateRuleAND(new float[] { miuRange[i], miuAttackDirection[j], miuAttackType[k] });
                    totalRuleValue += alphaRuleValue[i * 9 + j * 3 + k];
                }
            }
        }

        for (int i = 0; i < ruleValues.Length; i++)
        {
            ruleValues[i] = alphaRuleValue[i] / totalRuleValue;

            /*for (int j = 0; j < movementType.Length; j++)
            {
                if (Rule.Instance.ruleAfterIdle[i] == movementType[j])
                {
                    finalRuleValues[j] += ruleValues[i];
                    continue;
                }
            }*/

            //Debug.Log(Player.Instance.Target.GetComponent<Enemy>().CurrentState.getStateName());
            switch (Player.Instance.Target.GetComponent<Enemy>().CurrentState.getStateName())
            {
                //masih bug sedikit disini
                case "idle":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterIdle[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "walk":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterWalk[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "walkBackward":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterWalkBackward[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "lightAttack":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterLightAttack[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;

                case "heavyAttack":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterHeavyAttack[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "rangedAttack":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterRangedAttack[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "jump":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterJump[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
                case "crouch":
                    for (int j = 0; j < movementType.Length; j++)
                    {
                        if (Rule.Instance.ruleAfterCrouch[i] == movementType[j])
                        {
                            finalRuleValues[j] += ruleValues[i];
                            continue;
                        }
                    }
                    break;
            }
        }

        float randomValue = (float)Random.Range(1, 100);
        float tempTotal = 0;
        for (int i = 0; i < finalRuleValues.Length; i++)
        {
            if (tempTotal < randomValue)
            {
                tempTotal += (finalRuleValues[i] * 100);
            }

			if(tempTotal >= randomValue)
            {
                choosenRuleIndex = i;
                break;
            }
        }
		//----------------------------------------------------------------

        fusmsLogHistory.addTransition(Player.Instance.Target.GetComponent<Enemy>().CurrentState.getStateName(),
            totalRange, totalLightAttack - 1, totalHeavyAttack - 1, totalRangedAttack - 1, 
            totalUpAttack - 1, totalMiddleAttack - 1, totalDownAttack - 1,
            finalRuleValues, ((MovementType.enemy)ChoosenRuleIndex).ToString());
    }

    float calculateMiuValue(float value, bool isUpLinier, bool isRangeType)
    {
        float miuValue, maxValue, minValue;
        maxValue = totalInput;
        minValue = 0;

        if (isRangeType)
        {
            maxValue = maxRange;
            minValue = minRange;
        }

        if (isUpLinier)
        {
            if (value <= minValue)
            {
                miuValue = 0;
            }
            else if (value > minValue && value < maxValue)
            {
                miuValue = (value - minValue) / (maxValue - minValue);
            }
            else
            {
                miuValue = 1;
            }
        }
        else
        {
            if (value <= minValue)
            {

                miuValue = 1;
            }
            else if (value > minValue && value < maxValue)
            {
                miuValue = (maxValue - value) / (maxValue - minValue);
            }
            else //(value >= maxValue)
            {
                miuValue = 0;
            }
        }

        return miuValue;
    }

    float calculateRuleAND(float[] values)
    {
        float result = 0;

        if (values.Length > 0)
        {
            result = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < result)
                {
                    result = values[i];
                }
            }
        }

        return result;
    }

    float calculateRuleOR(float[] values)
    {
        float result = 0;

        if (values.Length > 0)
        {
            result = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > result)
                {
                    result = values[i];
                }
            }
        }

        return result;
    }

    float[] getRuleValues()
    {
        return ruleValues;
    }
}