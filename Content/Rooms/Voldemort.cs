﻿using System;
using AdventureBot;
using AdventureBot.Room;
using AdventureBot.User;

namespace Content.Rooms
{
    [Available(Id, Difficulity.Hard)]
    public class Voldemort : MonsterBase
    {
        public const string Id = "monster/voldemort";
        public override string Name => "Волан-де-Морт";
        public override string Identifier => Id;

        protected override decimal Health
        {
            get
            {
                var no = (int?) (Serializable.Int) GlobalVariables.Variables.Get("voldemort_no") ?? 1;
                // 2.5% per each death
                return 50 * (decimal) Math.Pow(1.025, no);
            }
        }

        protected override decimal GetDamage(User user)
        {
            var no = (int?) (Serializable.Int) GlobalVariables.Variables.Get("voldemort_no") ?? 1;
            // 1% per 5 deaths
            // ReSharper disable once PossibleLossOfFraction
            return 40 * (decimal) Math.Pow(1.01, no / 5);
        }

        protected override void Enter(User user, string[][] buttons)
        {
            var no = (int?) (Serializable.Int) GlobalVariables.Variables.Get("voldemort_no") ?? 1;
            var time = DateTimeOffset.FromUnixTimeSeconds(
                (int?) (Serializable.Long) GlobalVariables.Variables.Get("voldemort_time") ?? 0);
            // var player = (string) (Serializable.String) GlobalVariables.Variables.Get("voldemort_killer");

            var timeOffset = DateTimeOffset.Now - time;

            SendMessage(user,
                "Одно его имя внушает страх, как и все трехстепенные имена, а отсутствие носа только усиливает это ощущение.");
            SendMessage(user,
                $"Он воскресал уже {no} раз. Его реинкарнация длится уже {timeOffset.TotalMinutes:F2} минут, и снова умирать он не собирается.");
            SendMessage(user, "Его взгляд совсем не кажется дружелюбным. Похоже, пора расчехлять оружие.", buttons);
        }

        protected override bool OnRunaway(User user)
        {
            return true;
        }

        protected override void OnWon(User user)
        {
            var no = (int?) (Serializable.Int) GlobalVariables.Variables.Get("voldemort_no") ?? 1;

            GlobalVariables.Variables.Set("voldemort_no", new Serializable.Int(no + 1));
            GlobalVariables.Variables.Set("voldemort_time",
                new Serializable.Long(DateTimeOffset.Now.ToUnixTimeSeconds()));
            // GlobalVariables.Variables.Set("voldemort_killer", new Serializable.Long(DateTimeOffset.Now.ToUnixTimeSeconds()));
        }
    }
}