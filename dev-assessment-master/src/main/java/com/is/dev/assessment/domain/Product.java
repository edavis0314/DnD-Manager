package com.is.dev.assessment.domain;

public class Product {
    String Title;
    String UPC;
    String SKU ;
    String Price;
    String Condition;
    String Quantity;
    
    public Product(){}
    
    @Override
    public String toString(){
        return (this.PrintTitle() + " " + this.PrintUPC() + " " + this.PrintSKU() + " " + this.PrintPrice() + " " + this.PrintCondition() + " " + this.PrintQuantity());
    }
    
    public void StoreTitle(String TitleInput){
        this.Title = TitleInput;
    }
    
    public void StoreUPC(String UPCInput){
        this.UPC = UPCInput;
    }
    
    public void StoreSKU(String SKUInput){
        this.SKU = SKUInput;
    }
    
    public void StorePrice(String priceInput){
        this.Price = priceInput;
    }
    
    public void StoreCondition(String conditionInput){
        this.Condition = conditionInput;
    }
    
    public void StoreQuantity(String QuantityInput){
        this.Quantity = QuantityInput;
    }

    //The Following methods are being used for checking the data is being stored correctly into the object.
    public String PrintTitle(){
        return Title;
    }

    public String PrintUPC(){
        return UPC;
    }
    
    public String PrintSKU(){
        return SKU;
    }
    
    public String PrintPrice(){
        return Price;
    }
    
    public String PrintCondition(){
        return Condition;
    }
    
    public String PrintQuantity(){
        return Quantity;
    }
}