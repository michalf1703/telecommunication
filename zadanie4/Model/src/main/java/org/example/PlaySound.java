package org.example;

import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.Clip;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.UnsupportedAudioFileException;
import java.io.File;
import java.io.IOException;

public class PlaySound {

    private Clip clipToPlay;

    private SoundPlayerThread soundPlayerThread;

    public void playSound(String fileName) {
        soundPlayerThread = new SoundPlayerThread(fileName);
        soundPlayerThread.start();
    }

    public void stopPlayingSound() {
        soundPlayerThread.interrupt();
    }


    private class SoundPlayerThread extends Thread {

        private String fileName;
        private File recordedFile;

        public SoundPlayerThread(String fileName) {
            this.fileName = fileName;
            recordedFile = new File(fileName);
        }

        @Override
        public void start() {
            super.start();
        }

        @Override
        public void interrupt() {
            clipToPlay.stop();
            clipToPlay.close();
        }

        @Override
        public void run() {
            try {
                clipToPlay = AudioSystem.getClip();
                clipToPlay.open(AudioSystem.getAudioInputStream(recordedFile));    //Otwiera plik dźwiękowy, rozpoczyna odtwarzanie i oczekuje, aż odtwarzacz osiągnie koniec pliku.
                clipToPlay.start();
                while (clipToPlay.getFramePosition() <= clipToPlay.getFrameLength() && clipToPlay.isOpen()) {}
                clipToPlay.stop();
                clipToPlay.close();                                                //Następnie zatrzymuje odtwarzacz i zamyka go.
            } catch (UnsupportedAudioFileException | LineUnavailableException | IOException e) {
                e.printStackTrace();
            }
        }
    }
}
