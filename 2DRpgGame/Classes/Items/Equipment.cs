using _2DRpgGame.Classes.GameControllers;
using System.Collections.Generic;
using System.Drawing;

namespace _2DRpgGame.Classes.Items
{
    internal abstract class Equipment : Item
    {
        internal Rarities Rarity { get; private set; }
        internal Equipment(string _name, string _tooltip, float _value, Bitmap _sprite) : base(_name, _tooltip, _value, _sprite)
        {
            RerollEquipmentRarity(this);
        }

        internal static Dictionary<Rarities, float> RarityDamageModifier = new Dictionary<Rarities, float>()
        {
            { Rarities.Common, 0.4f },
            { Rarities.Uncommon, 0.9f },
            { Rarities.Rare, 1f },
            { Rarities.Epik, 1.6f },
            { Rarities.Legendary, 2f }
        };

        internal static void RerollEquipmentRarity(Equipment _equipment)
        {
            double rarity = GameUpdate.Random.NextDouble();

            if (rarity < 0.60)
            {
                _equipment.Rarity = Rarities.Common;
            }
            else if (rarity >= 0.60 && rarity < 0.80)
            {
                _equipment.Rarity = Rarities.Uncommon;
            }
            else if (rarity >= 0.80 && rarity < 0.90)
            {
                _equipment.Rarity = Rarities.Rare;
            }
            else if (rarity >= 0.90 && rarity < 0.99)
            {
                _equipment.Rarity = Rarities.Epik;
            }
            else if (rarity >= 0.99 && rarity < 1)
            {
                _equipment.Rarity = Rarities.Legendary;
            }
        }

        internal static void SetRarity(Equipment _equipment, Rarities _rarity)
        {
            _equipment.Rarity = _rarity;
        }

        internal enum Rarities
        {
            None = 0,
            Common,
            Uncommon,
            Rare,
            Epik,
            Legendary
        }

        private static Dictionary<Rarities, Color> RarityColorDict = new Dictionary<Rarities, Color>()
        {
            { Rarities.Common, Color.Gray },
            { Rarities.Uncommon, Color.Green },
            { Rarities.Rare, Color.Blue },
            { Rarities.Epik, Color.Purple },
            { Rarities.Legendary, Color.Yellow }
        };

        internal static Color RarityColor(Rarities _rarity)
        {
            return RarityColorDict[_rarity];
        }
    }
}
