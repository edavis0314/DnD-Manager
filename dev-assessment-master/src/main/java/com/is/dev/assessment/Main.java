package com.is.dev.assessment;

import com.is.dev.assessment.util.*;
import com.is.dev.assessment.domain.*;
import java.io.FileNotFoundException;
import java.util.*;

public class Main {
    public static void main(String[] args) throws FileNotFoundException {
        ArrayList<Product> Inventory = new ArrayList<Product>();
        
        Inventory = ProductCsvUtil.ReadFile();
        ProductJsonUtil.PrintJsonFile(Inventory);
        ProductXmlUtil.PrintXMLFile(Inventory);
    }
}
