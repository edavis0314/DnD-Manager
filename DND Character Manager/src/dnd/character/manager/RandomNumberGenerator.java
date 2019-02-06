package dnd.character.manager;

import java.util.*;

public class RandomNumberGenerator {
    
    public static int Roll(int CharacterID, int PlayerID, int SessionNumber, int RollCount, int Side_Max, int Side_Min, int NumberofDie){//If Multiple Rolls
        Random rand = new Random();
        int Outcome, temp, valueTotal, difference;   
        Outcome = 0;
        difference = Side_Max - Side_Min + 1;
        
        for (int i = 0; i < NumberofDie; i++) {
            temp = rand.nextInt();
            valueTotal = (((CharacterID ^ (PlayerID % SessionNumber) % temp) ^ RollCount)) % difference;
            valueTotal = valueTotal + Side_Min;
                    
            Outcome = Outcome + valueTotal;
            RollCount++;
        }
        return Outcome;
    }
    
    public static int Roll(int CharacterID, int PlayerID, int SessionNumber, int RollCount, int Side_Max, int NumberofDie){//If Multiple Rolls - One is the lowest number on the die
        Random rand = new Random();
        int Outcome, temp, valueTotal;   
        Outcome = 0;
        
        for (int i = 0; i < NumberofDie; i++) {
            temp = rand.nextInt();
            valueTotal = (((CharacterID ^ (PlayerID % SessionNumber) % temp) ^ RollCount)) % Side_Max;
            valueTotal = valueTotal + 1;
                    
            Outcome = Outcome + valueTotal;
            RollCount++;
        }
        return Outcome;
    }
    
    public static int Roll(int Side_Max, int Side_Min){//If only one roll
        Random rand = new Random();
        int Outcome, temp, difference;   
        
        difference = Side_Max - Side_Min;
        temp = rand.nextInt(difference);
        Outcome = temp + Side_Min;
        
        return Outcome;
    }

    public static int Roll(int Side_Max){//If only one roll - One is the lowest number on the die.
        Random rand = new Random();
        int Outcome, temp;   
        
        temp = rand.nextInt(Side_Max);
        Outcome = temp + 1;
        
        return Outcome;
    }
    
    public static int Roll(){//If One six sided die.
        Random rand = new Random();
        int Outcome, temp;   
        
        temp = rand.nextInt(6);
        Outcome = temp + 1;
        
        return Outcome;
    }
}