﻿using System.Linq;
using AdventureBot;
using AdventureBot.Messenger;
using AdventureBot.User;

namespace Content.Rooms.MegaMonster.Room
{
    public partial class MegaMonsterRoom
    {
        private void BeginBattle(User user)
        {
            SwitchAction(user, SelectTarget);
            SendMessage(user, "Вы встали в пафосную позу и бьете монстра прямо...", GetButtons(user));
        }

        private void BattleTarget(User user, Place place)
        {
            using (var stats = new StatsContext(user.Random, GetRoomVariables(user)))
            {
                GetRoomVariables(user).Set("current_defence", new Serializable.Decimal(
                    (stats.Stats.DefencedPlaces & place) != 0
                        ? stats.Stats.Defence
                        : 0
                ));
            }

            SwitchAction(user, Battle);
            SendMessage(user, "Чем же вы ударите?", GetItems(user)
                .Select(i => new[] {i})
                .ToArray());
        }

        private void SelectTarget(User user, RecivedMessage message)
        {
            HandleButtonAlways(user, message);
        }

        private void Battle(User user, RecivedMessage messageRecived)
        {
            if (!UseItem(user, messageRecived))
            {
                SendMessage(user, "Чтобы что-то использовать, надо сначала этим чем-то завладеть");
                return;
            }

            using (var stats = new StatsContext(user.Random, GetRoomVariables(user)))
            {
                if (stats.Stats.Health <= 0)
                {
                    // Won!
                    user.RoomManager.Leave();
                    return;
                }

                SwitchAction(user, SelectTarget);
                SendMessage(user, "А монстр то ещё жив! Постарайтесь это исправить, пока ещё можете.");
                SendMessage(user,
                    $"Вы оставили монстру {stats.Stats.Health:0.##} здоровья, а он вам его попытался понизить на {stats.Stats.Damage:0.##}");
                SendMessage(user, "Как будем исправлять ситуацию?", GetButtons(user));
                user.Info.MakeDamage(stats.Stats.Damage);
            }
        }

        public void MakeDamage(User user, decimal damage)
        {
            using (var stats = new StatsContext(user.Random, GetRoomVariables(user)))
            {
                var def = GetRoomVariables(user).Get<Serializable.Decimal>("current_defence");
                stats.Stats.Health -= UserInfo.CalculateDefence(damage, def);
            }
        }
    }
}