package com.example.view;

import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.stage.FileChooser;
import org.example.PlaySound;
import org.example.RecordSound;
import org.example.exceptions.GuiException;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.Map;

public class ApplicationController {
    @FXML
    private ComboBox sampleRateRecord;
    @FXML
    private ComboBox sampleSizeInBitsRecord;
    @FXML
    private ComboBox channelsRecord;
    @FXML
    private ComboBox sampleRatePlay;
    @FXML
    private ComboBox sampleSizeInBitsPlay;
    @FXML
    private ComboBox channelsPlay;
    @FXML
    private TextField snrRecord;
    @FXML
    private TextField snrPlay;

    private Socket clientSocket;
    private ServerSocket serverSocket;
    private Socket actualServerSocket;

    private RecordSound recordSound;
    private PlaySound playSound;

    private PopUpWindow popUpWindow = new PopUpWindow();

    private float inputSampleRate;
    private float inputSampleRateForPlaying;
    private InputStream inputStream;
    private int inputSampleSizeInBits;
    private int inputSampleSizeInBitsForPlaying;
    private int inputNumberOfChannels;
    private int inputNumberOfChannelsForPlaying;
    private boolean isServer;

    private OutputStream outputStream;
    @FXML
    private ComboBox chooseFunction;
    private
    String serverIP;
    private
    String serverPort;
    private
    String serverPortNumber;

    @FXML
    public void initialize() {
        chooseFunction.getItems().addAll("Klient", "Serwer");
        chooseFunction.getSelectionModel().selectFirst();
        sampleRateRecord.getItems().addAll("8000", "11025", "16000", "22050", "32000", "44100", "48000", "88200", "96000", "176400", "192000");
        sampleSizeInBitsRecord.getItems().setAll("8", "16", "24", "32");
        channelsRecord.getItems().setAll("1", "2");

        sampleRatePlay.getItems().addAll("8000", "11025", "16000", "22050", "32000", "44100", "48000", "88200", "96000", "176400", "192000");
        sampleSizeInBitsPlay.getItems().setAll("8", "16", "24", "32");
        channelsPlay.getItems().setAll("1", "2");

        sampleRateRecord.getSelectionModel().select(6);
        sampleSizeInBitsRecord.getSelectionModel().select(1);
        channelsRecord.getSelectionModel().select(0);

        sampleRatePlay.getSelectionModel().select(6);
        sampleSizeInBitsPlay.getSelectionModel().select(1);
        channelsPlay.getSelectionModel().select(0);

        updateAllData();
        refreshValues();
    }

    private void updateSampleRate() {
        String sampleRateToString = sampleRateRecord.getSelectionModel().getSelectedItem().toString();
        inputSampleRate = Float.parseFloat(sampleRateToString);
    }

    private void updateSampleSizeInBits() {
        String sampleSizeInBitsToString = sampleSizeInBitsRecord.getSelectionModel().getSelectedItem().toString();
        inputSampleSizeInBits = Integer.parseInt(sampleSizeInBitsToString);
    }

    private void updateNumberOfChannels() {
        String numberOfChannelsToString = channelsRecord.getSelectionModel().getSelectedItem().toString();
        inputNumberOfChannels = Integer.parseInt(numberOfChannelsToString);
    }

    private void updateSampleRateForPlaying() {
        String sampleRateForPlayingToString = sampleRatePlay.getSelectionModel().getSelectedItem().toString();
        inputSampleRateForPlaying = Float.parseFloat(sampleRateForPlayingToString);
    }

    private void updateSampleSizeInBitsForPlaying() {
        String sampleSizeInBitsForPlayingToString = sampleSizeInBitsPlay.getSelectionModel().getSelectedItem().toString();
        inputSampleSizeInBitsForPlaying = Integer.parseInt(sampleSizeInBitsForPlayingToString);
    }

    private void updateNumberOfChannelsForPlaying() {
        String numberOfChannelsForPlayingToString = channelsPlay.getSelectionModel().getSelectedItem().toString();
        inputNumberOfChannelsForPlaying = Integer.parseInt(numberOfChannelsForPlayingToString);
    }

    private void updateAllData() {
        updateSampleRate();
        updateSampleSizeInBits();
        updateNumberOfChannels();

        updateSampleRateForPlaying();
        updateSampleSizeInBitsForPlaying();
        updateNumberOfChannelsForPlaying();
    }


    @FXML
    public void startRecording() throws GuiException {
        updateAllData();
        try {
            FileChooser fileChooser = new FileChooser();
            String fullFileName = fileChooser.showSaveDialog(StageSetup.getStage()).getAbsolutePath();
            if (fullFileName != null) {
                recordSound = new RecordSound(inputSampleRate, inputSampleSizeInBits, inputNumberOfChannels);
                recordSound.recordSound(fullFileName);
            } else {
                popUpWindow.showError("Nie ma takiego pliku.");
                throw new GuiException("Nie ma takiego pliku.");
            }
        } catch (Exception e) {
            popUpWindow.showError("Wybrano błędne parametry.");
            throw new GuiException("Wybrano błędne parametry.");
        }
    }

    @FXML
    public void stopRecording() throws GuiException {
        try {
            recordSound.closeTargetDataLine();
        } catch (Exception e) {
            popUpWindow.showError("Nagrywanie zostało już wstrzymane!");
            throw new GuiException("Nagrywanie zostało już wstrzymane!");
        }
    }

    @FXML
    public void startPlaying() throws GuiException {
        String fileName;
        FileChooser fileChooser = new FileChooser();
        fileName = fileChooser.showOpenDialog(StageSetup.getStage()).getAbsolutePath();
        if (fileName != null) {
            playSound = new PlaySound();
            playSound.playSound(fileName);
        } else {
            popUpWindow.showError("Nie ma takiego pliku.");
            throw new GuiException("Nie ma takiego pliku.");
        }
    }

