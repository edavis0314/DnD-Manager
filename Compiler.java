/*
Eric Davis
N00911966
Compilers
Project #1 & 2
03/01/2018
*/

import java.util.*;
import java.lang.*;
import java.io.*;

class Compiler{
   private static File inputFile;
   private static Scanner fileScan;
   public static ArrayList<String> FileList = new ArrayList<String>();
   public static int lengthOfFileList=0;

   public static void main(String[] args){
      try{
         readFileIntoArray(args[0]);
      }  catch (ArrayIndexOutOfBoundsException e)  {
         System.out.println("[ERROR] No test file provided");
         System.exit(0);
      }
      LexicalAnalizer.main();
      parcer.main();
      Semantic_Analyzer.main();
      CodeGenerator.main();
   }

   public static void readFileIntoArray(String argFile){
      inputFile = new File(argFile);
      try {
         fileScan = new Scanner(inputFile);
         while (fileScan.hasNext()){
            FileList.add(fileScan.nextLine());
            lengthOfFileList++;
         }
         fileScan.close();

      } catch(FileNotFoundException fnfe) {
			System.out.println("FileNotFoundException thrown:");
			System.out.println("[ERROR] File not found");
			System.exit(0);
		}
   }

}

class LexicalAnalizer{
   public static ArrayList<String> FileList = new ArrayList<String>();
   public static ArrayList<String> TokenList = new ArrayList<String>();
   public static int lengthOfFileList=0;
   private static char CheckByChar;
   private static int nestedComment = 0;
   public static String TransitionString;
   private static String Token = "";
   private static int ascii;
   private static int word = 0;
   private static int number = 0;
   private static int Echeck=0;
   private static int Dotcheck=0;
   public static int MainCheck = 0;

   public static void main(){
      for(int n=0; n<Compiler.lengthOfFileList; n++){
         TransitionString = Compiler.FileList.get(n)+ " ";
         for (int m=0; m < TransitionString.length(); m++){
            CheckByChar = TransitionString.charAt(m);
            ascii=(int)CheckByChar;
            if(nestedComment>0){
               m=commentizer(m);
            }
            else if((97<=ascii && ascii<=122) || word!=0){
               m=WordCheck(m);
            }
            else if((ascii>=48 && ascii<=57) || number>0
                  || ascii==69 || ascii==46 || Dotcheck>0
                  || Echeck>0){
               if(Dotcheck>0||Echeck>0){
                  m=NumberCheck_float(m);
               }
               else{
                 m=NumberCheck_int(m);
               }
            }
            else if(ascii==43 || ascii==45 || ascii==42 || ascii==59
                 || ascii==44 || ascii==91 || ascii==93 || ascii==40
                 || ascii==41 || ascii==123 || ascii==125){
               m=interator(m);
            }
            else if(ascii==47){
               m=SlashCheck(m);
            }
            else if(ascii==60 || ascii==61 || ascii==62 || ascii==33){
               m = SpecialCharacterCheck(m);
            }
            else if(ascii==32){
            }
            else{
            }
         }
      }
      TokenList.add("$");
      if(MainCheck!=1){parcer.reject();}

   }

   public static void tokenCheck(String Token){
      if (Token.contains(".")||Token.contains("E")){parcer.check = "FLOAT";}
      else if(Token.contains("0")||Token.contains("1")||Token.contains("2")
            ||Token.contains("3")||Token.contains("4")||Token.contains("5")
            ||Token.contains("6")||Token.contains("7")||Token.contains("8")
            ||Token.contains("9")||Token.contains("0")){parcer.check = "NUM";}
      else if (Token.equals("if")){parcer.check = "IF";}
      else if (Token.equals("else")){parcer.check = "ELSE";}
      else if (Token.equals("return")){parcer.check = "RETURN";}
      else if (Token.equals("void")){parcer.check = "VOID";}
      else if (Token.equals("while")){parcer.check = "WHILE";}
      else if (Token.equals("INT")){parcer.check = "INT";}
      else if (Token.equals("main")){parcer.check = "ID";MainCheck++;}
      else if (Token.equals(";")){parcer.check = ";";}
      else if (Token.equals(",")){parcer.check = ",";}
      else if (Token.equals("+")){parcer.check = "+";}
      else if (Token.equals("-")){parcer.check = "-";}
      else if (Token.equals("*")){parcer.check = "*";}
      else if (Token.equals("/")){parcer.check = "/";}
      else if (Token.equals("<")){parcer.check = "<";}
      else if (Token.equals(">")){parcer.check = ">";}
      else if (Token.equals("<=")){parcer.check = "<=";}
      else if (Token.equals(">=")){parcer.check = ">=";}
      else if (Token.equals("==")){parcer.check = "==";}
      else if (Token.equals("!=")){parcer.check = "!=";}
      else if (Token.equals("=")){parcer.check = "=";}
      else if (Token.equals("(")){parcer.check = "(";}
      else if (Token.equals(")")){parcer.check = ")";}
      else if (Token.equals("[")){parcer.check = "[";}
      else if (Token.equals("{")){parcer.check = "{";}
      else if (Token.equals("}")){parcer.check = "}";}
      else if (Token.equals("$")){parcer.check = "$";}
      else{parcer.check = "ID";}
   }

