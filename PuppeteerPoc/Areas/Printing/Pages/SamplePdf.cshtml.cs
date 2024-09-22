using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PuppeteerPoc.Areas.Printing.Pages;

public class SamplePdf : PageModel
{
    public void OnGet()
    {
        People =
        [
            new Person { Id = Guid.NewGuid(), Name = "Alice", IsSelected = true },
            new Person { Id = Guid.NewGuid(), Name = "Bob", IsSelected = false },
            new Person { Id = Guid.NewGuid(), Name = "Charlie", IsSelected = true },
            new Person { Id = Guid.NewGuid(), Name = "David", IsSelected = false },
            new Person { Id = Guid.NewGuid(), Name = "Eve", IsSelected = true }
        ];
    }

    public List<Person> People { get; set; }
    
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}