using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoicevoxCoreSharp.Core;
using VoicevoxCoreSharp.Core;
using VoicevoxCoreSharp.Core.Struct;
using VoicevoxCoreSharp.Core.Enum;
using System.Diagnostics;
using NAudio.Wave;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml;

namespace Yomiage.Services
{
    class VoiceVoxService
    {
        private readonly Synthesizer synthesizer;
        private readonly uint VOICEVOX_MODEL_STYLE_ID = 3;

        public VoiceVoxService() {
            synthesizer = InitSynthesizer();
        }

        private Synthesizer InitSynthesizer()
        {
            string openJTalkDictPath = System.IO.Path.Combine(AppContext.BaseDirectory, "voicevox_core", "open_jtalk_dic_utf_8-1.11");
            var initializeOptions = InitializeOptions.Default();
            var result = OpenJtalk.New(openJTalkDictPath, out var openJtalk);
            if (result != ResultCode.RESULT_OK)
            {
                throw new Exception(result.ToMessage()); // TODO エラーハンドリング
            }
            result = Synthesizer.New(openJtalk, initializeOptions, out var synthesizer);
            if (result != ResultCode.RESULT_OK)
            {
                throw new Exception(result.ToMessage()); // TODO エラーハンドリング
            }
            string voiceModelPath = System.IO.Path.Combine(AppContext.BaseDirectory, "voicevox_core", "model", "0.vvm");
            result = VoiceModel.New(voiceModelPath, out var voiceModel);
            if (result != ResultCode.RESULT_OK)
            {
                throw new Exception(result.ToMessage()); // TODO エラーハンドリング
            }
            result = synthesizer.LoadVoiceModel(voiceModel);
            if (result != ResultCode.RESULT_OK)
            {
                throw new Exception(result.ToMessage()); // TODO エラーハンドリング
            }
            return synthesizer;
        }

        private byte[] ConvertToWav(string text)
        {
            var result = synthesizer.Tts(text, VOICEVOX_MODEL_STYLE_ID, TtsOptions.Default(), out var outputWavSize, out var outputWav);
            if (result != ResultCode.RESULT_OK || outputWav == null)
            {
                throw new Exception(result.ToString() ?? "outputWav is null."); // TODO エラーハンドリング
            }
            return outputWav;
        }

        private async Task PlayWav(byte[] wav)
        {
            using var memoryStream = new MemoryStream(wav);
            using var waveStream = new WaveFileReader(memoryStream);
            using var player = new WaveOut();
            var tcs = new TaskCompletionSource();
            player.Init(waveStream);
            player.PlaybackStopped += (sender, e) => {
                tcs.TrySetResult();
            };
            player.Play();
            await tcs.Task;
        }


        public async Task PlayTexts(List<string> texts, TextBlock textBlock)
        {
            List<TaskCompletionSource<byte[]>> results = texts.Select(_ => new TaskCompletionSource<byte[]>()).ToList();
            _ = Task.Run(async () =>
            {
                await Parallel.ForEachAsync(Enumerable.Range(0, texts.Count), async (i, _) =>
                {
                    try
                    {
                        var wav = await Task.Run(() => ConvertToWav(texts[i]));
                        results[i].SetResult(wav);
                    }
                    catch (Exception ex)
                    {
                        results[i].SetException(ex);
                    }
                });
            });
            for (int i = 0; i < texts.Count; i++)
            {
                textBlock.Text = texts[i];
                var wav = await results[i].Task;
                await PlayWav(wav);
            }
            textBlock.Text = "";
        }
    }
}
