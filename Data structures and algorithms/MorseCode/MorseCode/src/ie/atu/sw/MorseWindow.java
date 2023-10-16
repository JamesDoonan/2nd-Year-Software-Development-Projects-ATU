package ie.atu.sw;

import javax.swing.*;
import javax.swing.border.*;
import java.awt.*;
import java.awt.event.*;
import java.io.IOException;
import java.text.DecimalFormat;
import java.util.Map;
import java.util.concurrent.ThreadLocalRandom;

public class MorseWindow {
	private Colour[] colours = Colour.values(); // This might come in handy
	private String nameOfColour; // String variable for the name of the random colour generated
	private String nameOfColourInMorse; // String variable for the name of the random colour converted to morse

	private ThreadLocalRandom rand = ThreadLocalRandom.current(); // This will definitely come in handy
	private JFrame win; //The GUI Window
	private JTextArea txtOutput = new JTextArea(); //The text box to output the results to
	private JTextField txtFilePath; // The file name to process

	private String fileChosen; // File chosen by user
	private String morseConvertedString;
	private String englishConvertedString;
	private String localMorseEncodedFile = "localEncodedFile.txt"; // Output encoded file path
	private String localMorseDecodedFile = "localDecodedFile.txt"; // Output decoded file path

	public MorseWindow(){
		/*
		 * Create a window for the application. Building a GUI is an example of
		 * "divide and conquer" in action. A GUI is really a tree. That is why
		 * we are able to create and configure GUIs in XML.
		 */
		win = new JFrame();
		win.setTitle("Data Structures & Algorithms 2023 - Morse Encoder/Decoder");
		win.setSize(650, 400);
		win.setResizable(false);
		win.setLayout(new FlowLayout());

        /*
         * The top panel will contain the file chooser and encode / decode buttons
         */
        var top = new JPanel(new FlowLayout(FlowLayout.LEADING));
        top.setBorder(new javax.swing.border.TitledBorder("Select File"));
        top.setPreferredSize(new Dimension(600, 80));

        txtFilePath =  new JTextField(20);
		txtFilePath.setPreferredSize(new Dimension(100, 30));


		var chooser = new JButton("Browse...");
		chooser.addActionListener((e) -> {

			// Clear whatever text is already there
			txtOutput.setText("");

			var fc = new JFileChooser(System.getProperty("user.dir"));
			var val = fc.showOpenDialog(win);
			if (val == JFileChooser.APPROVE_OPTION) {
				var file = fc.getSelectedFile().getAbsoluteFile();
				txtFilePath.setText(file.getAbsolutePath());

				// Set the File Chosen by the user
				fileChosen = file.getAbsolutePath();
			}
		});

		var btnEncodeFile = new JButton("Encode");
		btnEncodeFile.addActionListener(e -> {

			// Clear whatever text is already there
			txtOutput.setText("");

			/*
			 * Encode Steps
			 */
			try {
				// Start the timer to see how long it took to encode
				var start = System.currentTimeMillis();

				// Create a Map<Integer, String> version of the file
				Map<Integer, String> mappedFile = FileInput.convertFileToMap(fileChosen);

				// Convert the File to Morse Code
				morseConvertedString = MorseEncoder.encodeToMorse(mappedFile);

				// Stop the Timer once encoding is finished
				double searchTime = ((System.currentTimeMillis() - start));
				String timeTaken = ("Encoded text file in: " + new DecimalFormat("#,###.00").format(searchTime) + "ms.");

				// Add coverted string to the GUI Window Text area
				appendText(timeTaken + "\n" + morseConvertedString);

				// Output the encoded string to a text file
				FileOutput.writeStringToFile(localMorseEncodedFile, morseConvertedString);

			} catch (IOException exception) {
				System.out.println("File does not exist. Please try again!");
				exception.printStackTrace();
			}
		});

		var btnDecodeFile = new JButton("Decode");
		btnDecodeFile.addActionListener(e -> {

			// Clear whatever text is already there
			txtOutput.setText("");

			/*
			 * Decode Steps
			 */

			try {
				// Start the timer to see how long it took to decode
				var start = System.currentTimeMillis();

				// Create a Map<Integer, String> version of the local encoded morse file
				Map<Integer, String> mappedFile = FileInput.convertFileToMap(localMorseEncodedFile);

				// Decode the already encoded morse file "localEncodedFile.txt" to English
				englishConvertedString = MorseDecoder.decodeMorseToEnglish(mappedFile);

				// Stop the Timer once encoding is finished
				double searchTime = ((System.currentTimeMillis() - start));
				String timeTaken = ("Decoded text file in: " + new DecimalFormat("#,###.00").format(searchTime) + "ms.");

				// Add coverted string to the GUI Window Text area
				replaceText(timeTaken + "\n" + englishConvertedString);

				// Output the decoded String to a text file
				FileOutput.writeStringToFile(localMorseDecodedFile, englishConvertedString);

			} catch (IOException exception) {
				System.out.println("File does not exist. Please try again!");
				exception.printStackTrace();
			}
		});

		// Add all the components to the panel and the panel to the window
        top.add(txtFilePath);
        top.add(chooser);
        top.add(btnEncodeFile);
        top.add(btnDecodeFile);
        win.getContentPane().add(top); // Add the panel to the window

        /*
         * The middle panel contains the coloured square and the text
         * area for displaying the outputted text.
         */
        var middle = new JPanel(new FlowLayout(FlowLayout.LEADING));
        middle.setPreferredSize(new Dimension(600, 200));

        var dot = new JPanel();
		JLabel colourName = new JLabel();
		colourName.setText("Click Me");
		JTextArea colourNameInMorse = new JTextArea();

		// Add the Label that shows the name of the random colour
		dot.add(colourName);

		// Add the Text Area that shows the morse version of the random colour
		dot.add(colourNameInMorse);

        dot.setBorder(new SoftBevelBorder(SoftBevelBorder.RAISED));

        dot.setPreferredSize(new Dimension(140, 150));
        dot.addMouseListener(new MouseAdapter() {
        	// Can't use a lambda against MouseAdapter because it is not a SAM
        	public void mousePressed( MouseEvent e ) {

				// When the Panel dot is clicked
        		dot.setBackground(getRandomColour());	// Set Panels background to a random colour
				colourName.setText(nameOfColour); // Set the text to the name of that random colour
				colourNameInMorse.setText(nameOfColourInMorse); // Set the text to the morse version of the name of that random colour
        	}
        });

        // Add the text area
		txtOutput.setLineWrap(true);
		txtOutput.setWrapStyleWord(true);
		txtOutput.setBorder(new SoftBevelBorder(SoftBevelBorder.LOWERED));

		var scroller = new JScrollPane(txtOutput);
		scroller.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
		scroller.setPreferredSize(new Dimension(450, 150));
		scroller.setMaximumSize(new Dimension(450, 150));

		// Add all the components to the panel and the panel to the window
		middle.add(dot);
		middle.add(scroller);
		win.getContentPane().add(middle);

		/*
		 * The bottom panel contains the clear and quit buttons.
		 */
		var bottom = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        bottom.setPreferredSize(new java.awt.Dimension(500, 50));

        // Create and add Clear and Quit buttons
        var clear = new JButton("Clear");
        clear.addActionListener((e) -> txtOutput.setText(""));

        var quit = new JButton("Quit");
        quit.addActionListener((e) -> System.exit(0));

        // Add all the components to the panel and the panel to the window
        bottom.add(clear);
        bottom.add(quit);
        win.getContentPane().add(bottom);

        /*
         * All done. Now show the configured Window.
         */
		win.setVisible(true);
	}

	private Color getRandomColour() {
		Colour colour = colours[rand.nextInt(0, colours.length)]; // Generate a random colour

		nameOfColour = getRandomColourName(colour); // Get the name of the random colour and assign to varaiable
		nameOfColourInMorse = getMorseCodeOfRandomColourName(nameOfColour); // convert the name of the random colour to morse and assign to varaiable

		return Color.decode(colour.hex() + "");
	}

	private String getRandomColourName(Colour colour){
		return String.format(colour.name().toString()); // returns the name of the random colour
	}

	private String getMorseCodeOfRandomColourName(String colourName){
		return  MorseEncoder.encodeColourName(colourName); // returns the converted name of the random colour from the string inputted
	}

	protected void replaceText(String text) {
		txtOutput.setText(text);
	}

	protected void appendText(String text) {
		txtOutput.setText(txtOutput.getText() + " " + text);
	}
}
