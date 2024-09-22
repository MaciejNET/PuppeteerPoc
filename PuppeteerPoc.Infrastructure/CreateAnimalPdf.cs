using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PuppeteerPoc.Infrastructure;

public class CreateAnimalPdf(HtmlRenderer htmlRenderer)
{
    public async Task<byte[]> ExecuteAsync()
    {
        var animals = GenerateAnimals();
        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?>
            {
                { "AnimalModels", animals }
            };

            var parameters = ParameterView.FromDictionary(dictionary);
            var output = await htmlRenderer.RenderComponentAsync<Animals>(parameters);

            return output.ToHtmlString();
        });
        
        var footer = await GetFooterHtmlAsync();
        
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        await page.AddStyleTagAsync(new AddTagOptions { Path = "../PuppeteerPoc/wwwroot/bootstrap/bootstrap.min.css" });
        await page.EvaluateExpressionHandleAsync("document.fonts.ready");
        var pdf = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
            DisplayHeaderFooter = true,
            FooterTemplate = footer,
            MarginOptions = new MarginOptions
            {
                Bottom = "2cm"
            }
        });
        await browser.CloseAsync();
        return pdf;
    }
    
    private async Task<string> GetFooterHtmlAsync()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../PuppeteerPoc.Infrastructure", "Footer.html");
        return await File.ReadAllTextAsync(filePath);
    }
    
    private List<Animal> GenerateAnimals()
    {
        return
        [
            new Animal
            {
                Name = "Dog",
                Age = 5,
                Species = "Canis lupus familiaris",
                Weight = 20.0,
                IsDomestic = true
            },

            new Animal
            {
                Name = "Cat",
                Age = 3,
                Species = "Felis catus",
                Weight = 10.0,
                IsDomestic = true
            },

            new Animal
            {
                Name = "Lion",
                Age = 10,
                Species = "Panthera leo",
                Weight = 250.0,
                IsDomestic = false
            }
        ];
    }
}