﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuSMsLogHistory : MonoBehaviour {
    [System.Serializable]
	public class TransitionData
    {
        [System.Serializable]
        public struct TotalAttackType
        {
            public int light, heavy, ranged;
        }

        [System.Serializable]
		public struct TotalAttackDirection
        {
            public int up, middle, down;
        }

        [System.Serializable]
		public struct NextStatePersentage
        {
            public float idle, walk, walkBackward, lightAttack, heavyAttack, rangedAttack, jump, crouch;
        }

        public int transitionNumber;
		public string previousState;
        //MovementType.enemy previousState; sementara string
		public float range;
        [SerializeField]
        public TotalAttackType totalAttackType;
        [SerializeField]
		public TotalAttackDirection totalAttackDirection;
        [SerializeField]
		public NextStatePersentage nextStatePersentage;
		public string choosenStage;
        //MovementType.enemy choosenStage; sementara string

        public TransitionData(int transitionNumber, string previousState, float range,
        int totalLightAttack, int totalHeavyAttack, int totalRangedAttack, int totalUpAttack, int totalMiddleAttack, int totalDownAttack,
        float[] nextStateValuePersentage, string choosenStage)
        {
            this.transitionNumber = transitionNumber;
            this.previousState = previousState;
            this.range = range;

            this.totalAttackType.light = totalLightAttack;
            this.totalAttackType.heavy = totalHeavyAttack;
            this.totalAttackType.ranged = totalRangedAttack;
            this.totalAttackDirection.up = totalUpAttack;
            this.totalAttackDirection.middle = totalMiddleAttack;
            this.totalAttackDirection.down = totalDownAttack;

            this.nextStatePersentage.idle = nextStateValuePersentage[0];
            this.nextStatePersentage.walk = nextStateValuePersentage[1];
            this.nextStatePersentage.walkBackward = nextStateValuePersentage[2];
            this.nextStatePersentage.lightAttack = nextStateValuePersentage[3];
            this.nextStatePersentage.heavyAttack = nextStateValuePersentage[4];
            this.nextStatePersentage.rangedAttack = nextStateValuePersentage[5];
            this.nextStatePersentage.jump = nextStateValuePersentage[6];
            this.nextStatePersentage.crouch = nextStateValuePersentage[7];

            this.choosenStage = choosenStage;
        }
    }

    [SerializeField]
    List<TransitionData> transitionData;
    int totalTransition;

	private static FuSMsLogHistory instance;
	public static FuSMsLogHistory Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<FuSMsLogHistory> ();
			}
			return instance;
		}
	}

	void Start () {
        transitionData.Clear();
        totalTransition = 0;
    }

    void Update()
    {
        totalTransition = transitionData.Count;
    }

    public void addTransition(string previousState, float range,
        int totalLightAttack, int totalHeavyAttack, int totalRangedAttack, int totalUpAttack, int totalMiddleAttack, int totalDownAttack,
        float[] nextStatePersentage, string choosenStage)
    {
		transitionData.Add(new TransitionData(transitionData.Count, previousState, range, totalLightAttack, totalHeavyAttack, totalRangedAttack,
            totalUpAttack, totalMiddleAttack, totalDownAttack, nextStatePersentage, choosenStage));
    }

	public List<TransitionData> getTransitionData(){
		return transitionData;
	}
}
