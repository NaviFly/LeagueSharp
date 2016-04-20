﻿/*
 Copyright 2015 - 2015 SPrediction
 Prediction.PathTracker.cs is part of SPrediction
 
 SPrediction is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 SPrediction is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with SPrediction. If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace SPrediction
{
    /// <summary>
    /// Path Tracker class for SPrediction
    /// </summary>
    internal class PathTracker
    {
        /// <summary>
        /// structure for enemy data
        /// </summary>
        public struct EnemyData
        {
            public bool IsStopped;
            public List<Vector2> LastWaypoints;
            public int LastWaypointTick;
            public int StopTick;
            public float AvgTick;
            public float AvgPathLenght;
            public int Count;
            public int LastAATick;
            public int LastWindupTick;
            public bool IsWindupChecked;
            public int OrbwalkCount;
            public float AvgOrbwalkTime;
            public object m_lock;

            public EnemyData(List<Vector2> wp)
            {
                IsStopped = false;
                LastWaypoints = wp;
                LastWaypointTick = 0;
                StopTick = 0;
                AvgTick = 0;
                AvgPathLenght = 0;
                Count = 0;
                LastAATick = 0;
                LastWindupTick = 0;
                IsWindupChecked = false;
                OrbwalkCount = 0;
                AvgOrbwalkTime = 0;
                m_lock = new object();
            }
        }

        public static Dictionary<int, EnemyData> EnemyInfo = new Dictionary<int, EnemyData>();

        /// <summary>
        /// Initialize PathTracker services
        /// </summary>
        public static void Initialize()
        {
            foreach (Obj_AI_Hero enemy in HeroManager.Enemies)
                EnemyInfo.Add(enemy.NetworkId, new EnemyData(new List<Vector2>()));

            Obj_AI_Hero.OnNewPath += Obj_AI_Hero_OnNewPath;
            Obj_AI_Hero.OnDoCast += Obj_AI_Hero_OnDoCast;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        /// <summary>
        /// OnNewPath event for average reaction time calculations
        /// </summary>
        private static void Obj_AI_Hero_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (!sender.IsEnemy || !sender.IsChampion() || args.IsDash)
                return;

            EnemyData enemy = EnemyInfo[sender.NetworkId];

            lock (enemy.m_lock)
            {
                if (args.Path.Length < 2)
                {
                    if (!enemy.IsStopped)
                    {
                        enemy.StopTick = Environment.TickCount;
                        enemy.LastWaypointTick = Environment.TickCount;
                        enemy.IsStopped = true;
                        enemy.Count = 0;
                        enemy.AvgTick = 0;
                        enemy.AvgPathLenght = 0;
                    }
                }
                else
                {
                    List<Vector2> wp = args.Path.Select(p => p.To2D()).ToList();
                    if (!enemy.LastWaypoints.SequenceEqual(wp))
                    {
                        if (!enemy.IsStopped)
                        {
                            enemy.AvgTick = (enemy.Count * enemy.AvgTick + (Environment.TickCount - enemy.LastWaypointTick)) / ++enemy.Count;
                            enemy.AvgPathLenght = ((enemy.Count - 1) * enemy.AvgPathLenght + wp.PathLength()) / enemy.Count;
                        }
                        enemy.LastWaypointTick = Environment.TickCount;
                        enemy.IsStopped = false;
                        enemy.LastWaypoints = wp;

                        if (!enemy.IsWindupChecked)
                        {
                            if (Environment.TickCount - enemy.LastAATick < 300)
                                enemy.AvgOrbwalkTime = (enemy.AvgOrbwalkTime * enemy.OrbwalkCount + (enemy.LastWaypointTick - enemy.LastWindupTick)) / ++enemy.OrbwalkCount;
                            enemy.IsWindupChecked = true;
                        }
                    }
                }

                EnemyInfo[sender.NetworkId] = enemy;
            }
        }

        /// <summary>
        /// OnDoCast event for aa windup check
        /// </summary>
        private static void Obj_AI_Hero_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.IsAutoAttack() && sender.IsEnemy && sender.IsChampion())
            {
                EnemyData enemy = EnemyInfo[sender.NetworkId];
                lock (enemy.m_lock)
                {
                    enemy.LastWindupTick = Environment.TickCount;
                    enemy.IsWindupChecked = false;
                    EnemyInfo[sender.NetworkId] = enemy;
                }
            }
        }

        /// <summary>
        /// OnProcessSpellCast event for aa windup check
        /// </summary>
        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.IsAutoAttack() && sender.IsEnemy && sender.IsChampion())
            {
                EnemyData enemy = EnemyInfo[sender.NetworkId];
                lock (enemy.m_lock)
                {
                    enemy.LastAATick = Environment.TickCount;
                    EnemyInfo[sender.NetworkId] = enemy;
                }
            }
        }
    }
}
