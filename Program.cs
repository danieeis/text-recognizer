// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


string text = File.ReadAllText(@"./json2.json");
var json = JsonSerializer.Deserialize<List<OCRDocumentResponse>>(text);
var fullText = json.FirstOrDefault().FullTextAnnotation.Text;
var pages = json.FirstOrDefault().FullTextAnnotation.Pages;
// Console.WriteLine(fullText);

List<Word> Words = new();
foreach (var page in pages)
{
    var pageText = "";
    foreach (var block in page.Blocks)
    {
        var blockText = "";
        foreach (var paragraph in block.Paragraphs)
        {
            var paraText = "";
            foreach (var word in paragraph.Words)
            {
                var wordText = "";
                foreach (var symbol in word.Symbols)
                {
                    wordText += symbol.Text;
                }
                paraText = string.Format("{0} {1}", paraText, wordText);
                word.Text = wordText;
                Words.Add(word);
            }
            blockText += paraText;
        }
        pageText += blockText;
    }
}

Dictionary<string, string> fieldsToScan = new Dictionary<string, string>()
{
    { "field_votos_candidato_1", "CALECA" },
    { "field_votos_candidato_2", "VELASQUEZ" },
    { "field_votos_candidato_3", "RADONSKI" },
    { "field_votos_candidato_4", "PROSPERI" },
    { "field_votos_candidato_5", "ALMEIDA" },
    { "field_votos_candidato_6", "VIVAS" },
    { "field_votos_candidato_7", "DELSA" },
    { "field_votos_candidato_8", "FREDDY" },
    { "field_votos_candidato_9", "GLORIA" },
    { "field_votos_candidato_10", "LUIS" },
    { "field_votos_candidato_11", "MACHADO" },
    { "field_votos_candidato_12", "ROBERTO" },
    { "field_votos_candidato_13", "TAMARA" },
    { "field_boletas_escrutadas", "ESCRUTADAS" },
    { "field_participantes_segun_cuader", "PARTICIPANTES" },
    { "field_votos_nulos", "NULOS" },
    //{ "field_votos_nulos", "CIERRE DE MESA" },
    { "field_hora_fin_del_escrutinio", "ESCRUTINIO" }
};

foreach (var cand in fieldsToScan)
{
    var candidate = Words.FirstOrDefault(x => x.Text.ToUpper() == cand.Value.ToUpper());
    if (candidate != null)
    {

        var votesOfCandidate = Words.FirstOrDefault(x =>
        x.Text.ToUpper() != candidate.Text.ToUpper()

        && (x.BoundingBox.Vertices.Average(x => x.Y) + 30) >= (candidate.BoundingBox.Vertices.Average(x => x.Y))
        && (x.BoundingBox.Vertices.Average(x => x.Y)) <= (candidate.BoundingBox.Vertices.Average(x => x.Y) + 30)

        && (x.BoundingBox.Vertices.Average(x => x.X)) >= (candidate.BoundingBox.Vertices.Average(x => x.X) + 40)
        && (x.BoundingBox.Vertices.Average(x => x.X)) <= (candidate.BoundingBox.Vertices.Average(x => x.X) + 200)
        );

        var textFound = votesOfCandidate?.Text;
        if (int.TryParse(textFound, out int result))
        {
            Console.WriteLine($"{candidate.Text} | {result}");
        }
        else
        {
            var normalized = textFound?.Replace(":", "");
            if (int.TryParse(normalized, out int result2))
            {
                Console.WriteLine($"{candidate.Text} || {result2}");
            }
            else
            {
                Console.WriteLine($"Search word for {candidate.Text} not is number !!! | Result: {normalized}");
            }
        }
    }
    else
    {
        Console.WriteLine($"XXXXXX {cand.Value} not found");
    }
}