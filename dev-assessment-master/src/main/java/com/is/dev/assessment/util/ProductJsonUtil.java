package com.is.dev.assessment.util;

import com.is.dev.assessment.domain.*;
import java.util.*;
import java.io.*;

public class ProductJsonUtil {
    public static void PrintJsonFile(ArrayList<Product> Inventory) throws FileNotFoundException {
        File file = new File("..\\dev-assessment-master\\Inventory.json");
        BufferedWriter Writer = new BufferedWriter(new PrintWriter("..\\dev-assessment-master\\Inventory.json"));
        int Quote = 34;
        
        try {
            Writer.write("[");
            for(int n=0; n<Inventory.size(); n++){
                Writer.write("{");
                
                Writer.write("\"Title\": " + (char)Quote + Inventory.get(n).PrintTitle() + (char)Quote + ",\n");
                Writer.write("\"UPC\": " + Inventory.get(n).PrintUPC() + ",");
                Writer.write("\"SKU\": " + (char)Quote + Inventory.get(n).PrintSKU() + (char)Quote + ",");
                Writer.write("\"Price\": " + Inventory.get(n).PrintPrice() + ",");
                Writer.write("\"Condition\": " + (char)Quote + Inventory.get(n).PrintCondition() + (char)Quote + ",");
                Writer.write("\"Quantity\": " + Inventory.get(n).PrintQuantity());
                
                if((n+1)==Inventory.size()){
                    Writer.write("}");
                } else {
                    Writer.write("},");
                }
            }
            Writer.write("]");
            
            Writer.close();
        } catch (IOException ex) {
            System.out.println("Error in the Write to Json File.");
        }
    }
}
