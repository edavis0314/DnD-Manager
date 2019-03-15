package MyObjects;

import java.util.*;

public class Character { //object
    int PlayerID;
    int CharacterID;
    int CharacterLevel;
    int Speed;
    int MaxHP;
    int CurrentHP;
    int TempHP;
    int ArmorClass;
    int DeathSavesSuc;
    int DeathSavesFail;
    int SpellSave;
    int SpellAttack;
    int SpellCastingAbility;
    int Age;
    int Height;
    int Weight;
    int ProfBonus;
    int inspiration;
    int initiative;
    double CharacterExperience;
    String PlayerName;
    String CharacterName;
    String Alignment;
    String PersonalityTraits;
    String Ideals;
    String Bonds;
    String Flaws;
    String Class;
    String Background;
    String Race;
    String CustomBackstory;
    String EyeColor;
    String HairColor;
    String SkinColor;
    int[] Ability = new int[6];//Ability Scores
    int[] AbilityMod = new int[6];
    int[] AbilitySav = new int[6];
    int[] AbilityProf = new int[6];
    int[] SpellSlots = new int[9];
    int[] SkillMod = new int[18];
    int[] SkillProf = new int[18];
    double[] Money = new double[5];
    ArrayList<Character> AllyList = new ArrayList<>();
    ArrayList<LangAndProf> LangAndProfList = new ArrayList<>();
    ArrayList<Magic> MagicList = new ArrayList<>();
    ArrayList<Equipment> EquipmentList = new ArrayList<>();
    ArrayList<Weapon> WeaponList = new ArrayList<>();
    ArrayList<Skill> SkillList = new ArrayList<>();
    ArrayList<Feat> FeatList = new ArrayList<>();
    
    public Character(){
        
    }
}
