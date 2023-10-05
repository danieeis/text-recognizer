// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


string text = File.ReadAllText(@"./file.json");
var json = JsonSerializer.Deserialize<Root>(text);
var fullText = json.responses.FirstOrDefault().FullTextAnnotation.Text;
var pages = json.responses.FirstOrDefault().FullTextAnnotation.Pages;
Console.WriteLine(fullText);

// SearchTextDesc("VOTOS", 2, 1);
// SearchTextAsc("MESA: ", "MESA: ".Length, 2);
// var at = 670;
// Console.WriteLine($"Element at({at})=>{fullText.ElementAt(at)}");

void SearchTextAsc(string input, int marginRight, int lenght){
    var i = fullText.IndexOf(input);
    var startIndex = i+marginRight;
    var r = fullText.Substring(startIndex, lenght).Trim();
    Console.WriteLine("Input index: "+i);
    Console.WriteLine("Start index: "+startIndex);
    Console.WriteLine(input + "=>" + r);
}
void SearchTextDesc(string input, int marginLeft, int lenght){
    var i = fullText.IndexOf(input);
    var startIndex = i-marginLeft;
    var r = fullText.Substring(startIndex, lenght).Trim();
    Console.WriteLine("Input index: "+i);
    Console.WriteLine("Start index: "+startIndex);
    Console.WriteLine(input + "=>" + r);
}

List<Word> words = new();
foreach(var page in pages){
    var pageText = "";
    foreach (var block in page.Blocks) {
        var blockText = "";
        foreach (var paragraph in block.Paragraphs) {
            var paraText = "";
            foreach (var word in paragraph.Words) {
                var wordText = "";
                foreach (var symbol in word.Symbols) {
                    wordText += symbol.Text;
                    // Console.WriteLine(
                    //     string.Format(
                    //     "Symbol text: {0} (confidence: {1})%n",
                    //     symbol.Text,
                    //     symbol.Confidence
                    // ));
                }
                // Console.WriteLine(
                //     string.Format("\nWord text:\n {0}\nWord Confidence: {1})\n",
                //     wordText,
                //     word.Confidence)
                // );

                // Console.WriteLine(string.Format("Word bounding box: \n{0}", word.BoundingBox));
                paraText = string.Format("{0} {1}", paraText, wordText);
                word.Text = wordText;
                words.Add(word);
            }

            // Console.WriteLine(string.Format("\nParagraph:\n {0}", paraText));
            // Console.WriteLine(string.Format("Paragraph Confidence: {0}\n", paragraph.Confidence));
            // Console.WriteLine(string.Format("Paragraph bounding box: \n{0}", paragraph.BoundingBox));
            blockText += paraText;
        }
        
        // Console.WriteLine(string.Format("\nBlock:\n {0}", blockText));
        // Console.WriteLine(string.Format("nBlock Confidence: {0}\n", block.Confidence));
        // Console.WriteLine(string.Format("nBlock bounding box: \n{0}", block.BoundingBox));
        pageText += blockText;
    }
}


// var cand = words.FirstOrDefault(x => x.Text.ToUpper() == "ESCRUTADAS".ToUpper());
// Console.WriteLine(cand.Text);
// Console.WriteLine(string.Format("Field bounding box: \n{0}", cand.BoundingBox));
// var votos = words.FirstOrDefault(x => 
// x.Text != cand.Text 

// && x.BoundingBox.Vertices[0].Y+20 >= cand.BoundingBox.Vertices[0].Y
// && x.BoundingBox.Vertices[0].Y <= cand.BoundingBox.Vertices[0].Y+10

// && x.BoundingBox.Vertices.Average(x=>x.X) >= (cand.BoundingBox.Vertices.Average(x=>x.X)+100)
// && x.BoundingBox.Vertices.Average(x=>x.X) <= (cand.BoundingBox.Vertices.Average(x=>x.X)+200)
// );
// Console.WriteLine(votos.Text);
// Console.WriteLine(string.Format("Votos Field bounding box: \n{0}", votos.BoundingBox));
