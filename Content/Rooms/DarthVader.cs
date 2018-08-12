﻿using System;
using System.Collections.Generic;
using AdventureBot;
using AdventureBot.Item;
using AdventureBot.Messenger;
using AdventureBot.Room;
using AdventureBot.User;

namespace Content.Rooms
{
    [Available("monster/vader", Difficulity.Hard)]
    public class DarthVader : RoomBase, IMonster
    {
        private readonly IMonster _battleRoom = new BattleRoom();

        public DarthVader()
        {
            Routes = new MessageRecived[] {PreBattle, Battle};
            Buttons = new NullableDictionary<MessageRecived, Dictionary<string, MessageRecived>>
            {
                {
                    null, new Dictionary<string, MessageRecived>
                    {
                        {
                            "— НЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕТ!", (user, message) =>
                            {
                                SwitchAction(user, PreBattle);
                                user.Info.MakeDamage(15);
                                SendMessage(user, "Тебе отрубили руку. А теперь хорош рыдать и в бой.",
                                    GetButtons(user));
                            }
                        }
                    }
                },
                {
                    PreBattle, new Dictionary<string, MessageRecived>
                    {
                        {
                            "Достать оружие", (user, message) =>
                            {
                                SendMessage(user, "*щелк*");
                                SwitchAction(user, Battle);
                                _battleRoom.OnEnter(user);
                            }
                        }
                    }
                }
            };
        }

        public override string Name => "Дарт Вейдер";
        public override string Identifier => "monster/vader";

        public override void OnEnter(User user)
        {
            base.OnEnter(user);

            SendMessage(user,
                "Перед вами предстал крупный мужчина в черной блестящей респираторной маске и темном матовом плаще.");
            SendMessage(user, "— Я твой отец!", GetButtons(user));
        }

        public override bool OnLeave(User user)
        {
            base.OnLeave(user);

            return _battleRoom.OnLeave(user);
        }

        public override void OnMessage(User user, RecivedMessage message)
        {
            if (!HandleAction(user, message))
            {
                HandleButtonAlways(user, message);
            }
        }

        public void MakeDamage(User user, decimal damage)
        {
            _battleRoom.MakeDamage(user, damage);
        }

        public decimal GetCurrentHealth(User user)
        {
            return _battleRoom.GetCurrentHealth(user);
        }

        private void PreBattle(User user, RecivedMessage message)
        {
            HandleButtonAlways(user, message);
        }

        private void Battle(User user, RecivedMessage message)
        {
            _battleRoom.OnMessage(user, message);
        }

        private class BattleRoom : MonsterBase
        {
            public override string Name => "Битва с отцом";
            public override string Identifier => "monster/vader/battle";
            protected override decimal Health => 550;

            protected override decimal GetDamage(User user)
            {
                return 40;
            }

            protected override void Enter(User user, string[][] buttons)
            {
                SendMessage(user, "Световой меч озарил все вокруг своим кроваво-красным светом.", buttons);
            }

            protected override bool OnRunaway(User user)
            {
                return true;
            }

            protected override void OnWon(User user)
            {
                var rand = user.Random.Next(3);
                switch (rand)
                {
                    case 0:
                        user.ItemManager.Add(new ItemInfo("vader/respirator", 1));
                        break;
                    case 1:
                        user.ItemManager.Add(new ItemInfo("vader/sword", 1));
                        break;
                    case 2:
                        user.ItemManager.Add(new ItemInfo("vader/cloak", 1));
                        break;
                    // ReSharper disable once RedundantEmptySwitchSection
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rand), rand, "rand > 2 || rand < 0");
                }
            }
        }
    }
}