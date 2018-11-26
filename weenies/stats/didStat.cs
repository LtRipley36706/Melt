using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Melt
{
    public enum eDidStat
    {
        Undef = 0,
        Setup = 1,
        MotionTable = 2,
        SoundTable = 3,
        CombatTable = 4,
        QualityFilter = 5,
        PaletteBase = 6,
        ClothingBase = 7,
        Icon = 8,
        EyesTexture = 9,
        NoseTexture = 10,
        MouthTexture = 11,
        DefaultEyesTexture = 12,
        DefaultNoseTexture = 13,
        DefaultMouthTexture = 14,
        HairPalette = 15,
        EyesPalette = 16,
        SkinPalette = 17,
        HeadObject = 18,
        ActivationAnimation = 19,
        InitMotion = 20,
        ActivationSound = 21,
        PhysicsEffectTable = 22,
        UseSound = 23,
        UseTargetAnimation = 24,
        UseTargetSuccessAnimation = 25,
        UseTargetFailureAnimation = 26,
        UseUserAnimation = 27,
        Spell = 28,
        SpellComponent = 29,
        PhysicsScript = 30,
        LinkedPortalOne = 31,
        WieldedTreasureType = 32,
        UnknownGuessedname = 33,
        UnknownGuessedname2 = 34,
        DeathTreasureType = 35,
        MutateFilter = 36,
        ItemSkillLimit = 37,
        UseCreateItem = 38,
        DeathSpell = 39,
        VendorsClassId = 40,
        ItemSpecializedOnly = 41,
        HouseId = 42,
        AccountHouseId = 43,
        RestrictionEffect = 44,
        CreationMutationFilter = 45,
        TsysMutationFilter = 46,
        LastPortal = 47,
        LinkedPortalTwo = 48,
        OriginalPortal = 49,
        IconOverlay = 50,
        IconOverlaySecondary = 51,
        IconUnderlay = 52,
        AugmentationMutationFilter = 53,
        AugmentationEffect = 54,
        ProcSpell = 55,
        AugmentationCreateItem = 56,
        AlternateCurrency = 57,
        BlueSurgeSpell = 58,
        YellowSurgeSpell = 59,
        RedSurgeSpell = 60,
        OlthoiDeathTreasureType = 61
    }

    public struct sDidStat
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public eDidStat key;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int value;

        public sDidStat(eDidStat key, int value)
        {
            this.key = key;
            this.value = value;
        }

        public sDidStat(byte[] buffer, StreamReader inputFile)
        {
            key = (eDidStat)Utils.ReadInt32(buffer, inputFile);

            if (!Enum.IsDefined(typeof(eDidStat), key))
                Console.WriteLine("Unknown didStat: {0}", key);

            value = Utils.ReadInt32(buffer, inputFile);
        }

        public void writeRaw(StreamWriter outputStream)
        {
            Utils.writeInt32((int)key, outputStream);
            Utils.writeInt32(value, outputStream);
        }

        public void writeJson(StreamWriter outputStream, string tab, bool isFirst)
        {
            string entryStarter = isFirst ? "" : ",";
            outputStream.Write("{0}\n{1}{{", entryStarter, tab);

            Utils.writeJson(outputStream, "key", (int)key, "", true, false, 3);
            Utils.writeJson(outputStream, "value", value, "    ", false, false, 10);
            switch (key)
            {
                case eDidStat.PhysicsScript:
                    {
                        Utils.writeJson(outputStream, "_comment", $"{key.ToString()} = {((ePScriptType)value).ToString()}", "    ", false, false, 0);
                        break;
                    }
                case eDidStat.BlueSurgeSpell:
                case eDidStat.RedSurgeSpell:
                case eDidStat.YellowSurgeSpell:
                case eDidStat.ProcSpell:
                case eDidStat.DeathSpell:
                case eDidStat.Spell:
                    {
                        Utils.writeJson(outputStream, "_comment", $"{key.ToString()} = {SpellInfo.getSpellName(value)}", "    ", false, false, 0);
                        break;
                    }
                case eDidStat.DeathTreasureType:
                    {
                        Utils.writeJson(outputStream, "_comment", $"{key.ToString()} = {((eTreasureGeneratorType)value).ToString()}", "    ", false, false, 0);
                        break;
                    }
                case eDidStat.ItemSkillLimit:
                    {
                        Utils.writeJson(outputStream, "_comment", $"{key.ToString()} = {((eSkills)value).ToString()}", "    ", false, false, 0);
                        break;
                    }
                default:
                    {
                        Utils.writeJson(outputStream, "_comment", key.ToString(), "    ", false, false, 0);
                        break;
                    }
            }
            outputStream.Write("}");
        }
    }
}