package MyObjects;

import java.util.*;

public class Equipment {//Add EffectList Constructors
    String Name;
    String Cost;
    String Discription;
    int Weight;
    int Amount;
    ArrayList<Effect> EffectList = new ArrayList<>();
    
    public void SetName (String Name){
        this.Name = Name;
    }
    public void SetCost (String Cost){
        this.Cost = Cost;
    }
    public void SetDiscription (String Discription){
        this.Discription = Discription;
    }
    public void SetWeight (int Weight){
        this.Weight = Weight;
    }
    public void SetAmount (int Amount){
        this.Amount = Amount;
    }
}
