
using KafainExam.Entity;
using KafainExam.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KafainExam.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _toDoItemService;
        public TaskController(ITaskService toDoItemService)
        {
            _toDoItemService = toDoItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetToDoItems(CancellationToken cancellationToken)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null)
                return Ok("Lütfen Giriş yapınız");
            var toDoItems = await _toDoItemService.GetToDoItemsByUsername(username, cancellationToken);
            return Ok(toDoItems);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoItem(ToDoItemCreateRequest toDoItemDto, CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("lütfen giriş yapınız."); // Kullanıcı oturum açmamışsa izinsiz istek döndürün
            }
            var item = new TaskEntity()
            {
                Description = toDoItemDto.Description,
                Status = toDoItemDto.Status,
                Title = toDoItemDto.Title,
                UserId = userId
            };
            await _toDoItemService.CreateToDoItem(item, cancellationToken);
            return Ok("Task Başarılı bir şekilde eklendi.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem(int id, ToDoItemCreateRequest toDoItemDto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = new TaskEntity()
            {
                Id = id,
                Description = toDoItemDto.Description,
                Status = toDoItemDto.Status,
                Title = toDoItemDto.Title,
                UserId = userId
            };

            if (userId == null)
            {
                return Unauthorized("lütfen giriş yapınız.");
            }

            var updatedToDoItem = await _toDoItemService.UpdateToDoItem(id, item, cancellationToken);
            if (updatedToDoItem == null)
            {
                return NotFound("Task bulunamadı.");
            }
            return Ok("Task başarıyla güncellendi.");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteToDoItem(ToDoItemDeleteRequest id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("lütfen giriş yapınız.");

            var updatedToDoItem = await _toDoItemService.DeleteToDoItem(id, userId, cancellationToken);
            
            if (!updatedToDoItem)
                return NotFound("Task bulunamadı.");

            return Ok("Task başarıyla silindi.");
        }
    }
    public class ToDoItemCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState Status { get; set; } 
    }

    public class ToDoItemDeleteRequest
    {
        public int Id { get; set; }
    }

}
