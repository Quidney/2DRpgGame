using _2DRpgGame.Classes.Items;

namespace _2DRpgGame.Classes
{
    internal static class Inventory
    {
        internal static Item[] Items = new Item[36];

        internal static int HotbarIndex = 0;

        internal static Item HeldItem;
        internal static Armor Helmet;
        internal static Armor Chestplate;
        internal static Armor Leggings;
        internal static Armor Boots;

        internal static bool AddItem(Item _item)
        {
            bool success = false;

            for (int i = 0; i < 9; i++)
            {
                if (Items[HotbarToInv(i)] == null)
                {
                    Items[HotbarToInv(i)] = _item;
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i] == null)
                    {
                        Items[i] = _item;
                        success = true;
                        break;
                    }
                }
            }
            UpdateInv();
            return success;
        }

        internal static void RemoveItem()
        {
            UpdateInv();
        }

        private static void UpdateInv()
        {
            HeldItem = Items[HotbarToInv(HotbarIndex)];

            if (HeldItem is Weapon weapon)
            {
                DrawWeapon.Draw(weapon);

                Player.MoveSpeed = (8f - weapon.Weight);
            }
            else
            {
                DrawWeapon.Draw(ItemDatabase.Fists());
                Player.MoveSpeed = 8f;
            }
        }

        private const int hotbarInvRat = 27;
        internal static int HotbarToInv(int _index)
        {
            return _index + hotbarInvRat;
        }
        internal static void ChangeSelectedHotbar(int _index)
        {
            HotbarIndex += _index;

            if (HotbarIndex < 0)
                HotbarIndex = 8;

            if (HotbarIndex > 8)
                HotbarIndex = 0;

            UpdateInv();
        }

        internal static void SetSelectedHotbar(int _index)
        {
            HotbarIndex = _index;

            UpdateInv();
        }

        internal static bool EquipArmor(Armor _armor, Armor.Types _type)
        {
            bool success = false;

            if (_armor.Type == _type)
            {
                success = true;

                switch (_type)
                {
                    case Armor.Types.Helmet:
                        Helmet = _armor; break;
                    case Armor.Types.Chestplate:
                        Chestplate = _armor; break;
                    case Armor.Types.Leggings:
                        Leggings = _armor; break;
                    case Armor.Types.Boots:
                        Boots = _armor; break;
                }
            }

            return success;
        }
    }
}
