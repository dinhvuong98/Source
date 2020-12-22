using Services.Dto.Shared;
using System;

namespace Services.Dtos.Account
{
    [Serializable]
    public class DetailUserResultDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public GroupDto Group { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DictionaryItemDto District { get; set; }

        public DictionaryItemDto Commune { get; set; }

        public DictionaryItemDto Province { get; set; }

        public string Address { get; set; }

        public long LastTimeReadNotification { get; set; }

        public long ModifiedAt { get; set; }
    }
}
