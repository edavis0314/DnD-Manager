package dndcharactermanager;

import java.sql.*;
import java.util.*;
import MyObjects.*;

public class Testing {
    public static void main() {
        String check = "";
        int sessionNumber;
        Scanner scanner = new Scanner(System.in);
        Random rand = new Random();
        
        sessionNumber = rand.nextInt(1000000);
        do{
            System.out.println("Welcom to the testing module. Please choose one option from the following: ");
            System.out.println("Type 'RNG')     To Test the Random Number Generator ");           
            System.out.println("Type 'DB')      To Test Database ");
            //System.out.println("Type '') ");
            
            System.out.println("Type 'Quit')    To Close Testing\n");
            
            check = scanner.nextLine();
            System.out.println("");
            
            if(check.equals("RNG")){
                do{
                    System.out.println("There are a few options for RNG. Please choose one option from the following: ");
                    System.out.println("Type 'One') If Multiple Rolls");
                    System.out.println("Type 'Two') If Multiple Rolls - One is the lowest number on the die");
                    System.out.println("Type 'Three') If only one roll");
                    System.out.println("Type 'Four') If only one roll - One is the lowest number on the die.");
                    System.out.println("Type 'Five') If One six sided die.");
                    System.out.println("Type 'Back') To go back one page\n");
                    check = scanner.nextLine();
                    System.out.println("");

                    if(check.equals("One")){
                        RNGOne(sessionNumber);
                    } else if (check.equals("Two")){
                        RNGTwo(sessionNumber);
                    } else if (check.equals("Three")){
                        RNGThree();
                    } else if (check.equals("Four")){
                        RNGFour();
                    } else if (check.equals("Five")){
                        RNGFive();
                    } else if (check.equals("Back")){
                        System.out.println("Leaving RNG");
                    } else {
                        System.out.println("Error. Not a Choice.");
                    }
                } while(!(check.equals("Back")));
                
                System.out.println("");
            }
            
            else if(check.equals("DB")){
                do{
                    System.out.println("There are a few options for DB. Please choose one option from the following: ");
                    System.out.println("Type 'One') Check and see if you are able to connect to SQL Workbench");
                    System.out.println("Type 'Two') Check and see if you are able to create a Table in SQL Workbench");
                    System.out.println("Type 'Three') Check and see if you are able to Drop a Table in SQL Workbench");
                    System.out.println("Type 'Back') To go back one page\n");
                    check = scanner.nextLine();
                    System.out.println("");

                    if(check.equals("One")){
                        DBOne();
                    } else if (check.equals("Two")){
                        DBTwo();
                    } else if (check.equals("Three")){
                        DBThree();
                    } else {
                        System.out.println("Error. Not a Choice.");
                    }
                } while(!(check.equals("Back")));
                
                System.out.println("");
            }
            
        }while(!(check.equals("Quit")));
        
    }   
    
    static void RNGOne(int sessionNumber){
        int check, MaxSides, MinSides, PlayerID, CharacterID, NumberOfDie;
        int RollCount = 0;
        Scanner scanner = new Scanner(System.in);
        Random rand = new Random();
        
        do{
            PlayerID = rand.nextInt(1000000);
            System.out.print("Please Enter the number of trials you would like?  ");
            check = scanner.nextInt();
            System.out.print("Please Enter the Lower Bound?  ");
            MinSides = scanner.nextInt();
            System.out.print("Please Enter the Upper Bound?  ");
            MaxSides = scanner.nextInt();
            System.out.print("Please Enter the number of die being used per run?  ");
            NumberOfDie = scanner.nextInt();
            if(check != 0 || MaxSides != 0){
                int Outcome;

                System.out.println("    Thank You    ");
                
                for(int n=0; n<check;n++){
                    CharacterID = rand.nextInt(1000000);
                    Outcome = RandomNumberGenerator.Roll(CharacterID, PlayerID, sessionNumber, RollCount, MaxSides, MinSides, NumberOfDie);
                    if(Outcome != 0){
                        System.out.print(Outcome + "   ");
                    }
                    RollCount = RollCount + NumberOfDie;
                    if((n%6) == 5){System.out.print("\n");}
                }
                System.out.println("    Trial Complete    ");
            }
            else{
                System.out.println("    Closing Program    \n");
            }
        }while(check != 0);
    }
    
