package MyObjects;

import java.util.ArrayList;

public class Magic {// Set EffectList Contructors
    String Name;
    String CasterType;
    String MagicType;
    String CastingTime;
    String Range;
    String Components;
    String Duration;
    String Saves;
    String Discription;
    int LevelRequirment;
    ArrayList<Effect> EffectList = new ArrayList<>();
    
    public void SetName (String Name){
        this.Name = Name;
    }
    public void SetCasterType (String CasterType){
        this.CasterType = CasterType;
    }
    public void SetMagicType (String MagicType){
        this.MagicType = MagicType;
    }
    public void SetCastingTime (String CastingTime){
        this.CastingTime = CastingTime;
    }
    public void SetRange (String Range){
        this.Range = Range;
    }
    public void SetComponents (String Components){
        this.Components = Components;
    }
    public void SetDuration (String Duration){
        this.Duration = Duration;
    }
    public void SetSaves (String Saves){
        this.Saves = Saves;
    }    
    public void SetDiscription (String Discription){
        this.Discription = Discription;
    }
    public void SetLevelRequirment (int LevelRequirment){
        this.LevelRequirment = LevelRequirment;
    }
}
