using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class AddressDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int? ParentId { get; set; }

        public AddressDto[] Childs { get; set; }
    }
}
