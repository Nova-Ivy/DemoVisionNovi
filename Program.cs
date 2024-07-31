using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

var imageUrl = "https://media.gettyimages.com/id/1285504723/vector/receipt-of-sale.jpg?s=2048x2048&w=gi&k=20&c=oBOaPWPkm8BEF3Q3aWaYnUTDeVxDNymMRrhPnYai23I=";
var client = new ComputerVisionClient(
    new ApiKeyServiceClientCredentials("a00a80b0f01b4350bf9248ca68e968ae"))
{ Endpoint = "https://testnovi.cognitiveservices.azure.com/" };

var headers = await client.ReadAsync(imageUrl);

int idLength = 36;
string operationId = headers.OperationLocation.Substring(headers.OperationLocation.Length - idLength);

ReadOperationResult results = new();
while ((results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted))
{
    results = await client.GetReadResultAsync(Guid.Parse(operationId));
}

foreach (ReadResult page in results.AnalyzeResult.ReadResults)
{
    foreach (Line line in page.Lines)
    {
        Console.WriteLine(line.Text);
    }
}
