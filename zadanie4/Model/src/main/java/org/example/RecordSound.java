package org.example;

import javax.sound.sampled.*;
import java.io.File;
import java.io.IOException;

public class RecordSound {
    private AudioFormat audioFormat;
    private DataLine.Info dataLineInfo;
    private TargetDataLine targetDataLine;
    private float sampleRate;
    private int sameSizeInBits;
    private int channels;

    public RecordSound(float sampleRate, int sameSizeInBits, int channels) throws LineUnavailableException {
        this.sampleRate = sampleRate;
        this.sameSizeInBits = sameSizeInBits;
        this.channels = channels;
        audioFormat = new AudioFormat(sampleRate, sameSizeInBits, channels, true, false);
        dataLineInfo = new DataLine.Info(TargetDataLine.class, audioFormat);
        targetDataLine = (TargetDataLine) AudioSystem.getLine(dataLineInfo);
    }

    public void recordSound(String fileName) {
        SoundRecorderThread soundRecorderThread = new SoundRecorderThread(fileName); //Metoda recordSound tworzy nowy wątek SoundRecorderThread i rozpoczyna jego działanie.
        soundRecorderThread.start(); // Wątek ten jest odpowiedzialny za nagrywanie dźwięku.
    }

    public void closeTargetDataLine() {
        targetDataLine.stop();
        targetDataLine.close();
    }


    private class SoundRecorderThread extends Thread {

        String fileName;

        public SoundRecorderThread(String fileName) {
            this.fileName = fileName;
        }

        @Override
        public void start() {
            super.start();
        }

        @Override
        public void run() { // W metodzie run otwierana jest linia danych docelowej, uruchamiana i zapisywana do pliku przy użyciu AudioSystem.write.
            AudioFileFormat.Type fileFormat = AudioFileFormat.Type.WAVE;
            File recordedFile = new File(fileName);
            try {
                targetDataLine.open(audioFormat);
                targetDataLine.start();
                AudioSystem.write(new AudioInputStream(targetDataLine), fileFormat, recordedFile);
            } catch (LineUnavailableException | IOException e) {
                e.printStackTrace();
            }
        }
    }
}
