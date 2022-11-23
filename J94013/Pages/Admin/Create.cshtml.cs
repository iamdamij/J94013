using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using J94013.Data1;
using J94013.Models;

namespace J94013.Pages.Menus
{
    public class CreateModel : PageModel
    {
        private readonly J94013.Data1.J94013DbContext _context;

        [BindProperty]
        public Menu Menu { get; set; }

        public CreateModel(J94013.Data1.J94013DbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            byte[] bytes = null;

            if (Menu.ImageFile != null)
            {
                using (Stream fs = Menu.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }
                Menu.ImgImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            _context.Menu.Add(Menu);
            _context.SaveChanges();


            return RedirectToPage("./Index");
        }
    }
}
