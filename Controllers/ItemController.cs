using DapperComPostgresql.Models;
using DapperComPostgresql.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DapperComPostgresql.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemRepository _itemRepository;

        public ItemsController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _itemRepository.GetAllItemsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _itemRepository.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddItem(Item item)
        {
            var id = await _itemRepository.AddItemAsync(item);
            return CreatedAtAction(nameof(GetItem), new { id = id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _itemRepository.UpdateItemAsync(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemRepository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
