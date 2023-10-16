package ie.atu.sw;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

public class FileInput {

    public static Map<Integer, String> convertFileToMap(String chosenFile) throws IOException {
        Map<Integer, String> mappedFile = new HashMap<>(); // ( Big-O Notation Hash Map:  O(1) )

        BufferedReader bufferedReader = new BufferedReader(new FileReader(chosenFile)); // Using a Buffered Reader ( Big-O Notation BufferedReader:  O(n) )

        String line;
        int lineCount = 1;

        while ((line = bufferedReader.readLine()) != null) { // While there still is another line to read in ( Big-O Notation While loop:  O(n) )
            mappedFile.put(lineCount, line); // Add the lineCount and the line to the map
            lineCount++;
        }
        bufferedReader.close(); // Close Buffered Reader

        return mappedFile;
    }
}