   public static int SlashCheck(int m){
      m=LookAhead(m);
      if(ascii==42){
         nestedComment++;
      }
      else if(ascii==47){
         m=TransitionString.length();
      }
      else{
         m--;
         CheckByChar = TransitionString.charAt(m);
         ascii=(int)CheckByChar;
         m=interator(m);
      }
      return m;
   }

   public static int commentizer(int m){
      if(ascii==47){
         if((m<TransitionString.length()) && (TransitionString.charAt(m+1)=='*')){
            nestedComment++;
            m++;
         }
      }
      else if(ascii==42){
         if((m<TransitionString.length()) && (TransitionString.charAt(m+1)=='/')){
            if(nestedComment>0){
               nestedComment--;
               m++;
            }
         }
      }
      return m;
   }

   public static int interator(int m){
      Token += CheckByChar;
      TokenList.add(Token);
      Token = "";
      return m;
   }

   public static int LookAhead(int m){
      if(m < TransitionString.length()-1){
         m++;
         CheckByChar = TransitionString.charAt(m);
         ascii=(int)CheckByChar;
      }
      return m;
   }

   public static int WordCheck(int m){
      if(97<=ascii && ascii<=122){ Token += CheckByChar; word++; }
      else{
         word = 0;
         tokenCheck(Token);
         TokenList.add(Token);
         Token = "";
         m--;
      }
      return m;
   }

   public static int NumberCheck_int(int m){
      if(ascii>=48 && ascii<=57){
         Token += CheckByChar;
         m=LookAhead(m);
         if(ascii>=48 && ascii<=57){
            number++;
            m--;
         }
         else if(ascii==46){
            Dotcheck++;
            Token += CheckByChar;
         }
         else if(ascii==69){
            Echeck++;
            Token += CheckByChar;
         }
         else{
            TokenList.add(Token);
            Token = "";
            number=0;
            m--;
         }
      }
      return m;
   }

   public static int NumberCheck_float(int m){
      if(Dotcheck>0){
         if(48<=ascii && ascii<=57){
            Dotcheck++;
            Token += CheckByChar;
         }
         else if(Dotcheck>=1){
            if(ascii==69){
               Echeck++;
               Token += CheckByChar;
               Dotcheck=0;
            }
            else{
               TokenList.add(Token);
               Token = "";
               number=0;
               m--;
               Dotcheck=0;
            }
         }
         else{
            TokenList.add(Token);
            Token = "";
            number=0;
            m--;
            Dotcheck=0;
            Echeck=0;
         }
      }
      else if(Echeck>0){
         if(ascii>=48 && ascii<=57){
            Echeck++;
            Token += CheckByChar;
         }
         else if(ascii==43||ascii==45){
            Echeck++;
            Token += CheckByChar;
         }
         else{
            TokenList.add(Token);
            Token = "";
            number=0;
            m--;
            Dotcheck=0;
            Echeck=0;
         }
      }

      return m;
   }

