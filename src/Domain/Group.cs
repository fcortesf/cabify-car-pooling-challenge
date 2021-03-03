using System;
using System.ComponentModel.DataAnnotations;

namespace car_pooling_api.Domain {
    public class Group {
        [Key]
        public int Id { get; set; }
        [Required]
        public int People { get; set; }
        public int? CarId { get; set; }
        public DateTime CreationDate {get; set;}
        public Car JourneyCar { get; set; }

        public Group()
        {
            CreationDate = DateTime.Now;
        }
    }
}