package dnd.character.manager;

import java.sql.*;
import java.util.*;

public class CharacterManager {
    
    public static int Check(){
        ArrayList<Double> CharacterID = new ArrayList<>();
        ArrayList<Double> PlayerID = new ArrayList<>();
        ArrayList<Double> CharacterName = new ArrayList<>();
        ArrayList<Double> PlayerName = new ArrayList<>();  
        
        return 0;
    }
    
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
            statement.executeUpdate("Create Table Movie"
                    + "(Movie_ID int, "
                    + "Movie_Title Varchar (150), "
                    + "Release_Date Varchar (12), "
                    + "Movie_Popularity Float, "
                    + "Average_Votes Float, "
                    + "Total_Votes int, "
                    + "Overview Varchar(10000), "
                    + "Tagline Varchar(2000), "
                    + "Runtime int, "
                    + "Primary Key(Movie_ID));");
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
            statement.executeUpdate("use CharacterManager;");
            statement.execute("drop database CharacterManager;");
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString() + "\n");
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString() + "\n");
        }
    }
    
}