   public static int SpecialCharacterCheck(int m){
      if(ascii==60){
         Token += CheckByChar;
         m=LookAhead(m);
         if (ascii==61){
            Token += CheckByChar;
            TokenList.add(Token);
         }else{
            m--;
            CheckByChar = TransitionString.charAt(m);
            ascii=(int)CheckByChar;
            TokenList.add(Token);
         }
      }
      else if (ascii==62){
        Token += CheckByChar;
        m=LookAhead(m);
        if (ascii==61){
           Token += CheckByChar;
           TokenList.add(Token);
        }else{
           m--;
           CheckByChar = TransitionString.charAt(m);
           ascii=(int)CheckByChar;
           TokenList.add(Token);
        }
      }
      else if (ascii==61){
         Token += CheckByChar;
         m=LookAhead(m);
         if (ascii==61){
            Token += CheckByChar;
            TokenList.add(Token);
         }
         else{
            m--;
            TokenList.add(Token);
         }
      }
      else if (ascii==33){
         Token += CheckByChar;
         m=LookAhead(m);
         if (ascii==61){
            Token += CheckByChar;
            TokenList.add(Token);
         }
         else{
         }
      }
      else{}
      Token="";
      return m;
   }
}

class parcer{
  public static int count = 0;
  public static String TransitionString;
  public static String check;

  public static void main(){
      TransitionString = LexicalAnalizer.TokenList.get(count);
      LexicalAnalizer.tokenCheck(TransitionString);
      program();
   }

   public static void next(){
      try{
        count++;
        TransitionString = LexicalAnalizer.TokenList.get(count);
        check = "";
        LexicalAnalizer.tokenCheck(TransitionString);
      } catch(NoSuchElementException e){
        TransitionString = null;
      }
   }

   public static void reject(){
     System.out.println("REJECT");
     System.exit(0);
   }

   public static void accept(){
     System.out.println("ACCEPT");
   }

    public static void program(){
        declerationList_1();
    }

    public static void declerationList_1(){
        declaration_1();
        declerationList_2();
    }

