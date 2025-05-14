using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Used for determing what text to display in our "grabbed butterfly menu"
public class InfoTextManager : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI infoTextTitle;
    public string infoTextKey;
    public TextAsset infoFile;

    //We handle our text inside a text asset that is read into this dictionary, with a given key
    private Dictionary<string, string> paragraphs = new Dictionary<string, string>();

    void Start()
    {
        LoadParagraphs();
    }

    public void AssignInfoText(string key)
    {
        //key format is all uppercase
        //capitalize the key we are sending in, just in case it isn't already 
        infoTextKey = key.ToUpper();

        //get the proper paragraph from our dictionary that matches our key
        string paragraph = GetParagraph(infoTextKey);

        //Set the title of our display frame
        //Can be done through the text asset/dictionary, I was just lazy
        if (paragraph != null)
        {
            switch (infoTextKey)
            {
                case "BLUE":
                    infoTextTitle.text = "Blue Morpho";
                    break;
                case "LEOPARD":
                    infoTextTitle.text = "Leopard Lacewing";
                    break;
                case "MALA":
                    infoTextTitle.text = "Malachite";
                    break;
                case "PAPER":
                    infoTextTitle.text = "Paper Kite";
                    break;
                case "POST":
                    infoTextTitle.text = "Postman";
                    break;
                case "TIGER":
                    infoTextTitle.text = "Tiger Longwing";
                    break;
                case "HEAD":
                    infoTextTitle.text = "Head";
                    break;
                case "THORAX":
                    infoTextTitle.text = "Thorax";
                    break;
                case "ABDOMEN":
                    infoTextTitle.text = "Abdomen";
                    break;
                case "LEGS":
                    infoTextTitle.text = "Legs";
                    break;
                case "FOREWING":
                    infoTextTitle.text = "Forewings";
                    break;
                case "HINDWING":
                    infoTextTitle.text = "Hindwings";
                    break;

            }

            //Set our TextMesh text to display our "paragraph" string
            infoText.text = paragraph;
        }
    }

    void LoadParagraphs()
    {
        //Can we find the text file asset?
        if (infoFile == null)
        {
            Debug.LogError("Text file is not assigned.");
            return;
        }

        //split our text asset into individual lines, the format is as goes:
        //KEY
        //paragraph
        //KEY
        //paragraph (etc...)
        string[] lines = infoFile.text.Split('\n');
        string currentKey = null;
        string currentParagraph = "";

        //Loop through our lines and find our keys and their associated paragraphs
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            //Skip empty lines
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            //Check if current line is a key
            if (IsKey(line))
            {
                //Save the previous paragraph to the previous key if it exists
                if (currentKey != null)
                {
                    paragraphs[currentKey] = currentParagraph.Trim();
                }

                //Set the new key and start a new paragraph
                currentKey = line;
                currentParagraph = "";
            }
            else
            {
                //Append to the current paragraph if we are not at a key
                currentParagraph += line + "\n";
            }
        }

        //Save the last paragraph
        if (currentKey != null)
        {
            paragraphs[currentKey] = currentParagraph.Trim();
        }

    }

    //Helper Method to determine if a line is a key
    bool IsKey(string line)
    {
        //Check if line is all uppercase and not too short (assuming keys are distinct and formatted properly)
        return line.Length > 1 && line.ToUpper() == line && !line.Contains(":");
    }

    //get the proper paragrapgh from our dictionary based on the key we send in
    public string GetParagraph(string key)
    {
        if (paragraphs.TryGetValue(key, out string paragraph))
        {
            return paragraph;
        }
        else
        {
            Debug.LogWarning("Key not found: " + key);
            return null;
        }
    }
}
