package ie.atu.sw;

import java.util.HashMap;
import java.util.Map;

public class MorseEncoder {

    public static String encodeToMorse(Map<Integer, String> mappedFile) {
        StringBuilder englishToMorse = new StringBuilder(); // Using a string builder ( Big-O Notation Stringbuilder:  O(1) )

        for (int i = 1; i <= mappedFile.entrySet().size(); i++) { // For each integer key in the mappedFile ( Big-O Notation For Loops:  O(n²),  Big-O Notation Hash Map:  O(1) )
            String line = mappedFile.get(i); // Get the associated key value

            for (char letter : line.toUpperCase().toCharArray()) {
                String morseCode = createEncodingMap().get(letter); // Search the encoding map for the letter

                if (morseCode != null) { // If the value is not null
                    englishToMorse.append(morseCode).append(" "); // Append the morsecode corresponding to the letter
                }
            }
            englishToMorse.append(" / "); // Add a space at the end of each sentence
            englishToMorse.append("\n"); // Add a new line after each map item converted
        }
        return englishToMorse.toString();
    }

    public static String encodeColourName(String colourName) {
        String morseConvertedColourName = "";

        for (char letter : colourName.toCharArray()) {
            String morseCode = createEncodingMap().get(letter); // Search the encoding map for the letter

            if (morseCode != null) { // If the value is not null
                morseConvertedColourName += (morseCode + " "); // Append the morsecode corresponding to the letter
            }
        }

        return morseConvertedColourName;
    }

    private static Map<Character, String> createEncodingMap() {
        Map<Character, String> encodingValues = new HashMap<>(); // ( Big-O Notation Hash Map:  O(1) )

        encodingValues.put(' ', " / ");
        encodingValues.put(',', "--..--");
        encodingValues.put('.', ".-.-.-");
        encodingValues.put('?', "..--..");
        encodingValues.put('0', "-----");
        encodingValues.put('1', ".----");
        encodingValues.put('2', "..---");
        encodingValues.put('3', "...--");
        encodingValues.put('4', "....-");
        encodingValues.put('5', ".....");
        encodingValues.put('6', "-....");
        encodingValues.put('7', "--...");
        encodingValues.put('8', "---..");
        encodingValues.put('9', "----.");
        encodingValues.put('A', ".-");
        encodingValues.put('B', "-...");
        encodingValues.put('C', "-.-.");
        encodingValues.put('D', "-..");
        encodingValues.put('E', ".");
        encodingValues.put('F', "..-.");
        encodingValues.put('G', "--.");
        encodingValues.put('H', "....");
        encodingValues.put('I', "..");
        encodingValues.put('J', ".---");
        encodingValues.put('K', "-.-");
        encodingValues.put('L', ".-..");
        encodingValues.put('M', "--");
        encodingValues.put('N', "-.");
        encodingValues.put('O', "---");
        encodingValues.put('P', ".--.");
        encodingValues.put('Q', "--.-");
        encodingValues.put('R', ".-.");
        encodingValues.put('S', "...");
        encodingValues.put('T', "-");
        encodingValues.put('U', "..-");
        encodingValues.put('V', "...-");
        encodingValues.put('W', ".--");
        encodingValues.put('X', "-..-");
        encodingValues.put('Y', "-.--");
        encodingValues.put('Z', "--...");
        encodingValues.put('É', "..-..");

        return encodingValues;
    }
}
