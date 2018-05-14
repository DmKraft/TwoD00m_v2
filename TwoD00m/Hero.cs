using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Collections;
using TwoD00m.PlayerItems.Potions;
using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.Interface;

namespace TwoD00m
{
    public class Hero : Creature
    {
        public SurvivalPoint AP;
        public bool madeTurn = true;
        Potion p;
        Weapon leftHand;
        Weapon rightHand;
        private int straterPotionNum = 0;
        private int starterWeaponNum = 0;

        public int currentPotionNum;
        public int currentWeaponNum;
        public int currentWeaponInLeftHand;
        public int currentWeaponInRightHand;
        static Weapon HeroWeapon;


        public Inventory inventory = new Inventory();
        
        GameGUI gameGui;

        // private List<Creature> targets = new List<Creature>();
        public Hero()
        {
            HP = new SurvivalPoint(50, 100);
            AP = new SurvivalPoint(25, 100);
            
            inventory.StartInventory();
            HeroWeapon =inventory.AvailableWeapon();
            position = new Point(4, 4);
            direction = Direction.North;
            p = inventory.GetPotion(straterPotionNum);
            leftHand = inventory.GetWeapon(starterWeaponNum);
            rightHand = inventory.GetWeapon(starterWeaponNum);
            setAlpha();

          //  gameGui = new GameGUI(this);
        }

        public void setPotion()
        {
            int zaglushka = 5;
            this.p = inventory.GetPotion(zaglushka);
        }

        public void setLeftHandWeapon()
        {
            int zaglushka = 5;
            this.leftHand = inventory.GetWeapon(zaglushka);
        }

        public void setRightHandWeapon()
        {
            int zaglushka = 6;
            this.leftHand = inventory.GetWeapon(zaglushka);
        }

        public void setAlpha()
        {
            alpha.Y = (int)Math.Sin(direction.getRadianAngleDirection());
            alpha.X = (int)Math.Cos(direction.getRadianAngleDirection());
        }
        
        public void leftMove(World map)
        {
            movements(map, Direction.getLeftDirection(direction));
            setAlpha();
        }

        public void rightMove(World map)
        {
            movements(map, Direction.getRightDirection(direction));
            setAlpha();
        }

        public void backwardMove(World map)
        {
            HP.Actual -= 3f;
            movements(map, Direction.getBackDirection(direction));
            setAlpha();
        }

        public void forwardMove(World map)
        {
            HP.Actual += 3f;
            movements(map, direction);
            setAlpha();
        }
        
        public void leftTurn()
        {
            direction = Direction.getLeftDirection(direction);
            setAlpha();
        }

        public void rightTurn()
        {
            direction = Direction.getRightDirection(direction);
            setAlpha();
        }

        public void UsePotion(int num)    //функция получения целей необходима
        {
            Potion p = inventory.GetPotion(num);
            string name = p.name.ToLower();
            if (name.Contains("heal"))
                p.Use(this);
            if (name.Contains("poison"))
                p.Use(this);
        }

        public void UsePotion()
        {
            string name = p.name.ToLower();
            if (name.Contains("heal"))
                this.p.Use(this);
            if (name.Contains("poison"))
                this.p.Use(this);
            inventory.potions.RemoveAt(inventory.potions.IndexOf(p));
        }

        public void impact(Dictionary<Point, Monster> monstersList, World map)
        {
            List<Point> AvailablePoint = HeroWeapon.Search(position, direction, map);
            foreach (Point point in AvailablePoint)
            {
                if (monstersList.ContainsKey(point))                                    
                HeroWeapon.Attack(monstersList[point]);
            }
        }

        public void ShowHidePayerHUD()
        {
          //  gameGui.GetPlayerHUD().InvertVisible();
        }
        public void UpdateGUI()
        {
            //gameGui.update();
        }
        public void DrawGUI()
        {
            //gameGui.DrawGUI();
        }
        
    }
}
