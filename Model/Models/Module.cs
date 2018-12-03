using DriveCentric.Core.Interfaces;
using DriveCentric.Model.Enums;
using ServiceStack.DataAnnotations;
using System;

namespace DriveCentric.Core.Models
{
    public class Module : IBaseModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public StoreCategory Category { get; set; }
        public string Icon { get; set; }

        public float Cost { get; set; }

        public bool IsOwned { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}