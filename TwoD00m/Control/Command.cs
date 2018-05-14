using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace TwoD00m
{
    public class Command
    {
        public bool EndTurn { get; set; }
        public virtual void execute(Hero hero, World world, Dictionary <Point,Monster> monstersList) { }
    }

    public class SetPotion : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.setPotion(); }
    }

    public class UsePotion : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.UsePotion();}
    }

    public class SetLeftHandWeapon : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) {hero.setLeftHandWeapon(); }
    }

    public class SetRightHandWeapon : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.setRightHandWeapon(); }
    }

    public class StepFront: Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.forwardMove(world); EndTurn = true; }
    }

    public class StepBack : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.backwardMove(world); EndTurn = true; }
    }

    public class StepLeft : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.leftMove(world); EndTurn = true; }
    }

    public class StepRight : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.rightMove(world); EndTurn = true; }
    }

    public class TurnLeft : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.leftTurn(); EndTurn = false; }
    }

    public class TurnRight : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.rightTurn(); EndTurn = false; }
    }
    public class Attack : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point,Monster> monstersList) { hero.impact(monstersList,world); EndTurn = true; }
    }
    public class PlayerHUD : Command
    {
        public override void execute(Hero hero, World world, Dictionary<Point, Monster> monstersList) { hero.ShowHidePayerHUD(); EndTurn = false; }
    }
}
