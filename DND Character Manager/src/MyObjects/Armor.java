package MyObjects;

import java.util.*;

public class Armor {//Add EffectList Constructors
    String Name;
    String Cost;
    String Stealth;
    String ArmorType;
    String Discription;
    int ArmorRating;
    int StrRequirment;
    int Weight;
    int Amount;
    ArrayList<Effect> EffectList = new ArrayList<>();
    
    public void SetName (String Name){
        this.Name = Name;
    }
    public void SetCost (String Cost){
        this.Cost = Cost;
    }
    public void SetStealth (String Stealth){
        this.Stealth = Stealth;
    }
    public void SetArmorType (String ArmorType){
        this.ArmorType = ArmorType;
    }
    public void SetDiscription (String Discription){
        this.Discription = Discription;
    }
    public void SetArmorRating (int ArmorRating){
        this.ArmorRating = ArmorRating;
    }
    public void SetStrRequirment (int StrRequirment){
        this.StrRequirment = StrRequirment;
    }
    public void SetWeight (int Weight){
        this.Weight = Weight;
    }
    public void SetAmount (int Amount){
        this.Amount = Amount;
    }
    public void SetEffect (String Discription, int Slots){
        Effect Effect = new Effect();
        Effect.SetEffect(Discription);
        Effect.SetSlots(Slots);
        this.EffectList.add(Effect);
    }
}