    public static void declerationList_2(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            declaration_1();
            declerationList_2();
        }
        else if(TransitionString.equals("$")){
            //accept();
        }
        else{
            reject();
        }
    }

    public static void type_decleration(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            next();
        }
        else{
            reject();
        }
    }

    public static void declaration_1(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            type_decleration();
            if(check.equals("ID")) {
                next();
                declaration_2();
            }
            else {
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void declaration_2(){
        if(TransitionString.equals(";") || TransitionString.equals("[")){
            var_decleration_2();
        }
        else if(TransitionString.equals("(")){
            next();
            params();
            if(TransitionString.equals(")")){
                next();
                compound_stmt();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void params(){
        if(TransitionString.equals("void")){
            next();
            parameter_1();
        }
        else if(TransitionString.equals("int") || TransitionString.equals("float")){
            next();
            if(check.equals("ID")){
                next();
                param_2();
                parameter_2();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }

    }

    public static void parameter_1(){
        if(check.equals("ID")){
            next();
            param_2();
            parameter_2();
        }
        else if(TransitionString.equals(")")){
        }
        else{
            reject();
        }
    }

    public static void parameter_2(){
        if(TransitionString.equals(",")){
            next();
            param_1();
            parameter_2();
        }
        else if(TransitionString.equals(")")){
        }
        else{
           reject();
        }
    }

    public static void param_1(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            type_decleration();
            if(check.equals("ID")){
                next();
                param_2();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void param_2(){
        if(TransitionString.equals("[")){
            next();
            if(TransitionString.equals("]")){
                next();
            }
            else{
                reject();
            }
        }
        else if(TransitionString.equals(",") || TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")
        || TransitionString.equals(")")){
        }
        else{
            reject();
        }
    }

    public static void compound_stmt(){
        if(TransitionString.equals("{")){
            next();
            local_decleration();
            statment_list();
            if(TransitionString.equals("}")){
                next();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void local_decleration(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            var_decleration_1();
            local_decleration();
        }
        else if(TransitionString.equals("(") || check.equals("ID") || check.equals("NUM") || TransitionString.equals(";") ||
                TransitionString.equals("{") || TransitionString.equals("if") || TransitionString.equals("while") ||
                TransitionString.equals("return") || TransitionString.equals("}")){
        }
        else{
           reject();
        }
    }

    public static void var_decleration_1(){
        if(TransitionString.equals("int") || TransitionString.equals("float") || TransitionString.equals("void")){
            type_decleration();
            if(check.equals("ID")){
                next();
                var_decleration_2();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void var_decleration_2(){
        if(TransitionString.equals(";")){
            next();
        }
        else if(TransitionString.equals("[")){
            next();
            if(check.equals("NUM")){
                next();
                if(TransitionString.equals("]")){
                    next();
                    if(TransitionString.equals(";")){
                        next();
                    }
                    else{
                        reject();
                    }
                }
                else{
                    reject();
                }
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void statment_list(){
        if(TransitionString.equals("if") || TransitionString.equals("return") || TransitionString.equals("while") || check.equals("ID")
           || TransitionString.equals("{") || TransitionString.equals("(") || check.equals("NUM")){
            statement();
            statment_list();
        }
        else if(TransitionString.equals("}")){
        }
        else{
            reject();
        }
    }

    public static void statement() {
        if(check.equals("ID") || TransitionString.equals("(")){
            expression_statement();
        }
        else if(TransitionString.equals("{")){
            compound_stmt();
        }
        else if(TransitionString.equals("if")){
            selection_statement();
        }
        else if(TransitionString.equals("while")){
            iteration_statement();
        }
        else if(TransitionString.equals("return")){
            return_statement();
        }
        else{
            reject();
        }
    }

    public static void selection_statement(){
        if(TransitionString.equals("if")){
            next();
            if(TransitionString.equals("(")){
                next();
                expression_1();
                if(TransitionString.equals(")")){
                    next();
                    statement();
                    else_statement();
                }
                else{
                    reject();
                }
            }
            else{
                reject();
            }

        }
        else{
            reject();
        }
    }

    public static void expression_statement(){
        if(check.equals("ID")){
            expression_1();
            if(TransitionString.equals(";")){
                next();
            }
            else{
                reject();
            }
        }
        else if(TransitionString.equals(";")){
            next();
        }
        else{
            reject();
        }
    }

    public static void else_statement(){
        if(TransitionString.equals("else")){
            next();
            statement();
        }
        else if(TransitionString.equals("(") || check.equals("ID") || check.equals("NUM") || TransitionString.equals(";") ||
                TransitionString.equals("{") || TransitionString.equals("if") ||TransitionString.equals("while")
                || TransitionString.equals("return") || TransitionString.equals("}")){
        }
        else{
            reject();
        }
    }

    public static void iteration_statement(){
        if(TransitionString.equals("while")){
            next();
            if(TransitionString.equals("(")){
                next();
                expression_1();
                if(TransitionString.equals(")")){
                    next();
                    statement();
                }
                else{
                    reject();
                }
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void return_statement(){
        if(TransitionString.equals("return")){
            next();
            var_return();
        }
        else{
            reject();
        }
    }

    public static void var_return(){
        if(TransitionString.equals(";")){
            next();
        }
        else{
            expression_1();
            if(TransitionString.equals(";")){
                next();
            }
            else{
                reject();
            }
        }
    }

    public static void expression_1(){
        if(check.equals("ID")){
            next();
            ID();
        }
        else if(TransitionString.equals("(")){
            expression_1();
            if(TransitionString.equals(")")){
                next();
                expression_2();
            }
        }
        else if(check.equals("NUM")){
            next();
            expression_2();
        }
        else{
            reject();
        }
    }

    public static void expression_2(){
        term_2();
        additive_expression_2();
        relation();
    }

    public static void var_type_1(){
        if(TransitionString.equals("[")){
            next();
            expression_1();
            if(TransitionString.equals("]")){
                next();
            }
            else{
                reject();
            }
        }
        else if(TransitionString.equals("=")|| TransitionString.equals("*") || TransitionString.equals("/")
                || TransitionString.equals("+") || TransitionString.equals("-") || TransitionString.equals("<=")
                || TransitionString.equals("<") || TransitionString.equals(">") || TransitionString.equals(">=")
                || TransitionString.equals("==") || TransitionString.equals("!=") || TransitionString.equals(";")
                || TransitionString.equals(")") || TransitionString.equals("]") || TransitionString.equals(",")){
        }
        else{
            reject();
        }
    }

    public static void var_type_2(){
        if(TransitionString.equals("=")){
            next();
            expression_1();
        }
        else if(TransitionString.equals("*") || TransitionString.equals("/") || TransitionString.equals("+")
                || TransitionString.equals("-") || TransitionString.equals("<=") || TransitionString.equals("<")
                || TransitionString.equals(">") || TransitionString.equals(">=") || TransitionString.equals("==")
                || TransitionString.equals("!=") || TransitionString.equals(";") || TransitionString.equals(")")
                || TransitionString.equals("]") || TransitionString.equals(",")){
            expression_2();
        }
        else {
            reject();
        }
     }

    public static void ID(){
         if(TransitionString.equals("[") || TransitionString.equals("=")
          || TransitionString.equals("*") || TransitionString.equals("/") || TransitionString.equals("+")
          || TransitionString.equals("-") || TransitionString.equals("<=") || TransitionString.equals("<")
          || TransitionString.equals(">") || TransitionString.equals(">=") || TransitionString.equals("==")
          || TransitionString.equals("!=") || TransitionString.equals(";") || TransitionString.equals(")")
          || TransitionString.equals("]") || TransitionString.equals(",")){
            var_type_1();
            var_type_2();
        }
        else if(TransitionString.equals("(")){
            next();
            args();
            if(TransitionString.equals(")")){
                next();
                expression_2();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void var(){
        var_type_1();
    }

    public static void relation(){
        if(TransitionString.equals("<=") || TransitionString.equals("<") || TransitionString.equals(">")
            || TransitionString.equals(">=") || TransitionString.equals("==") || TransitionString.equals("!=")){
            relop();
            additive_expression_1();
        }
        else if(TransitionString.equals(";") || TransitionString.equals(")") || TransitionString.equals("]")
            || TransitionString.equals(",")) {
        }
        else{
            reject();
        }
    }

    public static void relop(){
        if(TransitionString.equals("<=") || TransitionString.equals("<") || TransitionString.equals(">")
            || TransitionString.equals(">=") || TransitionString.equals("==") || TransitionString.equals("!=")){
            next();
        }
        else{
            reject();
        }
    }

    public static void additive_expression_1(){
        factor_1();
        additive_expression_2();
    }

    public static void additive_expression_2(){
        if(TransitionString.equals("+") || TransitionString.equals("-")){
            addop();
            term_1();
            additive_expression_2();
        }
        else if(TransitionString.equals("<=") || TransitionString.equals("<") || TransitionString.equals(">")
             || TransitionString.equals(">=")|| TransitionString.equals("==") || TransitionString.equals("!=")
             || TransitionString.equals(";") || TransitionString.equals(")") || TransitionString.equals("]")
             || TransitionString.equals(",")){
        }
        else{
            reject();
        }
    }

    public static void addop(){
        if(TransitionString.equals("+") || TransitionString.equals("-")){
            next();
        }
        else{
            reject();
        }
    }

    public static void term_1(){
        factor_1();
        term_2();
    }

    public static void term_2(){
        if(TransitionString.equals("*") || TransitionString.equals("/")){
            mulop();
            factor_1();
            term_2();
        }
        else if(TransitionString.equals("+") || TransitionString.equals("-") || TransitionString.equals("<=")
             || TransitionString.equals("<") || TransitionString.equals(">") || TransitionString.equals(">=")
             || TransitionString.equals("==") || TransitionString.equals("!=") || TransitionString.equals(";")
             || TransitionString.equals(")") || TransitionString.equals("]") || TransitionString.equals(",")){
        }
        else{
            reject();
        }
    }

    public static void mulop(){
        if(TransitionString.equals("*") || TransitionString.equals("/")){
            next();
        }
        else{
            reject();
        }
    }

    public static void factor_1(){
        if(TransitionString.equals("(")){
            next();
            expression_1();
            if(TransitionString.equals(")")){
                next();
            }
        }
        else if(check.equals("NUM")){
            next();
        }
        else if(check.equals("ID")){
            next();
            if(TransitionString.equals("(")){
                factor_2();
            }
            else{
                var();
            }
        }
        else{
            reject();
        }
    }

    public static void factor_2(){
        if(TransitionString.equals("(")){
            next();
            args();
            if(TransitionString.equals(")")){
                next();
            }
            else{
                reject();
            }
        }
        else{
            reject();
        }
    }

    public static void args(){
        if(TransitionString.equals(")")){
        }
        else{
            args_list_1();
        }
    }

    public static void args_list_1(){
        expression_1();
        args_list_2();
    }

    public static void args_list_2(){
        if(TransitionString.equals(",")){
            next();
            expression_1();
            args_list_2();
        }
        else if(TransitionString.equals(")")){
        }
        else{
            reject();
        }
    }
}