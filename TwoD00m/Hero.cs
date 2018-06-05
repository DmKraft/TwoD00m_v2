using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Collections;
using TwoD00m.PlayerItems.Potions;
using TwoD00m.PlayerItems.Arsenal;
using TwoD00m.Interface;
using TwoD00m.cWorld;

namespace TwoD00m
{
    public class Hero : Creature
    {
        public SurvivalPoint AP;
        public Inventory inventory = new Inventory();

        public bool inventoryOpen = false; // вряд ли это должно быть тут

        private Weapon PoketWeaponFirst;
        private Weapon PoketWeaponSecond;
        private Potion PoketPotion;

        public int SelectedPocket
        {
            get { return SelectedPocket; }
            set
            {
                if (value < 0)
                    { SelectedPocket = 0; }
                else if (value > Inventory.weapons.Count)
                    { SelectedPocket = Inventory.weapons.Count; }
                else
                    SelectedPocket = value ;
            }
        }
        
        // private List<Creature> targets = new List<Creature>();
        public Hero()
        {
            place = new Place(new Point(4, 8), Direction.South);
            HP = new SurvivalPoint(100, 100);
            AP = new SurvivalPoint(25, 100);
            
            inventory.StartInventory();
            PoketPotion = inventory.GetPotion(0);
            PoketWeaponFirst = inventory.GetWeapon(0);
            PoketWeaponSecond = inventory.GetWeapon(0);
        }
                
        public void GetLeftElement()
        {
            this.SelectedPocket = - 1;
            this.inventory.GetWeapon(SelectedPocket);
        }

        public void GetRightElement()
        {
            this.SelectedPocket =+ 1;
            this.inventory.GetWeapon(SelectedPocket);
        }

        public void setPotion()
        {
            int zaglushka = 5;
            this.PoketPotion = inventory.GetPotion(zaglushka);
        }

        public void setLeftHandWeapon()
        {
            int zaglushka = 5;
            PoketWeaponFirst = inventory.GetWeapon(zaglushka);
        }

        public void setRightHandWeapon()
        {
            int zaglushka = 6;
            PoketWeaponFirst = inventory.GetWeapon(zaglushka);
        }

        public void LeftMove(World map)
        {
            Move(map, Direction.GetLeftDirection(Direction));
        }

        public void RightMove(World map)
        {
            Move(map, Direction.GetRightDirection(Direction));
        }

        public void BackwardMove(World map)
        {
            Move(map, Direction.GetBackDirection(Direction));
        }

        public void ForwardMove(World map)
        {
            Move(map, Direction);
        }
        
        public void LeftTurn()
        {
            place.SetLeftDirection();
        }

        public void RightTurn()
        {
            place.SetRightDirection();
        }

        public void UsePotion(int num)    //функция получения целей необходима
        {
            Potion p = inventory.GetPotion(num);
            string name = p.name.ToLower();
            if (name.Contains("heal"))
                p.Use(this);
            if (name.Contains("poison")) // полиморфизм нужен, а не это
                p.Use(this);
        }
        public void Use(World map)
        {
            Point point = Position;           
            point.Y -= Direction.Alpha.Y;
            point.X += Direction.Alpha.X;
            map.GetBlock(point).Use();
        }

        public void UsePotion()
        {
            string name = PoketPotion.name.ToLower();
            if (name.Contains("heal"))
                this.PoketPotion.Use(this);
            if (name.Contains("poison"))
                this.PoketPotion.Use(this);
            inventory.potions.RemoveAt(inventory.potions.IndexOf(PoketPotion));
        }

        public void impact(Dictionary<Point, Monster> monstersList, World map)
        {
            List<Point> AvailablePoint = PoketWeaponFirst.Search(Position, Direction, map);
            foreach (Point point in AvailablePoint)
            {
                if (monstersList.ContainsKey(point))                                    
                PoketWeaponFirst.Attack(monstersList[point]);
            }
        }

        public void ShowHidePayerHUD()
        {
            GameGUI.GetPlayerHUD().InvertVisible();
        }
        public void UpdateGUI()
        {
            GameGUI.Update();
        }
        public void DrawGUI()
        {
            GameGUI.Draw();
        }
        
    }
}