    @FXML
    public void stopPlaying() throws GuiException {
        try {
            playSound.stopPlayingSound();
        } catch (Exception e) {
            popUpWindow.showError("Odtwarzanie zostało już wstrzymane!");
            throw new GuiException("Odtwarzanie zostało już wstrzymane!");
        }
    }

    @FXML
    public void showAuthors() {
        popUpWindow.showInfo("Michał Ferdzyn, Artur Grzybek");
    }


    @FXML
    public void start() {
        if (!isServer) {
            TextInputDialog textInput = new TextInputDialog();
            textInput.setHeaderText("Podaj adres IP serwera, z którym chcesz się połączyć.");
            textInput.setContentText("Adres IP: ");
            serverIP = textInput.showAndWait().get();
            textInput = new TextInputDialog();
            textInput.setHeaderText("Podaj numer portu serwera, z którym chcesz się połączyć.");
            textInput.setContentText("Numer portu: ");
            serverPort = textInput.showAndWait().get();

            try {
                clientSocket = new Socket(serverIP, Integer.parseInt(serverPort));
                popUpWindow.showInfo("Operacja nawiązania połączenia zakończona powodzeniem!");
            } catch (IOException ioException) {
                popUpWindow.showError("Nie można otworzyć gniazda");
            } catch (IllegalArgumentException argExc) {
                popUpWindow.showError("Zły numer portu");
            }
        } else {
            TextInputDialog textInput = new TextInputDialog();
            textInput.setHeaderText("Podaj numer portu serwera, z którym chcesz się połączyć.");
            textInput.setContentText("Numer portu: ");
            serverPortNumber = textInput.showAndWait().get();

            try {
                serverSocket = new ServerSocket(Integer.parseInt(serverPortNumber));
                actualServerSocket = serverSocket.accept();
                popUpWindow.showInfo("Operacja nawiązania połączenia zakończona powodzeniem!");
            } catch (IOException ioException) {
                popUpWindow.showError("Nie można otworzyć gniazda");
            } catch (IllegalArgumentException argExc) {
                popUpWindow.showError("Zły numer portu");
            }
        }
    }

    @FXML
    public void sendToHost() throws IOException {
        try {
            if (isServer) {
                outputStream = actualServerSocket.getOutputStream();
            } else {
                try {
                    outputStream = clientSocket.getOutputStream();
                } catch (NullPointerException e) {
                    throw new GuiException("Brak połączenia z gniazdem");
                }

            }

            FileChooser fileChooser = new FileChooser();
            String fullFileName = fileChooser.showSaveDialog(StageSetup.getStage()).getAbsolutePath();
            if (fullFileName != null) {
                File soundFile = new File(fullFileName);
                FileInputStream fileInput = new FileInputStream(soundFile);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = fileInput.read(buffer)) != -1) {
                    outputStream.write(buffer, 0, bytesRead);
                }
                fileInput.close();
                outputStream.close();
                clientSocket.close();
                actualServerSocket.close();

            } else {
                popUpWindow.showError("Nie ma takiego pliku.");
                throw new GuiException("Nie ma takiego pliku.");
            }
        } catch (
                IOException ioException) {
            popUpWindow.showError("Nie udało się wysłać pliku");
        }
    }

    @FXML
    public void receiveFromHost() throws IOException {
        try {
            if (isServer) {
                inputStream = actualServerSocket.getInputStream();
            } else {
                try {
                    inputStream = clientSocket.getInputStream();
                } catch (NullPointerException e) {
                    throw new GuiException("Brak połączenia z gniazdem");
                }
            }
            String fileName;
            FileChooser fileChooser = new FileChooser();
            fileName = fileChooser.showOpenDialog(StageSetup.getStage()).getAbsolutePath();

            if (fileName != null) {
                FileOutputStream fileOutput = new FileOutputStream(fileName);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = inputStream.read(buffer)) != -1) {
                    fileOutput.write(buffer, 0, bytesRead);
                }
                fileOutput.close();
                inputStream.close();
                actualServerSocket.close();
                serverSocket.close();
                popUpWindow.showInfo("Plik odebrany");
            } else {
                popUpWindow.showError("Nie ma takiego pliku.");
                throw new GuiException("Nie ma takiego pliku.");
            }

        } catch (IOException ioException) {
            popUpWindow.showError("Nie udało się otrzymać pliku");
        }
    }


    @FXML
    public void refreshValues() {
        updateAllData();
        double snrValue = 20 * Math.log10(Math.pow(2, inputSampleSizeInBits));
        snrRecord.setText(String.valueOf(snrValue));
        snrValue = 20 * Math.log10(Math.pow(2, inputSampleSizeInBitsForPlaying));
        snrPlay.setText(String.valueOf(snrValue));
        refreshValueForPlaying();
        refreshValueForRecording();
    }

    @FXML
    public void refreshValueForRecording() {
        updateSampleSizeInBits();
        double snrValue = 20 * Math.log10(Math.pow(2, inputSampleSizeInBits));
        snrRecord.setText(String.valueOf(snrValue));
    }

    @FXML
    public void refreshValueForPlaying() {
        updateSampleSizeInBitsForPlaying();
        double snrValue = 20 * Math.log10(Math.pow(2, inputSampleSizeInBitsForPlaying));
        snrPlay.setText(String.valueOf(snrValue));
    }

    @FXML
    public void changeFunction() {
        String function = chooseFunction.getSelectionModel().getSelectedItem().toString();
        if (function.equals("Klient")) {
            isServer = false;
        } else {
            isServer = true;
        }
    }

}