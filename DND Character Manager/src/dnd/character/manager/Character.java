package dnd.character.manager;

import java.util.*;

public class Character { //object
    int PlayerID = 0;
    int CharacterID = 0;
    int CharacterLevel = 0;
    int Speed = 0;
    int MaxHP = 0;
    int CurrentHP = 0;
    int TempHP = 0;
    int ArmorClass = 0;
    int DeathSavesSuc = 0;
    int DeathSavesFail = 0;
    int SpellSave = 0;
    int SpellAttack = 0;
    int SpellCastingAbility = 0;
    int Age = 0;
    int Height = 0;
    int Weight = 0;
    int ProfBonus = 0;
    int inspiration = 0;
    int initiative = 0;
    double CharacterExperience = 0;
    String PlayerName = "";
    String CharacterName = "";
    String Alignment= "";
    String PersonalityTraits = "";
    String Ideals = "";
    String Bonds = "";
    String Flaws = "";
    String Class = "";
    String Background = "";
    String Race = "";
    String CustomBackstory = "";
    String EyeColor = "";
    String HairColor = "";
    String SkinColor = "";
    int[] Ability = new int[6];
    int[] AbilityMod = new int[6];
    int[] AbilitySav = new int[6];
    int[] AbilityProf = new int[6];
    int[] SpellSlots = new int[9];
    int[] SkillMod = new int[18];
    int[] SkillProf = new int[18];
    double[] Money = new double[5];
    ArrayList<Integer> AttackList = new ArrayList<>();
    ArrayList<Integer> AllyList = new ArrayList<>();
    ArrayList<Integer> LangAndProfList = new ArrayList<>();
    ArrayList<Integer> MagicList = new ArrayList<>();
    ArrayList<Integer> EquipmentList = new ArrayList<>();
    ArrayList<Integer> WeaponList = new ArrayList<>();
    ArrayList<Integer> SkillList = new ArrayList<>();
    ArrayList<Integer> FeatList = new ArrayList<>();
    
    public Character(){
        
    }
}
