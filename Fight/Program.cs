using System;
using System.Collections.Generic;

namespace CSharpLight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isWork = true;

            while (isWork)
            {
                Field field = new Field();

                if (field.TryCreativeBattle())
                {
                    Console.ReadKey();
                    Console.Clear();
                    field.Battle();
                    field.ShowBattleResult();
                }
            }
        }
    }

    class Field
    {
        private Fighter _firstFighter;
        private Fighter _secondFighter;
        private List<Fighter> _fighters = new List<Fighter>();

        public bool TryCreativeBattle()
        {
            Console.WriteLine("Олег");
            ChooseFighter(out _firstFighter);
            Console.WriteLine("Виталя");
            ChooseFighter(out _secondFighter);

            if (_firstFighter == null || _secondFighter == null)
            {
                Console.WriteLine("Нет такого бойца");
                return false;
            }
            else
            {
                Console.WriteLine("Бойцы выбраны");
                return true;
            }
        }

        public Field()
        {
            _fighters.Add(new Archer("лучник", 100, 100, 40));
            _fighters.Add(new Wizard("маг", 100, 100, 15));
            _fighters.Add(new Knight("рыцарь", 100, 100, 50));
            _fighters.Add(new Barbarian("варвар", 90, 100, 30));
            _fighters.Add(new Mystic("мистик", 100, 100, 22));
        }

        public void ShowBattleResult()
        {
            if (_firstFighter.Health <= 0 && _secondFighter.Health <= 0)
            {
                Console.WriteLine("Ничья");
            }
            else if (_firstFighter.Health <= 0)
            {
                Console.WriteLine($"{_secondFighter.Name} победил!");
            }
            else if (_secondFighter.Health <= 0)
            {
                Console.WriteLine($"{_firstFighter.Name} победил!");
            }
        }

        public void Battle()
        {
            while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
            {
                _firstFighter.ShowStats();
                _secondFighter.ShowStats();
                _firstFighter.TakeDamege(_secondFighter.Damage);
                _secondFighter.TakeDamege(_firstFighter.Damage);
                _firstFighter.UseSpecialAttack();
                _secondFighter.UseSpecialAttack();
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ChooseFighter(out Fighter fighter)
        {
            ShowFighters();
            Console.Write("Напиши номер бойца: ");
            bool isNumber = int.TryParse(Console.ReadLine(), out int inputID);

            if (isNumber == false)
            {
                Console.WriteLine("Ошибка! Вы ввели не верно.");
                fighter = null;
            }
            else if (inputID > 0 && inputID - 1 < _fighters.Count)
            {
                fighter = _fighters[inputID - 1];
                _fighters.Remove(fighter);
                Console.WriteLine("Боец успешно выбран.");
            }
            else
            {
                Console.WriteLine("Бойца с таким номером отсутствует.");
                fighter = null;
            }
        }

        private void ShowFighters()
        {
            Console.WriteLine("Список доступный бойцов");

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.Write(i + 1);
                _fighters[i].ShowStats();
            }
        }
    }

    class Fighter
    {
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float Armor { get; protected set; }
        public float Damage { get; protected set; }

        public Fighter(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void TakeDamege(float damageFighter)
        {
            float finalDamage = 0;
            int absorbedArmorFactor = 100;

            if (Armor == 0)
            {
                Health -= damageFighter;
            }
            else
            {
                finalDamage = damageFighter / absorbedArmorFactor * Armor;
                Armor -= finalDamage;
                Health -= finalDamage;
            }

            Console.WriteLine($"{Name} нанес {finalDamage} урона");
        }

        public void ShowStats()
        {
            Console.WriteLine($" {Name}. Здоровье: {Health}. Броня: {Armor} Урон {Damage}");
        }

        public void UseSpecialAttack()
        {
            Random random = new Random();
            int rangeMaximalNumbers = 100;
            int chanceUsingAbility = random.Next(rangeMaximalNumbers);
            int chanceAbility = 20;

            if (chanceUsingAbility < chanceAbility)
            {
                UsePower();
            }
        }

        protected virtual void UsePower() { }
    }

    class Archer : Fighter
    {
        private int _damageBuff = 55;
        private int _armorBuff = 45;

        public Archer(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} использует супер-атаку урон и броня увелечены");
            Damage += _damageBuff;
            Armor += _armorBuff;
        }
    }

    class Knight : Fighter
    {
        private int _healthBuff = 60;

        public Knight(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал восстоновление. Здоровье увелечено");
            Health += _healthBuff;
        }
    }

    class Wizard : Fighter
    {
        private int _damageBuff = 20;

        public Wizard(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} ипользовал зелья силы. Урон увелечено");
            Damage += _damageBuff;
        }
    }

    class Barbarian : Fighter
    {
        private int _damageBuff = 60;
        private int _armorBuff = 50;

        public Barbarian(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} укрепил свои мышцы. Урон и броня увеличены");
            Armor += _armorBuff;
            Damage += _damageBuff;
        }
    }

    class Mystic : Fighter
    {
        private int _armorBuff = 50;

        public Mystic(string name, int health, int armor, int damage) : base(name, health, damage, armor) { }

        protected override void UsePower()
        {
            Console.WriteLine($"{Name} использовал улучшение брони. Броня увеличена");
            Armor += _armorBuff;
        }
    }
}