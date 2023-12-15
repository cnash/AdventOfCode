import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Cards {
  // private static String inputFile = "C:\\dev\\src\\nash\\advent\\20231204\\input-sample.txt";
  private static String inputFile = "C:\\dev\\src\\nash\\advent\\20231204\\input-actual.txt";

  static void Problem1() throws FileNotFoundException, IOException {
    int total = 0;
    try (BufferedReader br = new BufferedReader(new FileReader(inputFile))) {
      String line = br.readLine();

      while (line != null) {
        total += scoreCard(line);
        line = br.readLine();
      }
    }
    log("Problem 1 result: %d", total);
  }
  
  static void Problem2() throws FileNotFoundException, IOException {
    int total = 0;
    List<String> cards = new ArrayList<String>();
    try (BufferedReader br = new BufferedReader(new FileReader(inputFile))) {
      String line = br.readLine();
      while (line != null) {
        cards.add(line);
        line = br.readLine();
      }
    }
    int[] scoreCache = new int[cards.size()];
    for (int i=0;i<cards.size(); i++)
    {
      scoreCache[i] = -1;
    }
    for (int i=0;i<cards.size(); i++)
    {
      total += scoreCardRecursive(cards, i, scoreCache);
    }
    log("Problem 2 result: %d", total);
  }

  static int winningNumbersOnCard(String cardLine) {
    String s1 = cardLine.substring(4);
    String[] s2 = s1.split(":");
    String[] s3 = s2[1].split("[|]");
    String[] winnersArray = s3[0].trim().split("\\s+");
    List<String> winners = Arrays.asList(winnersArray);
    String[] mineArray = s3[1].trim().split("\\s+");
    List<String> mine = Arrays.asList(mineArray);
    int winnerCount = 0;
    for (String m : mine) {
      if (winners.contains(m)) {
        winnerCount++;
      }      
    }
    return winnerCount;
  }

  static int scoreCard(String cardLine) {
    int winnerCount = winningNumbersOnCard(cardLine);
    if (winnerCount <=1) {
      return winnerCount;
    } else {
      return (int) Math.pow(2, winnerCount-1);
    }
  }

  static int scoreCardRecursive(List<String> cards, int idx, int[] scoreCache)
  {
    // log("Evaluating %d", idx);
    if (idx >= cards.size())
    {
      return 0;
    }
    if (scoreCache[idx] >= 0) {
      return scoreCache[idx];
    }
    int thisCardValue = winningNumbersOnCard(cards.get(idx));
    int total = 1;
    for (int i=idx+1;i<=idx+thisCardValue && i< cards.size(); i++)
    {
      total += scoreCardRecursive(cards, i, scoreCache);
    }
    scoreCache[idx] = total;
    return total;
  }

  private static void log(String format, Object... args) {
    System.out.printf(format + "\n", args);
  }

  public static void main(String[] args) {
    try {
      Problem1();
      Problem2();
    } catch (Exception exc) {
      System.out.printf("Exception: %s", exc.getMessage());
    }
  }
}