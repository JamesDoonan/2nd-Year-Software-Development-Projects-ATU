package ie.atu.sw;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;

public class FileOutput {

    public static void writeStringToFile(String outputFileName, String convertedString) throws IOException {
        BufferedWriter bw = new BufferedWriter(new FileWriter(outputFileName)); // Using a Buffered Writer
        bw.write(convertedString); // Write the converted string to a text file called the outputFileName

        bw.close(); // Close Buffered Writer
    }
}
