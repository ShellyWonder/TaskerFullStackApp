using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskerFullStackApp.Client.Models;
using TaskerFullStackApp.Data;
using TaskerFullStackApp.Models;

namespace TaskerFullStackApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskerItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        #region HELPER METHODS
        private string CurrentUserId => _userManager.GetUserId(User)!;
        private async Task<DbTaskerItem?> FindItemAsync(Guid id) => await _context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == CurrentUserId);
        #endregion

        #region GET ENDPOINT
        // GET: api/TaskerItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskerItem>>>GetTaskerItems()
        {
           
            List<DbTaskerItem> taskerItems = await _context.TaskerItems
                                                         .Where(t => t.UserId == CurrentUserId)
                                                         .ToListAsync();
            return taskerItems;
        }
        #endregion
        #region GET ENDPOINT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskerItem>> GetTaskerItem([FromRoute] Guid id) 
        {
            //get the taskerItem from the database
            var dbItem = await FindItemAsync(id);
            //check if the taskerItem exists in the database
            if (dbItem == null) return NotFound();
            //return the taskerItem to the client
            return dbItem;
        }
        #endregion
        #region POST ENDPOINT
        // POST: api/TaskerItem
        [HttpPost]
        public async Task<ActionResult<TaskerItem>> PostDbTaskerItem([FromBody] TaskerItem item)
        {
            DbTaskerItem dBTaskerItem = new();

            //dBTaskerItem.Id = item.Id;--set automatically by the database
            dBTaskerItem.Name = item.Name;
            dBTaskerItem.IsComplete = item.IsComplete;
            //derived from the user log in
            string userId = _userManager.GetUserId(User)!;
            //set the userId to the logged in user by the system
            dBTaskerItem.UserId = userId;

            //add the item to the database
            _context.TaskerItems.Add(dBTaskerItem);
            //save the changes to the database
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTaskerItem", new {id= dBTaskerItem.Id}, dBTaskerItem);
        }
        #endregion

        #region PUT ENDPOINT - REPLACE THE TASKER ITEM WITH THE NEW ONE
        // PUT: api/TaskerItem
        [HttpPut("{id}")]
        public async Task<ActionResult> PutDbTaskerItem([FromRoute] Guid id, [FromBody] TaskerItem taskerItem) 
        {
            if (id != taskerItem.Id) return BadRequest();

            
            //get the taskerItem from the database
            var dbItem = await FindItemAsync(id);

            //check if the taskerItem exists in the database
            if (dbItem == null) return NotFound();

            //update the taskerItem with the new values
            dbItem.Name = taskerItem.Name;
            dbItem.IsComplete = taskerItem.IsComplete;//changes to item id and item userId are not allowed

            //save the changes to the database
            _context.Update(dbItem);
            await _context.SaveChangesAsync();
            return NoContent();

        }
        #endregion

        #region DELETE ENDPOINT
        // DELETE: api/TaskerItem{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDbTaskerItem([FromRoute] Guid id) 
        {
            
           var dbItem = await FindItemAsync(id);
            if (dbItem == null) return NotFound();
            _context.TaskerItems.Remove(dbItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }

}
