using _2DRpgGame.Classes.GameControllers;
using _2DRpgGame.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _2DRpgGame.Classes
{
    internal static class GenerateBackground
    {
        private static Bitmap[] tiles;
        private static Dictionary<Tiles, int> tileIndexes = new Dictionary<Tiles, int>();

        internal const int mapTileCount = 34;
        internal const int tilePixels = 16;
        internal const int tileBorderPixels = 1;
        internal const int tileResolution = 8;

        enum Tiles
        {
            TopLeftCorner,
            TopRightCorner,
            BottomLeftCorner,
            BottomRightCorner,
            TopBorder,
            BottomBorder,
            LeftBorder,
            RightBorder,
            Abyss
        }

        internal static void GenerateTilesFromFile()
        {
            Bitmap tilemap = null;
#if(DEBUG)
            tilemap = Resources.Tilemap_Debug;
#else
            tilemap = Resources.Tilemap;
#endif


            int xTileCount = (int)Math.Floor(tilemap.PhysicalDimension.Width / tilePixels);
            int yTileCount = (int)Math.Floor(tilemap.PhysicalDimension.Height / tilePixels);

            tiles = new Bitmap[xTileCount * yTileCount];

            for (int y = 0; y < yTileCount; y++)
            {
                for (int x = 0; x < xTileCount; x++)
                {
                    int index = (int)(y * yTileCount + x);
                    tiles[index] = tilemap.Clone(new RectangleF(new PointF(x * (tilePixels + tileBorderPixels) + tileBorderPixels, y * (tilePixels + tileBorderPixels) + tileBorderPixels), new Size(tilePixels, tilePixels)), PixelFormat.Format32bppArgb);
                    tileIndexes.Add((Tiles)index, index);
                }
            }
        }

        internal static Bitmap GenerateMap()
        {
            if (!Directory.Exists("Maps"))
            {
                Directory.CreateDirectory("Maps");
            }

            using (Bitmap map = new Bitmap(mapTileCount * tilePixels, mapTileCount * tilePixels))
            {
                int border = 3;
                int borderP = 4;


                for (int yMap = 0; yMap < mapTileCount; yMap++)
                {
                    for (int xMap = 0; xMap < mapTileCount; xMap++)
                    {
                        int tileIndex = GameUpdate.Random.Next(9, 16);

                        if (xMap < border || yMap < border || xMap > mapTileCount - borderP || yMap > mapTileCount - borderP)
                        {
                            tileIndex = (int)Tiles.Abyss;
                        }
                        else if (xMap == border && yMap == border)
                        {
                            tileIndex = (int)Tiles.TopLeftCorner;
                        }
                        else if (xMap == border && yMap == mapTileCount - borderP)
                        {
                            tileIndex = (int)Tiles.BottomLeftCorner;
                        }
                        else if (xMap == mapTileCount - borderP && yMap == border)
                        {
                            tileIndex = (int)Tiles.TopRightCorner;
                        }
                        else if (xMap == mapTileCount - borderP && yMap == mapTileCount - borderP)
                        {
                            tileIndex = (int)Tiles.BottomRightCorner;
                        }
                        else if (xMap == border)
                        {
                            tileIndex = (int)Tiles.LeftBorder;
                        }
                        else if (yMap == border)
                        {
                            tileIndex = (int)Tiles.TopBorder;
                        }
                        else if (xMap == mapTileCount - borderP)
                        {
                            tileIndex = (int)Tiles.RightBorder;
                        }
                        else if (yMap == mapTileCount - borderP)
                        {
                            tileIndex = (int)Tiles.BottomBorder;
                        }

                        Bitmap tile = tiles[tileIndex];

                        for (int yTile = 0; yTile < tilePixels; yTile++)
                        {
                            for (int xTile = 0; xTile < tilePixels; xTile++)
                            {
                                Color color = tile.GetPixel(xTile, yTile);
                                map.SetPixel(xMap * tilePixels + xTile, yMap * tilePixels + yTile, color);
                            }
                        }
                    }
                }

                //map.Save(Path.Combine("Maps", $"Map{random.Next(1000)}.png"));

                map.SetResolution(tileResolution, tileResolution);
                return (Bitmap)map.Clone();
            }
        }
    }
}
