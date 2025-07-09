using System.IO;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

//This code is entirely borrowed from the YouTube channel "Sarge"
//https://www.youtube.com/watch?v=rdHqRRzltTo

//Text to speech used for the tutorial subtitles, uses AmazonWebServices' Polly, likely to be replaced by Meta's Voice SDK
public class TextToSpeech : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    // Start is called before the first frame update
    public async Task Speak(string tts)
    {
    //    //This is a very dangerous way of accessing credentials, on top of that these are set to the root user of my AWS account. ***MUST*** BE CHANGED ASAP
    //    var credentials = new BasicAWSCredentials("", "");
    //    var client = new AmazonPollyClient(credentials, RegionEndpoint.USEast2);

    //    var request = new SynthesizeSpeechRequest()
    //    {
    //        Text = tts,
    //        Engine = Engine.Standard,
    //        VoiceId = VoiceId.Matthew,
    //        OutputFormat = OutputFormat.Mp3
    //    };

    //    var response = await client.SynthesizeSpeechAsync(request);

    //    WriteIntoFile(response.AudioStream);

    //    using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
    //    {
    //        var op = www.SendWebRequest();

    //        while (!op.isDone) await Task.Yield();

    //        var clip = DownloadHandlerAudioClip.GetContent(www);

    //        audioSource.clip = clip;
    //        audioSource.Play();   
    //    }
    //}

    //private void WriteIntoFile(Stream stream)
    //{
    //    using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
    //    {
    //        byte[] buffer = new byte[8 * 1024];
    //        int bytesRead;

    //        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            fileStream.Write(buffer, 0, bytesRead);
    //        }
    //    }
    }

}
