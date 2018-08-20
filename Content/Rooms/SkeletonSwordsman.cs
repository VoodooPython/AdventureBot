using AdventureBot.Room;
using AdventureBot.User;

namespace Content.Rooms
{
    [Available(Id, Difficulity.Easy, TownRoot.Id)]
    public class SkeletonSwordsman : MonsterBase
    {
        public const string Id = "monster/SkeletonSwordsman";
        public override string Name => "Скелет-воин";
        public override string Identifier => Id;
        protected override decimal Health => 35;

        protected override decimal GetDamage(User user)
        {
            return 12;
        }

        protected override void Enter(User user, string[][] buttons)
        {
            SendMessage(user, "Мы же не в данже! Ох уж эти некроманты! Гадят своими мертвецами где попало!", buttons);
        }

        protected override bool OnRunaway(User user)
        {
            return true;
        }

        protected override void OnWon(User user)
        {
            user.Info.Gold += 20;
        }
    }
}