using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Homm5RMG.Data_Layer
{

    //    enum eTerrainAddressOffset
    //{
    //    StartOffset = 0x00042,
    //    Conquest_Dirt = 0x00043, //Full FF
    //    Lava_Arena_Brick = 0x01573,
    //    Conquest_Mud = 0x02AAC,
    //    Conquest_Paving = 0x03FDC,
    //    Conquest_SandStone = 0x0550F,
    //    Dirt_DarkGround = 0x06A46,
    //    Conquest_Stone = 0x07F7A,
    //    Dirt_Weed = 0x094AB,
    //    Dirt_Ground = 0x0A9D8,
    //    Grass_DarkGrass = 0x0BF07, //Full FF
    //    Grass_Grass = 0x0D43B, //full ff
    //    Grass_Flowers = 0x0E96A, 
    //    Grass_Field = 0x0FE9B, 
    //    Grass_UsedGrass = 0x113CA, 
    //    Lava_Lava = 0x128FE, 
    //    Lava_DeadLand = 0x13E2B,
    //    Lava_DeadLand2 = 0x1535D,
    //    Lava_DeadLand3 = 0x16891,
    //    Lava_DeadLand4 = 0x17DC5,
    //    Sand_RiverBed = 0x192F9,
    //    Subterrain_Subterrain2 = 0x1A82B,
    //    Subterrain_Subterrain = 0x1BD68,
    //    Subterrain_Subterrain3 = 0x1D2A1,
    //    Dirt_Root = 0x1E7DE,
    //    Subterrain_DwarfFloor = 0x1FD0B,
    //    Subterrain_DwarfFloorIncrusted = 0x21245,
    //    Subterrain_DwarfFloorRaised = 0x22789,
    //    Grass_Nidle_Grass_Green = 0x23CCA,
    //    Grass_StoneRoad = 0x25205,
    //    Grass_Road = 0x26738,
    //    Lava_InfernoBricks = 0x27C66,
    //    Snow_Snow= 0x291C,
    //    Snow_Snow2 = 0x2A6C9,
    //    Snow_Snow3 = 0x2BBFA,
    //    Sand_SandDune = 0x2D12B,
    //    Sand_SandDunes = 0x2E65D,
    //    Sand_Dunes = 0x2FB90,
    //    Sand_Rock = 0x310BE,
    //    Sand_Cracked = 0x325EB,
    //    Sand_SandDirt = 0x33B20,
    //    Sand_SandRock = 0x35052,
    //    Dirt_DirtRoad = 0x36584,
    //    Sand_ScatterStonesSand = 0x37AB5,
    //    Lava_LavaRoad = 0x38FF1,
    //    Subterrain_SubterrainRoad = 0x3A522,
    //    Sand_SandRoad = 0x3BA5F,
    //    Snow_SnowRoad2 = 0x3CF90,
    //    Sand_SandWet = 0x3E4C2,
    //    Dirt_Ground2 = 0x3F9F3,
    //    OrcTerrain_RockFloor = 0x40F24,
    //    Grass_RockFloorGrass = 0x4245E,
    //    OrcTerrain_MossyRock = 0x43998,
    //    OrcTerrain_NidleDirt = 0x44ED2,
    //    OrcTerrain_NidleGrass = 0x4640C,
    //    OrcTerrain_ScatterStone = 0x47947
    //    Grass_ScatterStoneGrass,
    //    OrcTerrain_ScatterStoneDirt,
    //    OrcTerrain_ScatterStoneGrass,
    //    Grass_ScatterStoneGreenGrass,
    //    OrcTerrain_RockFloorGreen,
    //    Grass_RockFloorGreenGrass,
    //    Dirt_ScatterStoneDirt,
    //    OrcTerrain_WhiteRock,
    //    OrcTerrain_Moss,
    //    OrcTerrain_NidleDirtRoad,
    //    OrcTerrain_NidleRoad,
    //    Grass_RiverBedGrass,
    //}
    public enum eTerrainAddressOffset
    {
        Conquest_Dirt = 0, //Full FF
        Lava_Arena_Brick = 1,
        Conquest_Mud = 2,
        Conquest_Paving = 3,
        Conquest_SandStone = 4,
        Dirt_DarkGround = 5,
        Conquest_Stone = 6,
        Dirt_Weed = 7,
        Dirt_Ground = 8,
        Grass_DarkGrass = 9, //Full FF
        Grass_Grass = 10, //full ff
        Grass_Flowers = 11, 
        Grass_Field = 12, 
        Grass_UsedGrass = 13, 
        Lava_Lava = 14, 
        Lava_DeadLand = 15,
        Lava_DeadLand2 = 16,
        Lava_DeadLand3 = 17,
        Lava_DeadLand4 = 18,
        Sand_RiverBed = 19,
        Subterrain_Subterrain2 = 20,
        Subterrain_Subterrain = 21,
        Subterrain_Subterrain3 = 22,
        Dirt_Root = 23,
        Subterrain_DwarfFloor = 24,
        Subterrain_DwarfFloorIncrusted = 25,
        Subterrain_DwarfFloorRaised = 26,
        Grass_Nidle_Grass_Green = 27,
        Grass_StoneRoad = 28,
        Grass_Road = 29,
        Lava_InfernoBricks = 30,
        Snow_Snow= 31,
        Snow_Snow2 = 32,
        Snow_Snow3 = 33,
        Sand_SandDune = 34,
        Sand_SandDunes = 35,
        Sand_Dunes = 36,
        Sand_Rock = 37,
        Sand_Cracked = 38,
        Sand_SandDirt = 39,
        Sand_SandRock = 40,
        Dirt_DirtRoad = 41,
        Sand_ScatterStonesSand = 42,
        Lava_LavaRoad = 43,
        Subterrain_SubterrainRoad = 44,
        Sand_SandRoad = 45,
        Snow_SnowRoad2 = 46,
        Sand_SandWet = 47,
        Dirt_Ground2 = 48,
        OrcTerrain_RockFloor = 49,
        Grass_RockFloorGrass = 50,
        OrcTerrain_MossyRock = 51,
        OrcTerrain_NidleDirt = 52,
        OrcTerrain_NidleGrass = 53,
        OrcTerrain_ScatterStone = 54,
        Grass_ScatterStoneGrass = 55,
        OrcTerrain_ScatterStoneDirt = 56,
        OrcTerrain_ScatterStoneGrass = 57,
        Grass_ScatterStoneGreenGrass = 58,
        OrcTerrain_RockFloorGreen = 59,
        Grass_RockFloorGreenGrass = 60,
        Dirt_ScatterStoneDirt = 61,
        OrcTerrain_WhiteRock = 62,
        OrcTerrain_Moss = 63,
        OrcTerrain_NidleDirtRoad = 64,
        OrcTerrain_NidleRoad = 65,
        Grass_RiverBedGrass = 66,
    }

    public enum eTerrainType
    {
        Conquest_Dirt = 0 ,
        Grass_Grass 
    }

    class TerrainWriter
    {


        const string STRFILEPATH = "";
        BinaryWriter brTerrainFile;
        MapSize msizeSize;

        public TerrainWriter()
        {

        }


        public TerrainWriter(string strFilePath ,MapSize mszSize)
        {
            brTerrainFile = new BinaryWriter(File.Open(strFilePath, FileMode.Open));
            msizeSize = mszSize;

        }

        public const int ITERRAINNUMBER = 67;
        public const int ISTARTOFFSET = 0x0003D;
        public const int ISTARTSEQUENCESIZE = 5;
       // public const int ICHAINOFFSET = 0x00042;

        public void WriteTerrainData(ObjectsMap TerrainMap )
        {
            int iNewX,iNewY;
            //first read start char sequence (this signifise the start of a terrain block in each file)
            brTerrainFile.Seek(ISTARTOFFSET, SeekOrigin.Begin);
            byte[] iarrStartSequence = new byte[ISTARTSEQUENCESIZE];
            for (int i = 0; i < ISTARTSEQUENCESIZE; i++)
            {
                iarrStartSequence[i] = (byte) brTerrainFile.BaseStream.ReadByte();
            }
            
            for (int TerrainIndex = 0; TerrainIndex < ITERRAINNUMBER; TerrainIndex++)
			{
			       
                //for (int i = 0; i < ((int)MapSize.Medium + 1) * ((int)MapSize.Medium + 1) / 4; i++)
                //{
                //    brTerrainFile.Write((byte)0x00);
                //}
                for (int i = 0; i < (int)msizeSize +1; i++)
                {
                    for (int j = 0; j < (int)msizeSize +1; j++)
                    {
                        iNewX = i;
                        iNewY = j;
                        if (iNewX == (int)msizeSize )
                            iNewX = (int)msizeSize-1;

                        if (iNewY == (int)msizeSize )
                            iNewY  = (int)msizeSize-1;

                        if ((int)TerrainMap.iarrMap[(int)MapLayer.Terrain, iNewY, iNewX] == TerrainIndex)
                            brTerrainFile.Write((byte)0xff);
                        else
                            brTerrainFile.Write((byte)0x00); 
                    }     
                }

                byte bEndOfNextZone=0;
                int iStartCharsIndex = 0;
                //skip to next zone
                do
                {
                    bEndOfNextZone = (byte) brTerrainFile.BaseStream.ReadByte();
                    if (iarrStartSequence[iStartCharsIndex] == bEndOfNextZone)
                        iStartCharsIndex++;
                    else
                        iStartCharsIndex = 0;


                } while (iStartCharsIndex!=ISTARTSEQUENCESIZE);
                //brTerrainFile.Seek(97, SeekOrigin.Current);
                //for (int i = 0; i < 104; i++)
                //{
                //    brTerrainFile.BaseStream.ReadByte();   
                //}
            }
            brTerrainFile.Close();
        }
    }
}
