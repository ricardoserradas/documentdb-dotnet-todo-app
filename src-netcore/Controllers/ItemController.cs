namespace todo.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;
    
    public class ItemController : Controller
    {
        private DocumentDBRepository<Item> repository;
        private IConfiguration Configuration;

        public ItemController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.repository = new DocumentDBRepository<Item>(
                this.Configuration["endpoint"].ToString(),
                this.Configuration["authKey"].ToString(),
                this.Configuration["database"].ToString(), 
                this.Configuration["collection"].ToString()
            );
        }
        
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(){
            var items = await this.repository.GetItemsAsync(d => !d.Completed);
            return View(items);
        }
    }
}