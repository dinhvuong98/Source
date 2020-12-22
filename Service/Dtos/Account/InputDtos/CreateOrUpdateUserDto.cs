using Services.Dto.Shared;
using System;

namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class CreateOrUpdateUserDto
    {
        public Guid? Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ShortGroupDto Group { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DictionaryItemDto District { get; set; }

        public DictionaryItemDto Commune { get; set; }

        public DictionaryItemDto Province { get; set; }

        public string Address { get; set; }

        public long? ModifiedAt { get; set; }
    }
}
