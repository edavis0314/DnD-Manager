package MyObjects;

import java.util.ArrayList;

public class Feat {
    String Name;
    String Requirement;
    String Discription;
    ArrayList<Effect> EffectList = new ArrayList<>();
    
    public void SetName (String Name){
        this.Name = Name;
    }
    public void SetRequirement (String Requirement){
        this.Requirement = Requirement;
    }
    public void SetDiscription (String Discription){
        this.Discription = Discription;
    }
    public void SetEffect (String Discription, int Slots){
        Effect Effect = new Effect();
        Effect.SetEffect(Discription);
        Effect.SetSlots(Slots);
        this.EffectList.add(Effect);
    }
}
