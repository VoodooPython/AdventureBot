﻿using AdventureBot.Item;
using AdventureBot.Room;
using AdventureBot.User;
using Content.Items;

namespace Content.Rooms
{
    [Available(Id, Difficulity.Medium, TownRoot.Id)]
    public class Anime : MonsterBase
    {
        public const string Id = "monster/anime";
        public override string Name => "Анимешник";
        public override string Identifier => Id;
        protected override decimal Health => 150;

        protected override decimal GetDamage(User user)
        {
            return 15;
        }

        protected override void Enter(User user, string[][] buttons)
        {
            SendMessage(user, "— Это не просто мультики! Это искусство!", buttons);
        }

        protected override bool OnRunaway(User user)
        {
            return true;
        }

        protected override void OnWon(User user)
        {
            user.ItemManager.Add(new ItemInfo(AnimeBadge.Id, 1));
        }
    }
}