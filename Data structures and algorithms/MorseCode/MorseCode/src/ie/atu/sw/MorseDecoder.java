package ie.atu.sw;

import java.util.LinkedHashMap;
import java.util.Map;

public class MorseDecoder {

    public static String decodeMorseToEnglish(Map<Integer, String> mappedFile) {
        StringBuilder morseToEnglish = new StringBuilder(); // Using a string builder ( Big-O Notation Stringbuilder:  O(1) )

        for (int i = 1; i <= mappedFile.keySet().size(); i++) { // For each integer key in the mappedFile ( Big-O Notation For Loop:  O(n²), Big-O Notation Hash Map:  O(1) )
            String line = mappedFile.get(i); // Get the associated key value

            String[] words = line.split(" / "); // Split out this line into an array of words by " / "

            for (String word : words) {
            String[] charactersInWord = word.split(" "); // Split out these words into an array of characters by " "

                for (String character : charactersInWord) {
                        Character convertedMorse = createDecodingMap().get(character); // Get the associated Letter for that morse code

                        if (convertedMorse != null) {
                            morseToEnglish.append(convertedMorse); // Append this to the string
                        }
                }
                morseToEnglish.append(" "); // Add a space between words
            }
            morseToEnglish.append("\n"); // Add a new line after each line
        }
        return morseToEnglish.toString().toLowerCase();
    }

    private static Map<String, Character> createDecodingMap() {
        Map<String, Character> decodingValues = new LinkedHashMap<>(); // ( Big-O Notation Hash Map:  O(1) )

        decodingValues.put("/", ' ');
        decodingValues.put("--..--", ',');
        decodingValues.put(".-.-.-", '.');
        decodingValues.put("..--..", '?');
        decodingValues.put("-----", '0');
        decodingValues.put(".----", '1');
        decodingValues.put("..---", '2');
        decodingValues.put("...--", '3');
        decodingValues.put("....-", '4');
        decodingValues.put(".....", '5');
        decodingValues.put("-....", '6');
        decodingValues.put("--...", '7');
        decodingValues.put("---..", '8');
        decodingValues.put("----.", '9');
        decodingValues.put(".-", 'A');
        decodingValues.put("-...", 'B');
        decodingValues.put("-.-.",'C');
        decodingValues.put("-..", 'D');
        decodingValues.put(".", 'E');
        decodingValues.put("..-.", 'F');
        decodingValues.put("--.", 'G');
        decodingValues.put("....", 'H');
        decodingValues.put("..", 'I');
        decodingValues.put(".---", 'J');
        decodingValues.put("-.-", 'K');
        decodingValues.put(".-..", 'L');
        decodingValues.put("--", 'M');
        decodingValues.put("-.", 'N');
        decodingValues.put("---", 'O');
        decodingValues.put(".--.", 'P');
        decodingValues.put("--.-", 'Q');
        decodingValues.put(".-.", 'R');
        decodingValues.put("...", 'S');
        decodingValues.put("-", 'T');
        decodingValues.put("..-", 'U');
        decodingValues.put("...-", 'V');
        decodingValues.put(".--", 'W');
        decodingValues.put("-..-", 'X');
        decodingValues.put("-.--", 'Y');
        decodingValues.put("--...", 'Z');

        return decodingValues;
    }
}
