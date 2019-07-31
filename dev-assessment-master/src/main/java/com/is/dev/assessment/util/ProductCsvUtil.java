package com.is.dev.assessment.util;

import com.is.dev.assessment.domain.*;
import java.util.*;
import java.io.*;

public class ProductCsvUtil {//This will contain all the logic for the .tsv and .csv files
    public static ArrayList<Product> ReadFile(){
        File file = new File("..\\dev-assessment-master\\products.csv");
        ArrayList<Product> Inventory = new ArrayList<Product>();
        String CurrentLine;
        
        try {
            Scanner fileScan = new Scanner(file);
            CurrentLine = (fileScan.nextLine());
            while (fileScan.hasNext()){
                CurrentLine = (fileScan.nextLine());
                Inventory = StoreDataIntoObject(44,CurrentLine,Inventory);
            }
            file = new File("..\\dev-assessment-master\\products.tsv");
            fileScan = new Scanner(file);
            CurrentLine = (fileScan.nextLine());
            while (fileScan.hasNext()){
                CurrentLine = (fileScan.nextLine());
                Inventory = StoreDataIntoObject(9,CurrentLine,Inventory);
            }
            
            fileScan.close();

        } catch(FileNotFoundException fnfe) {
            System.out.println("FileNotFoundException thrown:");
            System.out.println("[ERROR] File not found");
            System.exit(0);
	}
        
        return Inventory;
    }
    
    public static ArrayList<Product> StoreDataIntoObject(int delimitor, String LineInput, ArrayList<Product> Inventory){
        Product Item = new Product();
        String tempString = "", checkString = "";
        char CurrentLocation;
        int ascii;
        int Quote = 0, count = 0, checkValue = 0;
        
        LineInput = LineInput + " ";
        
        for (int n=0; n < LineInput.length(); n++){
            CurrentLocation = LineInput.charAt(n);
            ascii = (int)CurrentLocation;
            if (ascii == 34){
                if(Quote == 0){Quote = 1;}
                else if(Quote == 1){Quote = 0;}
                
                if(ascii != 34){
                    tempString += CurrentLocation;
                }
            }
            else if((ascii == delimitor || (n+1 == LineInput.length())) && Quote == 0){
                if (count == 0){
                    Item.StoreTitle(tempString);
                    tempString = "";
                } else if (count == 1){
                    Item.StoreUPC(tempString);
                    tempString = "";
                } else if (count == 2){
                    checkValue=CheckSKU(tempString,Inventory);
                    if(checkValue==1){
                        checkString = tempString;
                    }
                    Item.StoreSKU(tempString);
                    tempString = "";
                } else if (count == 3){
                    Item.StorePrice(tempString);
                    tempString = "";
                } else if (count == 4){
                    Item.StoreCondition(tempString);
                    tempString = "";
                } else if (count == 5){
                    if(checkValue==1){
                        UpdateQuantity(checkString,Inventory,tempString);
                    }
                    Item.StoreQuantity(tempString);
                    tempString = "";
                } else {}
                count++;
            }
            else{
                tempString += CurrentLocation;
            }
        }
        if(checkValue==0){
            Inventory.add(Item);
        }

        return Inventory;
    }
    
    public static int CheckSKU(String SKU, ArrayList<Product> Inventory){
        int CheckValue = 0;
        if(!(Inventory.isEmpty())){
            for(int n=0; n<Inventory.size();n++){
                if(Inventory.get(n).PrintSKU().equals(SKU)){
                    CheckValue = 1;
                }
            }
        }
        return CheckValue;
    }
    
    public static void UpdateQuantity(String SKU, ArrayList<Product> Inventory, String Quantity){
        int NewSum = 0;
        if(!(Inventory.isEmpty())){
            for(int n=0; n<Inventory.size();n++){
                if(Inventory.get(n).PrintSKU().equals(SKU)){
                    NewSum = Integer.valueOf(Inventory.get(n).PrintQuantity()) + Integer.valueOf(Quantity);
                    Inventory.get(n).StoreQuantity(Integer.toString(NewSum));
                }
            }
        }
    }
}
