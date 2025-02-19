﻿using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////           
            if (GetTemperature() >= overheatTemperature)//Не перегрелось ли оружие?
            { 
                return; //Выход из метода, если перегрелось
            }
            for (int i = 0; i <= _temperature; i++)//совершает выстрелов на 1 больше, чем "температура" т.е. даже при 0 один выстрел будет
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }
            IncreaseTemperature();//Увеличивает нагрев после выстрела
            ///////////////////////////////////////
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            ///////////////////////////////////////
            List<Vector2Int> result = GetReachableTargets(); // лист с координатами потенциальных целей
            float minDistance=float.MaxValue;                // поле для перебора и нахождения ближайшей цели
            Vector2Int firstTarget = Vector2Int.zero;        // координаты ближайшей цели

            foreach (Vector2Int target in result)            // перебор целей для нахождения ближайшей
            {
                float targetDistance = DistanceToOwnBase(target);

                if (targetDistance < minDistance)
                {
                    firstTarget = target;
                    minDistance = targetDistance;
                }
            }
            if (firstTarget != Vector2Int.zero)              // если цель была найдена, очищаем список и добавляем в него цель
            {
                result.Clear();
                result.Add(firstTarget);
                return result;
            }
            else { return result; }                          // дописал этот кусок т.к. была ошибка: "не все пути возвращают значение"

           ///////////////////////////////////////
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}