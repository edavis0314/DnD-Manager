package com.is.dev.assessment.util;

import com.is.dev.assessment.domain.*;
import java.util.*;
import java.io.*;

public class ProductXmlUtil {
    public static void PrintXMLFile(ArrayList<Product> Inventory) throws FileNotFoundException {
        File file = new File("..\\dev-assessment-master\\Inventory.xml");
        BufferedWriter Writer = new BufferedWriter(new PrintWriter("..\\dev-assessment-master\\Inventory.xml"));
        int Quote = 34;
        
        try {
            Writer.write("<Inventory>");
            for(int n=0; n<Inventory.size(); n++){
                Writer.write("<Product>");
                
                Writer.write("<Title>" + Inventory.get(n).PrintTitle() +"</Title>");
                Writer.write("<UPC>" + Inventory.get(n).PrintUPC() + "</UPC>");
                Writer.write("<SKU>" + Inventory.get(n).PrintSKU() + "</SKU>");
                Writer.write("<Price>" + Inventory.get(n).PrintPrice() + "</Price>");
                Writer.write("<Condition>" + Inventory.get(n).PrintCondition() + "</Condition>");
                Writer.write("<Quantity>" + Inventory.get(n).PrintQuantity() + "</Quantity>");
                
                Writer.write("</Product>");
                
            }
            Writer.write("</Inventory>");
            
            Writer.close();
        } catch (IOException ex) {
            System.out.println("Error in the Write to Json File.");
        }
    }
}
