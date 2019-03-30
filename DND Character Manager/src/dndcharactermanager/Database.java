package dndcharactermanager;

import java.sql.*;
import MyObjects.*;

public class Database {
    
    public static void ConnectToDatabase (String username, String password) {
        try {
            Class.forName("com.mysql.jdbc.Driver");
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Change password for local server to use new computer
            Connection con = DriverManager.getConnection(connectionUrl);
        }  catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString() + " \n");
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString() +"\n");
        }   
    }    
    
    public static void CreateDatabase(String username, String password) throws SQLException, ClassNotFoundException{
        try {
            Class.forName("com.mysql.jdbc.Driver");
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Update the Structure to the ER I am going to use
            Connection con = DriverManager.getConnection(connectionUrl);
            Statement statement = con.createStatement();
            statement.executeUpdate("create database CharacterManager;");
            statement.executeUpdate("use CharacterManager;");
            //Add the code back after you get everything to work correctly in SQL workbench
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString() + "\n");
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString() + "\n");
        }        
    }
    
    public static void DeleteDatabase(String username, String password) throws SQLException, ClassNotFoundException{
        try {
            Class.forName("com.mysql.jdbc.Driver");
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password;
            Connection con = DriverManager.getConnection(connectionUrl);
            Statement statement = con.createStatement();
            statement.executeUpdate("drop database CharacterManager;");
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString() + "\n");
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString() + "\n");
        }
    }
}