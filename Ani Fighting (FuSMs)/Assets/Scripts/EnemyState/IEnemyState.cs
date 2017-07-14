﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Enter(Enemy enemy);
    void Execute();
    void Exit();
    void OnTriggerEnter(Collider2D other);
    string getStateName();
}