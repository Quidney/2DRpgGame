using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Properties;
using System.Collections.Generic;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal static class ItemDatabase
    {
        private static List<Item> Items = new List<Item>();
        private static List<Weapon> Weapons = new List<Weapon>();
        private static List<Armor> Armors = new List<Armor>();

        private static Projectile Arrow = new Projectile(20f, 98, Resources.Arrow1, new Rectangle(new Point(512, 512), new Size(5, 10)));
        private static Projectile Dagger = new Projectile(10f, 85, Resources.Dagger1, new Rectangle(new Point(512, 512), new Size(7, 12)));

        internal static void InitItems()
        {
            //Melee Weapons
            MeleeWeapon spear = new MeleeWeapon("Spear", "Little brother's design", 45f, Resources.Spear1, 50f, true, 30f, 5f, 2f, MeleeWeapon.AttackTypes.Impale);
            Items.Add(spear);

            MeleeWeapon bigSword = new MeleeWeapon("Iron Greatsword", "Gut from berk", 50f, Resources.BigSword1, 30f, true, 30f, 10f, 4f, MeleeWeapon.AttackTypes.Swing);
            Items.Add(bigSword);

            MeleeWeapon bigHammer = new MeleeWeapon("Big Hammer", "Bonk!", 50f, Resources.BigHammer1, 35f, true, 35f, 15f, 5f, MeleeWeapon.AttackTypes.Swing);
            Items.Add(bigHammer);

            MeleeWeapon dagger = new MeleeWeapon("Thief Dagger", "For the agile", 40f, Resources.Dagger1, 10f, false, 10f, 5f, 1f, MeleeWeapon.AttackTypes.Swing);
            Items.Add(dagger);
            
            //Ranged Weapons
            RangedWeapon throwingDagger = new RangedWeapon("Throwing Dagger", "Damn, too many projectiles", 35f, Resources.Dagger1, 10f, false, 100f, 20f, 1f, Dagger, RangedWeapon.AttackTypes.Penta);
            Items.Add(throwingDagger);

            RangedWeapon basicBow = new RangedWeapon("Basic Bow", "Finally Works :D", 30f, Resources.Bow1, 10f, true, 40f, 15f, 2f, Arrow, RangedWeapon.AttackTypes.Single);
            Items.Add(basicBow);

            //Armors
            Armor woodenHelmet = new Armor("Wooden Helmet", "Basic protection", 4f, Resources.WoodenHelmet_Item, Armor.Types.Helmet, 1f);
            Items.Add(woodenHelmet);

            Armor woodenChestplate = new Armor("Wooden Chestplate", "Basic protection", 4f, Resources.WoodenChestplate_Item, Armor.Types.Helmet, 2f);
            Items.Add(woodenChestplate);

            Armor woodenLeggings = new Armor("Wooden Leggings", "Basic protection", 4f, Resources.WoodenLeggings_Item, Armor.Types.Helmet, 2f);
            Items.Add(woodenLeggings);

            Armor woodenBoots = new Armor("Wooden Boots", "Basic protection", 4f, Resources.WoodenBoots_Item, Armor.Types.Helmet, 1f);
            Items.Add(woodenBoots);

            InitLists();
        }

        private static void InitLists()
        {
            foreach (Item item in Items)
            {
                if (item is Weapon weapon)
                {
                    Weapons.Add(weapon);
                }
                else if (item is Armor armor)
                {
                    Armors.Add(armor);
                }
            }
        }

        internal static Weapon GetRandomWeapon()
        {
            return (Weapon)Weapons[GameUpdate.Random.Next(Weapons.Count)].Clone();
        }

        internal static Armor GetRandomArmor()
        {
            return (Armor)Armors[GameUpdate.Random.Next(Armors.Count)].Clone();
        }

        internal static Weapon Fists()
        {
            Weapon fists = new MeleeWeapon("Fists", "Barehand", 0f, Resources.Fists1, 0f, true, 40f, 5f, 0f, MeleeWeapon.AttackTypes.Impale);
            Equipment.SetRarity(fists, Equipment.Rarities.None);
            return fists;
        }
    }
}
