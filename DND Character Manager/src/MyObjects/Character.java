package MyObjects;  //Finish the "Set" methods, no other methods have been made.

import java.util.*;

public class Character { //Add ArrayList Constructors
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
    int[] Ability = new int[6];
    int[] AbilityMod = new int[6];
    int[] AbilityProf = new int[6];
    int[] SpellSlots = new int[9];
    int[] SkillMod = new int[18];
    int[] SkillProf = new int[18];
    double[] Money = new double[5];
    ArrayList<Ally> AllyList = new ArrayList<>();
    ArrayList<Magic> MagicList = new ArrayList<>();
    ArrayList<Equipment> EquipmentList = new ArrayList<>();
    ArrayList<Weapon> WeaponList = new ArrayList<>();
    ArrayList<Armor> ArmorList = new ArrayList<>();
    ArrayList<Skill> SkillList = new ArrayList<>();
    ArrayList<Feat> FeatList = new ArrayList<>();
    
    public void SetPlayerID (int PlayerID){
        this.PlayerID = PlayerID;
    }
    public void SetCharacterID (int CharacterID){
        this.CharacterID = CharacterID;
    }
    public void SetCharacterLevel(int CharacterLevel){
        this.CharacterLevel = CharacterLevel;
    }
    public void SetSpeed(int Speed){
        this.Speed = Speed;
    }
    public void SetMaxHP(int MaxHP){
        this.MaxHP = MaxHP;
    }
    public void SetCurrentHP(int CurrentHP){
        this.CurrentHP = CurrentHP;
    }
    public void SetTempHP(int TempHP){
        this.TempHP = TempHP;
    }
    public void SetArmorClass(int ArmorClass){
        this.ArmorClass = ArmorClass;
    }
    public void SetDeathSavesSuc(int DeathSavesSuc){
        this.DeathSavesSuc = DeathSavesSuc;
    }
    public void SetDeathSavesFail(int DeathSavesFail){
        this.DeathSavesFail = DeathSavesFail;
    }
    public void SetSpellSave(int SpellSave){
        this.SpellSave = SpellSave;
    }
    public void SetSpellAttack(int SpellAttack){
        this.SpellAttack = SpellAttack;
    }
    public void SetSpellCastingAbility(int SpellCastingAbility){
        this.SpellCastingAbility = SpellCastingAbility;
    }
    public void SetAge(int Age){
        this.Age = Age;
    }
    public void SetHeight(int Height){
        this.Height = Height;
    }
    public void SetWeight(int Weight){
        this.Weight = Weight;
    }
    public void SetProfBonus(int ProfBonus){
        this.ProfBonus = ProfBonus;
    }
    public void Setinspiration(int inspiration){
        this.inspiration = inspiration;
    }
    public void Setinitiative(int initiative){
        this.initiative = initiative;
    }
    public void SetCharacterExperience(double CharacterExperience){
        this.CharacterExperience = CharacterExperience;
    }
    public void SetPlayerName(String PlayerName){
        this.PlayerName = PlayerName;
    }
    public void SetCharacterName(String CharacterName){
        this.CharacterName = CharacterName;
    }
    public void SetAlignment(String Alignment){
        this.Alignment = Alignment;
    }
    public void SetPersonalityTraits(String PersonalityTraits){
        this.PersonalityTraits = PersonalityTraits;
    }
    public void SetIdeals(String Ideals){
        this.Ideals = Ideals;
    }
    public void SetBonds(String Bonds){
        this.Bonds = Bonds;
    }
    public void SetFlaws(String Flaws){
        this.Flaws = Flaws;
    }
    public void SetClass(String Class){
        this.Class = Class;
    }
    public void SetBackground(String Background){
        this.Background = Background;
    }
    public void SetRace(String Race){
        this.Race = Race;
    }
    public void SetCustomBackstory(String CustomBackstory){
        this.CustomBackstory = CustomBackstory;
    }
    public void SetEyeColor(String EyeColor){
        this.EyeColor = EyeColor;
    }
    public void SetHairColor(String HairColor){
        this.HairColor = HairColor;
    }
    public void SetSkinColor(String SkinColor){
        this.SkinColor = SkinColor;
    }
    //Set Ability Scores
    public void SetStrScore (int AbilityScore){
        this.Ability[0] = AbilityScore;
    }
    public void SetDexScore (int AbilityScore){
        this.Ability[1] = AbilityScore;
    }
    public void SetConScore (int AbilityScore){
        this.Ability[2] = AbilityScore;
    }
    public void SetIntScore (int AbilityScore){
        this.Ability[3] = AbilityScore;
    }
    public void SetWisScore (int AbilityScore){
        this.Ability[4] = AbilityScore;
    }
    public void SetChaScore (int AbilityScore){
        this.Ability[5] = AbilityScore;
    }
    //Set Ability Modifier
    public void SetStrModifier (int AbilityModifier){
        this.AbilityMod[0] = AbilityModifier;
    }
    public void SetDexModifier (int AbilityModifier){
        this.AbilityMod[1] = AbilityModifier;
    }
    public void SetConModifier (int AbilityModifier){
        this.AbilityMod[2] = AbilityModifier;
    }
    public void SetIntModifier (int AbilityModifier){
        this.AbilityMod[3] = AbilityModifier;
    }
    public void SetWisModifier (int AbilityModifier){
        this.AbilityMod[4] = AbilityModifier;
    }
    public void SetChaModifier (int AbilityModifier){
        this.AbilityMod[5] = AbilityModifier;
    }
    //Set Ability Proficiency
    public void SetStrProf (int AbilityProf){
        this.AbilityProf[0] = AbilityProf;
    }
    public void SetDexProf (int AbilityProf){
        this.AbilityProf[1] = AbilityProf;
    }
    public void SetConProf (int AbilityProf){
        this.AbilityProf[2] = AbilityProf;
    }
    public void SetIntProf (int AbilityProf){
        this.AbilityProf[3] = AbilityProf;
    }
    public void SetWisProf (int AbilityProf){
        this.AbilityProf[4] = AbilityProf;
    }
    public void SetChaProf (int AbilityProf){
        this.AbilityProf[5] = AbilityProf;
    }
    //Set Spell Slots
    public void SetFirstLevel (int SpellSlots){
        this.SpellSlots[0] = SpellSlots;
    }
    public void SetSecondLevel (int SpellSlots){
        this.SpellSlots[1] = SpellSlots;
    }
    public void SetThirdLevel (int SpellSlots){
        this.SpellSlots[2] = SpellSlots;
    }
    public void SetFourthLevel (int SpellSlots){
        this.SpellSlots[3] = SpellSlots;
    }
    public void SetFifthLevel (int SpellSlots){
        this.SpellSlots[4] = SpellSlots;
    }
    public void SetSixthLevel (int SpellSlots){
        this.SpellSlots[5] = SpellSlots;
    }
    public void SetSeventhLevel (int SpellSlots){
        this.SpellSlots[6] = SpellSlots;
    }
    public void SetEighthLevel (int SpellSlots){
        this.SpellSlots[7] = SpellSlots;
    }
    public void SetNinethLevel (int SpellSlots){
        this.SpellSlots[8] = SpellSlots;
    }
    //Set Skill Modifier
    public void SetAcrobaticsModifier (int SkillScore){
        this.SkillMod[0] = SkillScore;
    }
    public void SetAnimalHandlingModifier (int SkillScore){
        this.SkillMod[1] = SkillScore;
    }
    public void SetArcanaModifier (int SkillScore){
        this.SkillMod[2] = SkillScore;
    }
    public void SetAthleticsModifier (int SkillScore){
        this.SkillMod[3] = SkillScore;
    }
    public void SetDeceptionModifier (int SkillScore){
        this.SkillMod[4] = SkillScore;
    }
    public void SetHistoryModifier (int SkillScore){
        this.SkillMod[5] = SkillScore;
    }
    public void SetInsightModifier (int SkillScore){
        this.SkillMod[6] = SkillScore;
    }
    public void SetIntimidationModifier (int SkillScore){
        this.SkillMod[7] = SkillScore;
    }
    public void SetInvestigationModifier (int SkillScore){
        this.SkillMod[8] = SkillScore;
    }
    public void SetMedicineModifier (int SkillScore){
        this.SkillMod[9] = SkillScore;
    }
    public void SetNatureModifier (int SkillScore){
        this.SkillMod[10] = SkillScore;
    }
    public void SetPerceptionModifier (int SkillScore){
        this.SkillMod[11] = SkillScore;
    }
    public void SetPerformanceModifier (int SkillScore){
        this.SkillMod[12] = SkillScore;
    }
    public void SetPersuasionModifier (int SkillScore){
        this.SkillMod[13] = SkillScore;
    }
    public void SetReligionModifier (int SkillScore){
        this.SkillMod[14] = SkillScore;
    }
    public void SetSleightOfHanModifier (int SkillScore){
        this.SkillMod[15] = SkillScore;
    }
    public void SetStealthModifier (int SkillScore){
        this.SkillMod[16] = SkillScore;
    }
    public void SetSurvivalModifier (int SkillScore){
        this.SkillMod[17] = SkillScore;
    }
    //Set Skill Proficiency
    public void SetAcrobaticsProf (int SkillProf){
        this.SkillProf[0] = SkillProf;
    }
    public void SetAnimalHandlingProf (int SkillProf){
        this.SkillProf[1] = SkillProf;
    }
    public void SetArcanaProf (int SkillProf){
        this.SkillProf[2] = SkillProf;
    }
    public void SetAthleticsProf (int SkillProf){
        this.SkillProf[3] = SkillProf;
    }
    public void SetDeceptionProf (int SkillProf){
        this.SkillProf[4] = SkillProf;
    }
    public void SetHistoryProf (int SkillProf){
        this.SkillProf[5] = SkillProf;
    }
    public void SetInsightProf (int SkillProf){
        this.SkillProf[6] = SkillProf;
    }
    public void SetIntimidationProf (int SkillProf){
        this.SkillProf[7] = SkillProf;
    }
    public void SetInvestigationProf (int SkillProf){
        this.SkillProf[8] = SkillProf;
    }
    public void SetMedicineProf (int SkillProf){
        this.SkillProf[9] = SkillProf;
    }
    public void SetNatureProf (int SkillProf){
        this.SkillProf[10] = SkillProf;
    }
    public void SetPerceptionProf (int SkillProf){
        this.SkillProf[11] = SkillProf;
    }
    public void SetPerformanceProf (int SkillProf){
        this.SkillProf[12] = SkillProf;
    }
    public void SetPersuasionProf (int SkillProf){
        this.SkillProf[13] = SkillProf;
    }
    public void SetReligionProf (int SkillProf){
        this.SkillProf[14] = SkillProf;
    }
    public void SetSleightOfHandProf (int SkillProf){
        this.SkillProf[15] = SkillProf;
    }
    public void SetStealthProf (int SkillProf){
        this.SkillProf[16] = SkillProf;
    }
    public void SetSurvivalProf (int SkillProf){
        this.SkillProf[17] = SkillProf;
    }
    //Set Money
    public void SetCopper (int Coins){
        this.Money[0] = Coins;
    }
    public void SetSilver (int Coins){
        this.Money[1] = Coins;
    }
    public void SetElectrum (int Coins){
        this.Money[2] = Coins;
    }
    public void SetGold (int Coins){
        this.Money[3] = Coins;
    }
    public void SetPlatnium (int Coins){
        this.Money[4] = Coins;
    }
}