    static void RNGTwo(int sessionNumber){
        int check, MaxSides, PlayerID, CharacterID, NumberOfDie;
        int RollCount = 0;
        Scanner scanner = new Scanner(System.in);
        Random rand = new Random();
        
        do{
            PlayerID = rand.nextInt(1000000);
            System.out.print("Please Enter the number of trials you would like?  ");
            check = scanner.nextInt();
            System.out.print("Please Enter the type of die being used?  ");
            MaxSides = scanner.nextInt();
            System.out.print("Please Enter the number of die being used per run?  ");
            NumberOfDie = scanner.nextInt();
            if(check != 0 || MaxSides != 0){
                int Outcome;

                System.out.println("    Thank You    ");
                
                for(int n=0; n<check;n++){
                    CharacterID = rand.nextInt(1000000);
                    Outcome = RandomNumberGenerator.Roll(CharacterID, PlayerID, sessionNumber, RollCount, MaxSides, NumberOfDie);
                    if(Outcome != 0){
                        System.out.print(Outcome + "   ");
                    }
                    RollCount = RollCount + NumberOfDie;
                    if((n%6) == 5){System.out.print("\n");}
                }
                System.out.println("    Trial Complete    ");
            }
            else{
                System.out.println("    Closing Program    \n");
            }
        }while(check != 0);
    }
    
    static void RNGThree(){
        int check, MinSides, MaxSides;
        Scanner scanner = new Scanner(System.in);
        do{
            System.out.print("Please Enter the number of trials you would like?  ");
            check = scanner.nextInt();
            System.out.print("Please Enter the Lower Bound?  ");
            MinSides = scanner.nextInt();
            System.out.print("Please Enter the Upper Bound?  ");
            MaxSides = scanner.nextInt();
            if(check != 0 || MinSides != 0){
                int Outcome;

                System.out.println("    Thank You    ");
                
                for(int n=0; n<check;n++){
                    Outcome = RandomNumberGenerator.Roll(MaxSides, MinSides);
                    if(Outcome != 0){
                        System.out.print(Outcome + "   ");
                    }
                }
                System.out.println("    Trial Complete    ");
            }
            else{
                System.out.println("    Closing Program    \n");
            }
        }while(check != 0);
    }
    
    static void RNGFour(){
        int check, MaxSides;
        Scanner scanner = new Scanner(System.in);
        do{
            System.out.print("Please Enter the number of trials you would like?  ");
            check = scanner.nextInt();
            System.out.print("Please Enter the type of die being used?  ");
            MaxSides = scanner.nextInt();
            if(check != 0 || MaxSides != 0){
                int Outcome;

                System.out.println("    Thank You    ");
                
                for(int n=0; n<check;n++){
                    Outcome = RandomNumberGenerator.Roll(MaxSides);
                    if(Outcome != 0){
                        System.out.print(Outcome + "   ");
                    }
                }
                System.out.println("    Trial Complete    ");
            }
            else{
                System.out.println("    Closing Program    \n");
            }
        }while(check != 0);
    }
    
    static void RNGFive(){
        int check;
        Scanner scanner = new Scanner(System.in);
        do{
            System.out.print("Please Enter the number of trials you would like?  ");
            check = scanner.nextInt();
            if(check != 0){
                int Outcome;

                System.out.println("    Thank You    ");

                for(int n=0; n<check;n++){
                    Outcome = RandomNumberGenerator.Roll();
                    if(Outcome != 0){
                        System.out.print(Outcome + "   ");
                    }
                }
                System.out.println("    Trial Complete    ");
            }
            else{
                System.out.println("    Closing Program    \n");
            }
        }while(check != 0);
    }
    
    static void DBOne(){
        Scanner read = new Scanner(System.in);
        String username = "";//root
        String password = "";//computer's password
        
        System.out.println("This will connect to your local host!!!\nEnter the Username:");           
        username = read.nextLine();
        System.out.println("Enter your computer's password:");
        password = read.nextLine();
        System.out.println("Testing Connection........");
        if(!(username.equals("")) && !(password.equals(""))){
            Database.ConnectToDatabase(username, password);
        } else {
            System.out.println("Failed Login");
        }
    }
    
    static void DBTwo(){
        String username = "root";//root
        String password = "intelCORE_03141996";//computer's password
        
        System.out.println("Testing Connection........");
        try {
            Database.CreateDatabase(username, password);
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString());
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString()); 
        } 
    }
    
    static void DBThree(){
        String username = "root";//root
        String password = "intelCORE_03141996";//computer's password
        
        System.out.println("Testing Connection........");
        try {
            Database.DeleteDatabase(username, password);
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString());
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString()); 
        } 
    }
}