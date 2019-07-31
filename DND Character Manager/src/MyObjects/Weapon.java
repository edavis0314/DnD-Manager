package MyObjects;

import java.util.*;

public class Weapon {//Add EffectList Constructors
    String Name;
    String Cost;
    String WeaponType;
    String DamageType;
    String MeleeRange;
    String DamageDice;
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
    public void SetWeaponType (String WeaponType){
        this.WeaponType = WeaponType;
    }
    public void SetDamageType (String DamageType){
        this.DamageType = DamageType;
    }
    public void SetMeleeRange (String MeleeRange){
        this.MeleeRange = MeleeRange;
    }
    public void SetDamageDice (String DamageDice){
        this.DamageDice = DamageDice;
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
    public void SetEffect (String Discription, int Slots){
        Effect Effect = new Effect();
        Effect.SetEffect(Discription);
        Effect.SetSlots(Slots);
        this.EffectList.add(Effect);
    }
}
