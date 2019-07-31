package finalprojectdm;

import java.util.*;
import java.io.*;
import java.sql.*;
import org.json.simple.*;
import org.json.simple.parser.*;


public class FinalProjectDM {
    public static ArrayList<Long> Genres = new ArrayList<>();
    public static ArrayList<Long> ID = new ArrayList<>();
    public static ArrayList<Long> Keywords = new ArrayList<>();
    public static ArrayList<String> Overview = new ArrayList<>();
    public static ArrayList<Double> Popularity = new ArrayList<>();
    public static ArrayList<Long> ProductionHouse = new ArrayList<>();
    public static ArrayList<String> ReleaseDate = new ArrayList<>();
    public static ArrayList<Long> RunTime = new ArrayList<>();
    public static ArrayList<String> Tag_Line = new ArrayList<>();
    public static ArrayList<String> MovieTitle = new ArrayList<>();
    public static ArrayList<Double> Vote_adv = new ArrayList<>();
    public static ArrayList<Long> Vote_total = new ArrayList<>();
    public static int Counter = 0;
    public static String username;
    public static String password;
    public static String MovieName;
    public static int MovieIDTemp;
    public static int check = 0; 

    public static void main(String[] args)  throws SQLException, ClassNotFoundException, org.json.simple.parser.ParseException, java.text.ParseException, FileNotFoundException, IOException{
        Scanner read = new Scanner(System.in);
        try {
            Login();
            System.out.println("Do you want to create and fill the table?\nIf yes type '1', if no type anything in.");
            check = read.nextInt();
            if(check == 1){
                Create();
                FillTable();
            }
            quaryStart();
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString());
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString()); 
        } catch (FileNotFoundException eA) {
            System.out.println("File Not Found. Exiting Program." + eA.toString());
        } catch (IOException eO) {
            System.out.println("File Not Found. Exiting Program." + eO.toString());
        } catch (org.json.simple.parser.ParseException eW) {
            System.out.println("File Not Parsed. Exiting Program." + eW.toString());
        }  
    }
    
    public static void Login() {
        Scanner read = new Scanner(System.in);
        try {
            System.out.println("This will connect to your local host!!!\nEnter the Username:");           
            username = read.nextLine();
            System.out.println("Enter the password:");
            password = read.nextLine();
            System.out.println("Testing Connection........");
            Class.forName("com.mysql.jdbc.Driver");
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Change password for local server to use new computer
            Connection con = DriverManager.getConnection(connectionUrl);
        }  catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString() + "Failed to ");
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString());
        }   
    }
    
    public static void Create() throws SQLException, ClassNotFoundException{
        try {
            Class.forName("com.mysql.jdbc.Driver");
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Change password for local server to use new computer
            Connection con = DriverManager.getConnection(connectionUrl);
            Statement statement = con.createStatement();
            statement.executeUpdate("create database FinalProject;");
            statement.executeUpdate("use FinalProject;");
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
            statement.executeUpdate("Create Table GenreID"
                    + "(Movie_ID int, "
                    + "Genre_ID int, "
                    + "Foreign Key(Movie_ID) references Movie (Movie_ID));");
            statement.executeUpdate("Create Table KeywordID("
                    + "Movie_ID int, "
                    + "Keyword_ID int, "
                    + "Foreign Key(Movie_ID) references Movie (Movie_ID));");
            statement.executeUpdate("Create Table KeywordsID_2("
                    + "Movie_ID int, "
                    + "Keyword_ID int, "
                    + "Foreign Key(Movie_ID) references Movie (Movie_ID));");
            statement.executeUpdate("Create Table ProductionHouseID("
                    + "Movie_ID int, "
                    + "ProductionHouse_ID int, "
                    + "Foreign Key(Movie_ID) references Movie (Movie_ID));");
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString());
        } catch (ClassNotFoundException cE) {
            System.out.println("Class Not Found Exception: "+ cE.toString());
        }        
    }
    
    public static void FillTable()throws org.json.simple.parser.ParseException, java.text.ParseException, FileNotFoundException, IOException{
        try{
            JSONParser parser = new JSONParser();
            JSONArray parsedArray = (JSONArray) parser.parse(new FileReader("C:\\Users\\edavi\\OneDrive\\Documents\\NetBeansProjects\\FinalProjectDM\\src\\finalprojectdm\\Movies_input.txt"));
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Change password for local server to use new computer
            Connection con = DriverManager.getConnection(connectionUrl);
            Statement statement = con.createStatement();
            statement.executeUpdate("use FinalProject;");
            String InputMovie = "insert into Movie(Movie_ID, Movie_Title, Release_Date, Movie_Popularity, Average_Votes, Total_Votes, Overview, Tagline, Runtime) values (?, ?, ?, ?, ?, ?, ?, ?, ?)";
            String InputGenre = "insert into GenreID(Movie_ID, Genre_ID) values (?, ?)";
            String InputKeyword = "insert into KeywordID(Movie_ID, Keyword_ID) values (?, ?)";
            String InputProductionHouse = "insert into ProductionHouseID(Movie_ID, ProductionHouse_ID) values (?, ?)";
            
            for (Object overall : parsedArray){
                JSONObject movies = (JSONObject) overall;
                
                Long MovieID = (Long) movies.get("id");
                ID.add(MovieID);

                JSONArray Genre = (JSONArray) movies.get("genres");
                for (Object Movie_Genre : Genre) {
                    JSONObject gen = (JSONObject) Movie_Genre;
                    long gID = (long) gen.get("id");
                    Genres.add(MovieID);
                    Genres.add(gID);
                }

                String rlsDate = (String) movies.get("release_date");
                ReleaseDate.add(rlsDate);

                JSONArray Keyword = (JSONArray) movies.get("keywords");
                for (Object Movie_Keyword : Keyword) {
                    JSONObject key = (JSONObject) Movie_Keyword;
                    long Keyword_ID = (long) key.get("id");
                    Keywords.add(MovieID);
                    Keywords.add(Keyword_ID);
                }

                double Popularity_input = (double) movies.get("popularity");
                Popularity.add(Popularity_input);

                JSONArray Company = (JSONArray) movies.get("production_companies");
                for (Object pc : Company) {
                    JSONObject production = (JSONObject) pc;
                    long ProductionHouse_ID = (long) production.get("id");
                    ProductionHouse.add(MovieID);
                    ProductionHouse.add(ProductionHouse_ID);
                }

                String title = (String) movies.get("title");
                MovieTitle.add(title);

                String tag = (String) movies.get("tagline");
                Tag_Line.add(tag);

                double voteAvg = (double) movies.get("vote_average");
                Vote_adv.add(voteAvg);

                long voteTotal = (long) movies.get("vote_count");
                Vote_total.add(voteTotal);

                long Run = (long) movies.get("runtime");
                RunTime.add(Run);

                String Overview_Input = (String) movies.get("overview");
                Overview.add(Overview_Input);
            }
            System.out.println("DONE HERE");
                
            Counter=0;
            do{
                PreparedStatement PreStatement = con.prepareStatement(InputMovie); 
                PreStatement.setLong (1, ID.get(Counter));
                PreStatement.setString (2, MovieTitle.get(Counter));
                PreStatement.setString (3, ReleaseDate.get(Counter));
                PreStatement.setDouble (4, Popularity.get(Counter));
                PreStatement.setDouble (5, Vote_adv.get(Counter));
                PreStatement.setDouble (6, Vote_total.get(Counter));
                PreStatement.setString (7, Overview.get(Counter));
                PreStatement.setString (8, Tag_Line.get(Counter));
                PreStatement.setDouble (9, RunTime.get(Counter));
                PreStatement.execute();
                System.out.println(Counter);
                Counter++;
            }while(Counter < MovieTitle.size());
            System.out.println("---------------------------------------------------------------------------------------------------------------------------------------------");
            
            Counter=0;
            do{
                PreparedStatement PreStatement = con.prepareStatement(InputGenre); 
                PreStatement.setLong(1, Genres.get(Counter));
                System.out.println(Counter);
                Counter++;
                PreStatement.setLong(2, Genres.get(Counter));
                PreStatement.execute();
                System.out.println(Counter);
                Counter++;
            }while(Counter < Genres.size());          
            System.out.println("---------------------------------------------------------------------------------------------------------------------------------------------");
            
            Counter=0;
            do{
                PreparedStatement PreStatement = con.prepareStatement(InputKeyword);
                PreStatement.setLong(1, Keywords.get(Counter));
                System.out.println(Counter);
                Counter++;
                PreStatement.setLong(2, Keywords.get(Counter));
                PreStatement.execute();
                System.out.println(Counter);
                Counter++;
            }while(Counter < Keywords.size());
            System.out.println("---------------------------------------------------------------------------------------------------------------------------------------------");
            
            Counter=0;
            do{
                PreparedStatement PreStatement = con.prepareStatement(InputProductionHouse);
                PreStatement.setLong(1, ProductionHouse.get(Counter));
                System.out.println(Counter);
                Counter++;
                PreStatement.setLong(2, ProductionHouse.get(Counter));
                PreStatement.execute();
                System.out.println(Counter);
                Counter++;
            }while(Counter < ProductionHouse.size());
            System.out.println("---------------------------------------------------------------------------------------------------------------------------------------------");
            
            statement.executeUpdate("INSERT INTO KeywordsID_2 "
                + "SELECT * "
                + "FROM KeywordID "
                + "GROUP BY Keyword_ID "
                + "HAVING count(Keyword_ID) >= 16");//Crashed at 5 moved to 16            
        } catch (SQLException e) {
            System.out.println("SQL Exception: "+ e.toString());
        } catch (FileNotFoundException eA) {
            System.out.println("File Not Found. Exiting Program." + eA.toString());
        } catch (IOException eO) {
            System.out.println("File Not Found. Exiting Program." + eO.toString());
        } catch (org.json.simple.parser.ParseException eW) {
            System.out.println("File Not Parsed. Exiting Program." + eW.toString());
        }             
    }
    
    public static void quaryStart() throws SQLException{
        int Genre1 = 0;int Genre2 = 0;int Genre3 = 0;int Genre4 = 0;int Genre5 = 0;int Genre6 = 0;int Genre7 = 0;
        int Keyword1 = 0;int Keyword2 = 0;int Keyword3 = 0;int Keyword4 = 0;int Keyword5 = 0;int Keyword6 = 0;int Keyword7 = 0;int Keyword8 = 0;
        Scanner read = new Scanner(System.in);
        try{
            String connectionUrl = "jdbc:mysql://localhost/?" + "user="+ username +"&password=" + password; //Change password for local server to use new computer
            Connection con = DriverManager.getConnection(connectionUrl);
            Statement statement = con.createStatement();
            statement.executeUpdate("use FinalProject;");
            System.out.print("\n\nHello Welcome to the Eric Davis Recomendation engine.");
            do{
                Counter=0;
                System.out.println("\nIf you want to quit type 'q'.\nFor Movies in the Database type 'MovieSearch' exactly.\nWhat movie would you like to look up?");
                MovieName = read.nextLine();
                if(MovieName.equals("Q")|| MovieName.equals("q")|| MovieName.equals("Quit")|| MovieName.equals("quit")){
                    System.out.println("Exiting, Hope you have a nice day!!!");
                    System.exit(0);
                }
                
                else if(MovieName.equals("MovieSearch")){
                    PreparedStatement PS = con.prepareStatement("SELECT Movie_ID, Movie_Title, Release_Date, Movie_Popularity, Runtime FROM Movie ");
                    ResultSet RS = PS.executeQuery();

                    System.out.printf("%-20s %-90s %-12s %-10s %-10s\n", "Movie_ID" , "Movie_Title" , "Release_Date", "Movie_Popularity", "Runtime");
                    while(RS.next()) {
                        System.out.printf("%-20s %-90s %-12s %-10s %-10s\n",RS.getString("Movie_ID"), RS.getString("Movie_Title"), RS.getString("Release_Date"), RS.getString("Movie_Popularity"), RS.getString("Runtime"));
                    }
                }
                
                else{ 
                    PreparedStatement PS = con.prepareStatement("SELECT Movie_ID, Movie_Title, Release_Date, Movie_Popularity, Runtime FROM Movie WHERE Movie_Title like '%"+ MovieName +"%';");
                    ResultSet RS = PS.executeQuery();

                    System.out.printf("%-20s %-90s %-12s %-20s %-10s\n", "Movie_ID" , "Movie_Title" , "Release_Date", "Movie_Popularity", "Runtime");
                    while(RS.next()) {
                        System.out.printf("%-20s %-90s %-12s %-20s %-10s\n",RS.getString("Movie_ID"), RS.getString("Movie_Title"), RS.getString("Release_Date"), RS.getString("Movie_Popularity"), RS.getString("Runtime"));
                        Counter++;
                        if(Counter == 1){    
                            System.out.println("\nLooking into Similar Movies ............");
                            PS = con.prepareStatement("Select m.Movie_ID, g.Genre_ID "
                                    + "From movie m, genreid g WHERE m.Movie_ID = g.Movie_ID AND m.Movie_Title LIKE '"+ MovieName +"';");
                            RS = PS.executeQuery();
                            Counter=0;
                            while(RS.next()) {
                                if(Counter==0){Genre1 = RS.getInt(2);}
                                if(Counter==1){Genre2 = RS.getInt(2);}
                                if(Counter==2){Genre3 = RS.getInt(2);}
                                if(Counter==3){Genre4 = RS.getInt(2);}
                                if(Counter==4){Genre5 = RS.getInt(2);}
                                if(Counter==5){Genre6 = RS.getInt(2);}
                                if(Counter==6){Genre7 = RS.getInt(2);}
                                Counter++;
                            }
                            PS = con.prepareStatement("Select m.Movie_ID, k.Keyword_ID, Count(*) "
                                    + "From movie m, keywordsid_2 k where m.Movie_ID = k.Movie_ID AND m.Movie_Title LIKE '"+ MovieName +"';");
                            RS = PS.executeQuery();
                            Counter=0;
                            while(RS.next()) {
                                if(Counter==0){Keyword1 = RS.getInt(2);}
                                if(Counter==1){Keyword2 = RS.getInt(2);}
                                if(Counter==2){Keyword3 = RS.getInt(2);}
                                if(Counter==3){Keyword4 = RS.getInt(2);}
                                if(Counter==4){Keyword5 = RS.getInt(2);}
                                if(Counter==5){Keyword6 = RS.getInt(2);}
                                if(Counter==6){Keyword7 = RS.getInt(2);}
                                if(Counter==6){Keyword8= RS.getInt(2);}
                                Counter++;
                            }

                            PS = con.prepareStatement("Select m.*, g.*, k.*, count(*) AS Similar "
                                    + "From movie m, genreid g, keywordsid_2 k "
                                    + "where m.Movie_ID = g.Movie_ID AND m.Movie_ID = k.Movie_ID AND (g.Genre_ID = '"+Genre1+"' or g.Genre_ID = '"+Genre2+"' or g.Genre_ID = '"+Genre3+"' or g.Genre_ID = '"+Genre4+"' "
                                        + "or g.Genre_ID = '"+Genre5+"' or g.Genre_ID = '"+Genre6+"' or g.Genre_ID = '"+Genre7+"' or k.Keyword_ID = '"+Keyword1+"' or k.Keyword_ID = '"+Keyword2+"' "
                                        + "or k.Keyword_ID = '"+Keyword3+"' or k.Keyword_ID = '"+Keyword4+"' or k.Keyword_ID = '"+Keyword5+"' or k.Keyword_ID = '"+Keyword6+"' or k.Keyword_ID = '"+Keyword7+"' "
                                        + "or k.Keyword_ID = '"+Keyword8+"')"
                                    + "group by m.Movie_ID "
                                    + "Having Similar > 5 "
                                    + "Order by Similar DESC;");
                            RS = PS.executeQuery();
                            Counter=0;
                            System.out.printf("%-20s %-90s %-12s %-20s %-10s %-20s %-20s %-20s\n", "Movie_ID" , "Movie_Title" , "Release_Date", "Movie_Popularity", "Runtime", "Genre_ID", "Keyword_ID", "Similar");
                            while(RS.next()) {
                                System.out.printf("%-20s %-90s %-12s %-20s %-10s %-20s %-20s %-20s\n",RS.getString("Movie_ID"), RS.getString("Movie_Title"), RS.getString("Release_Date"), 
                                        RS.getString("Movie_Popularity"), RS.getString("Runtime"), RS.getString("Genre_ID"), RS.getString("Keyword_ID"), RS.getString("Similar"));
                                Counter++;
                            }
                        }
                        
                        else if(Counter > 1){
                            do{
                                Counter=0;
                                System.out.println("There are multiple movies with that name.\n\n   What is the Movie_ID for the Movie?");
                                int MovieIDTemp = read.nextInt();
                                PS = con.prepareStatement("SELECT Movie_ID, Movie_Title, Release_Date, Movie_Popularity, Runtime FROM Movie WHERE Movie_ID ="+ MovieIDTemp +";");
                                RS = PS.executeQuery();
                                System.out.printf("%-20s %-90s %-12s %-20s %-10s\n", "Movie_ID" , "Movie_Title" , "Release_Date", "Movie_Popularity", "Runtime");
                                while(RS.next()) {
                                    System.out.printf("%-20s %-90s %-12s %-20s %-10s\n",RS.getString("Movie_ID"), RS.getString("Movie_Title"), RS.getString("Release_Date"), RS.getString("Movie_Popularity"), RS.getString("Runtime"));
                                    Counter++;
                                }
                            }while(Counter > 1);
                        }
                    }
                }      
            }while(!MovieName.equals("Quit")|| !MovieName.equals("quit"));
            System.out.println("Exiting, Hope you have a nice day!!!");
        }catch (SQLException e) {System.out.println("SQL Exception: "+ e.toString());}
    }
}