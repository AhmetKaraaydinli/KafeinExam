using AutoMapper;
using KafainExam.Controller;
using KafainExam.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace KafainExam.Service
{
    public interface ITaskService
    {
        Task<List<TaskEntity>> GetToDoItemsByUsername(string userId, CancellationToken cancellationToken);
        Task<TaskEntity> CreateToDoItem(TaskEntity toDoItemDto, CancellationToken cancellationToken);
        Task<TaskEntity> UpdateToDoItem(int id, TaskEntity toDoItemDto, CancellationToken cancellationToken);
        Task<bool> DeleteToDoItem(ToDoItemDeleteRequest toToDeleteItem, string userId, CancellationToken cancellationToken);
    }
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaskEntity>> GetToDoItemsByUsername(string userId, CancellationToken cancellationToken)
        {
            var toDoItems = await _context.Tasks.Where(item => item.User.Id == userId).ToListAsync(cancellationToken);
            return _mapper.Map<List<TaskEntity>>(toDoItems);
        }

        public async Task<TaskEntity> CreateToDoItem(TaskEntity taskEntity, CancellationToken cancellationToken)
        {
            var toDoItem = _mapper.Map<TaskEntity>(taskEntity);
            _context.Tasks.Add(toDoItem);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TaskEntity>(toDoItem);
        }
        public async Task<bool> DeleteToDoItem(ToDoItemDeleteRequest deleteItem, string userId, CancellationToken cancellationToken)
        {
            var todoItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == deleteItem.Id && t.UserId == userId, cancellationToken);

            if (todoItem == null)
                return false;

            _context.Tasks.Remove(todoItem);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<TaskEntity> UpdateToDoItem(int id, TaskEntity taskEntity, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(item => item.Id == id && item.User.Id == taskEntity.UserId, cancellationToken);
            if (task == null)
            {
                return null;
            }

            _mapper.Map(taskEntity, task);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TaskEntity>(task);
        }
    }
}
