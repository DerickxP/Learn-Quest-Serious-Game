using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] wavFileData)
    {
        int headerOffset = 44; 
        int subChunk2Size = BitConverter.ToInt32(wavFileData, 40); 
        int sampleCount = subChunk2Size / 2; 

        float[] audioData = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            short sample = BitConverter.ToInt16(wavFileData, headerOffset + i * 2);
            audioData[i] = sample / 32768.0f;
        }

        int sampleRate = BitConverter.ToInt32(wavFileData, 24);
        int channels = BitConverter.ToInt16(wavFileData, 22);

        AudioClip audioClip = AudioClip.Create("GeneratedAudioClip", sampleCount, channels, sampleRate, false);
        audioClip.SetData(audioData, 0);

        return audioClip;
    }

    public static byte[] FromAudioClip(AudioClip clip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            int headerSize = 44;
            stream.Position = headerSize;

            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);

            int sampleCount = samples.Length;
            int sampleRate = clip.frequency;
            int channels = clip.channels;
            int byteCount = sampleCount * channels * 2;

            byte[] bytes = new byte[byteCount];
            int rescaleFactor = 32767;
            int byteIndex = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                short value = (short)(samples[i] * rescaleFactor);
                bytes[byteIndex++] = (byte)(value & 0x00ff);
                bytes[byteIndex++] = (byte)((value & 0xff00) >> 8);
            }

            stream.Write(bytes, 0, bytes.Length);

            stream.Position = 0;
            stream.Write(WavHeader(bytes.Length, sampleRate, channels), 0, headerSize);

            return stream.ToArray();
        }
    }

    private static byte[] WavHeader(int dataLength, int sampleRate, int channels)
    {
        byte[] header = new byte[44];
        int totalDataLen = dataLength + 44 - 8;
        int byteRate = sampleRate * channels * 2;
        short blockAlign = (short)(channels * 2);
        short bitsPerSample = 16;

        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, header, 0, 4);
        BitConverter.GetBytes(totalDataLen).CopyTo(header, 4);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, header, 8, 4);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, header, 12, 4);
        BitConverter.GetBytes(16).CopyTo(header, 16);
        BitConverter.GetBytes((short)1).CopyTo(header, 20);
        BitConverter.GetBytes((short)channels).CopyTo(header, 22);
        BitConverter.GetBytes(sampleRate).CopyTo(header, 24);
        BitConverter.GetBytes(byteRate).CopyTo(header, 28);
        BitConverter.GetBytes(blockAlign).CopyTo(header, 32);
        BitConverter.GetBytes(bitsPerSample).CopyTo(header, 34);
        Buffer.BlockCopy(System.Text.Encoding.UTF8.GetBytes("data"), 0, header, 36, 4);
        BitConverter.GetBytes(dataLength).CopyTo(header, 40);

        return header;
    }
}
