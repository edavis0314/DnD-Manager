package dnd.character.manager;

import java.util.*;

public class Character { //object
    int PlayerID = 0;
    int CharacterID = 0;
    String PlayerName = "";
    String CharacterName = "";
    int CharacterLevel = 0;
    double CharacterExperience = 0;
    String Class = "";
    String Background = "";
    double AlignmentGE= 0;
    double AlignmentLC= 0;
    int Str = 0;
    int Dex = 0;
    int Con = 0;
    int Int = 0;
    int Wis = 0;
    int Cha = 0;
    ArrayList<Double> MagicList = new ArrayList<>();
    ArrayList<Double> EquipmentList = new ArrayList<>();
    ArrayList<Double> WeaponList = new ArrayList<>();
    ArrayList<Double> SkillList = new ArrayList<>();
    ArrayList<Double> FeatList = new ArrayList<>();
    
    public Character(){
        
    }
}
