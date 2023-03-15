using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace testingCognitive {
    class Program {
    static string speechKey = "mykey";
    static string speechRegion = "eastus";
        static async Task Main(string[] args)  {
            await OutputSpeechRecognitionResult();           
        }

        static async Task OutputSpeechRecognitionResult() {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);  
            speechConfig.SpeechRecognitionLanguage = "en-US";
            using var audioConfig = AudioConfig.FromWavFileInput("./audio/Michael_Caine_BBC_Radio4_Front_Row_29_Sept_2010.wav");
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            //speechRecognizer.Recognizing += (s, e) => {
            //    Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
            //};

            speechRecognizer.Recognized += (s, e) => {
                if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                    Console.WriteLine(e.Result.Text);
                }
                else if (e.Result.Reason == ResultReason.NoMatch) {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
            };

            speechRecognizer.Canceled += (s, e) => {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error) {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                }
            };

            await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            Console.WriteLine("Press enter to stop the program");
            Console.ReadKey();

            await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            Console.WriteLine("Speech recognition stopped.");

            return;
        }
    }
}
