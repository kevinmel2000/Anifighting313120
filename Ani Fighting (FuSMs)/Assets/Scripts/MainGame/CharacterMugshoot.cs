﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMugshoot : MonoBehaviour
{
    [SerializeField]
    GameObject charaIcon;
    [SerializeField]
	Sprite normalFace;
	[SerializeField]
	Sprite angryFace;
	[SerializeField]
	Sprite sickFace;

    public enum CharaExpression
    {
        normal,
        angry, 
        sick
    }

    CharaExpression charaExpression;

    void Start()
    {
		if (gameObject.tag == "Player") {
			charaIcon = GameObject.Find ("PlayerCharaIcon");
		}
		else if (gameObject.tag == "Enemy") {
			charaIcon = GameObject.Find ("EnemyCharaIcon");
		}
		charaExpression = CharaExpression.normal;
    }

    public void changeExpression(CharaExpression expression)
    {
        charaExpression = expression;
        switch (charaExpression)
        {
            case CharaExpression.angry:
                charaIcon.GetComponent<SpriteRenderer>().sprite = angryFace;
                break;
            case CharaExpression.sick:
                charaIcon.GetComponent<SpriteRenderer>().sprite = sickFace;
                break;
            default:
                charaIcon.GetComponent<SpriteRenderer>().sprite = normalFace;
                break;
        }
    }
}