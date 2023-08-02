using Microsoft.AspNetCore.Identity;

namespace KafainExam.Entity
{
    public class User : IdentityUser
    {
        public List<TaskEntity> Tasks { get; set; }
    }

}
